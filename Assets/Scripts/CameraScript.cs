using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float threshold = 2f;
    Camera camera;
    Vector3 mousePos = Vector3.zero;

    void Start()
    {
        camera = Camera.main;
        
    }

    
    void Update()
    {
       
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        // Declare a variable to store the distance to the plane
        float distance;

        // If the ray intersects the plane, set the distance to the intersection point
        if (plane.Raycast(ray, out distance))
        {
            // Get the intersection point
            mousePos = ray.GetPoint(distance);

        }
            Vector3 targetPos = (player.position+mousePos)/2f;

        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x, threshold + player.position.x);
        targetPos.y = 0f;
        targetPos.z = Mathf.Clamp(targetPos.z, -threshold + player.position.z, threshold + player.position.z);

        this.transform.position = targetPos;
       
    }
}
