using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour
{
    public float moveDuration = 0.5f;
    public float moveDistance = 1f;
    public float maxDistanceFromStart = 3f; // Maximum distance from the starting point

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        // Call Move() every 1-3 seconds
        InvokeRepeating("Move", Random.Range(1f, 3f), Random.Range(1f, 3f));
    }

    void Move()
    {// Calculate a random destination within moveDistance from the starting position
        Vector3 destination = startPosition + Random.insideUnitSphere * moveDistance;

        // Calculate the distance from the starting position to the destination
        float distanceToDestination = Vector3.Distance(startPosition, destination);

        // If the distance is greater than the maximum distance from the starting point, clamp the destination position
        if (distanceToDestination > maxDistanceFromStart)
        {
            destination = startPosition + (destination - startPosition).normalized * maxDistanceFromStart;
        }

        // Move to the destination using DoTween
        transform.DOMove(destination, moveDuration);
    }
}
