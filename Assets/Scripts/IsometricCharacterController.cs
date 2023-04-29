using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class IsometricCharacterController : MonoBehaviour
{
    [SerializeField] float Speed = 8f;
    [SerializeField] float TurnSpeed = 5f;
    [SerializeField] float backwardsMovmentPenalty = 0.7f;
    [SerializeField] float sprintMultiplyer = 2f;
    [SerializeField] private AudioClip _walkSound;
    [SerializeField] private AudioClip _collectItem;
    [SerializeField] private AudioClip _dashSound;
    [SerializeField] private AudioClip[] _deathSound;
    [SerializeField] private AudioClip[] _takeDamageSound;
    [SerializeField] private float _pickUpRadius = 5f;
    [SerializeField] Transform CameraObject;
    [SerializeField] private float rotationSpeed = 2f;
    private PlayerLight playerLight;
  
    public UnityEvent OnStartMoving;
    public UnityEvent OnStopMoving;
    public UnityEvent OnStartDashing;
    public UnityEvent OnEndDashing;
    public UnityEvent OnUseItem;
    public UnityEvent EverySecond;


    public AudioSource _audioSourceMove;
    public AudioSource _audioSourceAction;

    private Animator _animator;
    PickupObject closestPickUp;

    private bool _inMove;
    private bool _isDead = false;
    private bool _isSprinting;
    float currentMoveSpeed = 0f;
    private void Start()
    {
        InitializeEvents();

        _animator = this.GetComponent<Animator>();

        InvokeRepeating(nameof(RunEverySecondEvent), 0, 1f);
        playerLight= this.GetComponent<PlayerLight>();

    }
    private void Update()
    {
        if (_isDead)
        {
            return;
        }
        if (GameManager.Instance.isGamePaused)
            return;
        if (playerLight.GetCurrentIntensity() <= 0f)
        {
            _isDead= true;
            return;
        }
        if(_isDead)
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetBool("IsRunning", false);
            _animator.CrossFade("Death",0.5f);
            
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       
        PickUp();
        MoveWASD();
        Look(ray);
        


    }

    private void LateUpdate()
    {
        if (_isDead)
        {
            return;
        }
        if (playerLight.GetCurrentIntensity() <= 0f)
        {
            _isDead = true;
            return;
        }
       // RotateCamera();
    }
    private void RunEverySecondEvent()
    {
        playerLight.ChangeCurrentIntensity(_isSprinting ? -playerLight.intensityTakenPerTick*3f : -playerLight.intensityTakenPerTick);
        EverySecond?.Invoke();
    }
    private void InitializeEvents()
    {
        if (OnStartMoving == null) OnStartMoving = new UnityEvent();
        if (OnStopMoving == null) OnStopMoving = new UnityEvent();
        if (OnStartDashing == null) OnStartDashing = new UnityEvent();
        if (OnUseItem == null) OnUseItem = new UnityEvent();
        if (EverySecond == null) EverySecond = new UnityEvent();

    }
    private void RotateCamera()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            float rotateAmount = rotationSpeed;

            // Calculate the new Y rotation of the virtual camera
            float yRotation = CameraObject.transform.localEulerAngles.y + rotateAmount;

            // Apply the new rotation to the virtual camera, while keeping its X and Z rotation values
            CameraObject.transform.localEulerAngles = new Vector3(60, yRotation, 0);
        }
        else if(Input.GetKey(KeyCode.E))
        {
            float rotateAmount = -rotationSpeed;

            // Calculate the new Y rotation of the virtual camera
            float yRotation = CameraObject.transform.localEulerAngles.y + rotateAmount;

            // Apply the new rotation to the virtual camera, while keeping its X and Z rotation values
            CameraObject.transform.localEulerAngles = new Vector3(60, yRotation, 0);
        }
    }
    private void Look(Ray ray)
    {

            Plane plane = new Plane(Vector3.up, transform.position);

            // Declare a variable to store the distance to the plane
            float distance;

            // If the ray intersects the plane, set the distance to the intersection point
            if (plane.Raycast(ray, out distance))
            {
                // Get the intersection point
                Vector3 mousePosition = ray.GetPoint(distance);


                // Calculate the rotation needed to look at the mouse position
                Quaternion rotation = Quaternion.LookRotation(mousePosition - transform.position);

                // Smoothly rotate towards the mouse position

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, TurnSpeed * Time.deltaTime);
            }
        
    }
    void MoveWASD()
    {
        var inputMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        float angle = Vector3.SignedAngle(transform.forward, inputMovement, Vector3.up);
        _isSprinting = Input.GetKey(KeyCode.LeftShift);
        if (inputMovement != Vector3.zero )
        {
            if (!_inMove) OnStartMoving?.Invoke();

           
            if(_isSprinting)
            {
                inputMovement = MathF.Abs(angle) > 100 ? inputMovement.normalized * Speed * backwardsMovmentPenalty * Time.deltaTime : inputMovement.normalized * Speed* sprintMultiplyer * Time.deltaTime;
                currentMoveSpeed = MathF.Abs(angle) > 100 ? Speed * backwardsMovmentPenalty : Speed*sprintMultiplyer;
                int runningStateHash = Animator.StringToHash("Base Layer.Running");
                if (!_animator.GetNextAnimatorStateInfo(0).IsName("Running"))
                {
                    _animator.CrossFade("Running", 0.1f);

                }
            }
            else
            {
                inputMovement = MathF.Abs(angle) > 100 ? inputMovement.normalized * Speed * backwardsMovmentPenalty * Time.deltaTime : inputMovement.normalized * Speed * Time.deltaTime;
                currentMoveSpeed = MathF.Abs(angle) > 100 ? Speed * backwardsMovmentPenalty : Speed;
                // _animator.SetBool("IsWalking", true);
                //  _animator.SetBool("IsRunning", false);
                if (!_animator.GetNextAnimatorStateInfo(0).IsName("Walk"))
                {
                    _animator.CrossFade("Walk", 0.1f);

                }
            }
            

            Quaternion rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            Vector3 rotatedMovement = rotation * inputMovement;

           // SoundManager.Instance.PlayOnGivenAudioSource(_audioSourceMove, _walkSound);
            

            transform.Translate(inputMovement, Space.World);


            

            _inMove = true;
            _animator.SetFloat("InputAngle", angle);
            Debug.Log("Angle: " + angle);
            //_animator.SetFloat("MoveSpeed", currentMoveSpeed / Speed);



        }
        else
        {
            if (_inMove)
            {
              //  SoundManager.Instance.StopOnGivenAudioSource(_audioSourceMove);
                OnStopMoving?.Invoke();

                if (!_animator.GetNextAnimatorStateInfo(0).IsName("Idle"))
                {
                    _animator.CrossFade("Idle", 0.1f);

                }
                _animator.SetBool("IsWalking", false);
                _animator.SetBool("IsRunning", false);
            }
            _inMove = false;
            
            //  _animator.Play(PlayerAnimationConst.IDLE, 1); //Set legs layer to idle state

            //_animator.SetFloat("InputAngle", angle);
            // _animator.SetFloat("Speed", 0f);


        }

    }
    private void PickUp()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _pickUpRadius);
        float minDist = Mathf.Infinity;
        foreach (var hitCollider in hitColliders)
        {

            if (hitCollider.TryGetComponent<PickupObject>(out PickupObject pickable))
            {

                float dist = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (dist < minDist)
                {

                    closestPickUp = pickable;
                    minDist = dist;
                }
            }

        }

        if (closestPickUp != null)
        {
            


            if (Input.GetKeyDown(KeyCode.E) && _inMove == false)
            {

                //_animator.Play(PlayerAnimationConst.PICK_UP);
                //dSoundManager.Instance.PlayOnGivenAudioSource(_audioSourceMove, _collectItem);
                closestPickUp.PickUp(this);
            }
        }

        closestPickUp = null;
    }

    public void TakeHit(float value)
    {
        playerLight.ChangeCurrentIntensity(-value);
    }
    public void IncreaseLightIntensity(float value)
    {
        playerLight.ChangeCurrentIntensity(value);
    }
}
