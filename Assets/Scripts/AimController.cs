using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Get Input from the mouse of speed and direction
/// </summary>
public class AimController : MonoBehaviour
{
    private Vector3 initialClickPos;
    public Transform startingPoint;
    public float speedMultiplier = 1;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DataStore.drawPoints = true;
            initialClickPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            CalculateDirection();
            CalculateSpeed();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DataStore.drawPoints = false;
        }
    }

    private void CalculateDirection()
    {
        Vector3 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        camPos.z = 0;
        DataStore.initialVelocityDirn = (camPos - startingPoint.position).normalized;



        Debug.DrawLine(startingPoint.position, camPos);
    }

    private void CalculateSpeed()
    {
        Vector3 currentPos = Input.mousePosition;
        float magnitude = Vector3.Distance(initialClickPos, currentPos);
        if (magnitude < 1)
        {
            magnitude = 1;
        }
        DataStore.speed = magnitude / speedMultiplier;
    }
}
