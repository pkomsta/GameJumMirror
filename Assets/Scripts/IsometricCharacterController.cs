using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class IsometricCharacterController : MonoBehaviour
{
    [SerializeField] float Speed = 8f;
    [SerializeField] float backwardsMovmentPenalty = 0.7f;
    [SerializeField] float Hp = 100;
    [SerializeField] private Transform _kindlingHolder;
    [SerializeField] private AudioClip _walkSound;
    [SerializeField] private AudioClip _collectItem;
    [SerializeField] private AudioClip _dashSound;
    [SerializeField] private AudioClip[] _deathSound;
    [SerializeField] private AudioClip[] _takeDamageSound;
    [SerializeField] private float _pickUpRadius = 5f;

  
    public UnityEvent OnStartMoving;
    public UnityEvent OnStopMoving;
    public UnityEvent OnStartDashing;
    public UnityEvent OnEndDashing;
    public UnityEvent OnUseItem;
    public UnityEvent EverySecond;


    public AudioSource _audioSourceMove;
    public AudioSource _audioSourceAction;

    private Animator _animator;


    private bool _inMove;
    private bool _isDead;

    float currentMoveSpeed = 0f;
    private void Start()
    {
        InitializeEvents();

        _animator = this.GetComponent<Animator>();

        InvokeRepeating(nameof(RunEverySecondEvent), 0, 1f);


    }
    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
           // SoundManager.Instance.PlayOnGivenAudioSource(_audioSourceMove, _deathSound);
            _isDead = true;
           // _animator.Play(PlayerAnimationConst.DEATH);

            return;
        }


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        MoveWASD();
        Look(ray);



    }
    private void RunEverySecondEvent()
    {

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

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10f * Time.deltaTime);
            }
        
    }
    void MoveWASD()
    {
        var inputMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        float angle = Vector3.SignedAngle(transform.forward, inputMovement, Vector3.up);

        if (inputMovement != Vector3.zero )
        {
            if (!_inMove) OnStartMoving?.Invoke();

            inputMovement = MathF.Abs(angle) > 100 ? inputMovement.normalized * Speed * backwardsMovmentPenalty * Time.deltaTime : inputMovement.normalized * Speed * Time.deltaTime;
            currentMoveSpeed = MathF.Abs(angle) > 100 ? Speed * backwardsMovmentPenalty : Speed;

            Quaternion rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            Vector3 rotatedMovement = rotation * inputMovement;

           // SoundManager.Instance.PlayOnGivenAudioSource(_audioSourceMove, _walkSound);
            _inMove = true;

            transform.Translate(inputMovement, Space.World);

           // _animator.SetFloat("InputAngle", angle);
            //_animator.SetFloat("MoveSpeed", currentMoveSpeed / Speed);
           


        }
        else
        {
            if (_inMove)
            {
              //  SoundManager.Instance.StopOnGivenAudioSource(_audioSourceMove);
                OnStopMoving?.Invoke();
            }
            _inMove = false;

          //  _animator.Play(PlayerAnimationConst.IDLE, 1); //Set legs layer to idle state

            //_animator.SetFloat("InputAngle", angle);
           // _animator.SetFloat("Speed", 0f);

            
        }
    }
}
