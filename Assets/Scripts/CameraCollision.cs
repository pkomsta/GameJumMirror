using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public float wallDistance = 1f;
    public float cameraSpeed = 5f;
    public float rotationSpeed = 10f;

    private bool isColliding = false;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Update()
    {
        if (isColliding)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Object")
        {
            isColliding = true;

            Vector3 direction = (transform.position - collision.transform.position).normalized;
            targetPosition = collision.transform.position + direction * wallDistance;
            targetRotation = Quaternion.LookRotation(-direction, Vector3.up);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Object")
        {
            isColliding = false;
        }
    }
}
