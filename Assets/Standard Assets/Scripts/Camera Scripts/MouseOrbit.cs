using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOrbit : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    public float zSpeed = 100.0f;

    public int yMinLimit = -20;
    public int yMaxLimit = 80;

    private float x = 0;
    private float y = 0;

    // Start is called before the first frame update
    void Start()
    {
        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void LatUpdate()
    {
        if (target)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);
            var rotation = Quaternion.Euler(y, x, 0);
            var position = rotation * new Vector3(Input.GetAxis("Horizontal"),
                                                  Input.GetAxis("Mouse ScrollWheel") * zSpeed,
                                                  Input.GetAxis("Vertical"));

            transform.rotation = rotation;
            transform.position = transform.position + position;
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
