using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartController : MonoBehaviour
{
    public WheelCollider[] driveWheels;
    public WheelCollider[] steerWheels;
    public float maxMotorTorque = 1000f;
    public float maxBrakeTorque = 1000f;
    public float maxSteeringAngle = 45f;
    public float driftTorque = 2000f;
    public KeyCode driftKey = KeyCode.X;

    private bool isDrifting = false;

    void Update()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float brake = maxBrakeTorque * Mathf.Clamp01(-Input.GetAxis("Vertical"));
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (WheelCollider wheel in driveWheels)
        {
            if (!isDrifting)
            {
                wheel.motorTorque = motor;
            }
            else
            {
                wheel.motorTorque = 0;
            }
               

            wheel.brakeTorque = brake;

            if (Input.GetKeyDown(driftKey))
            {
                isDrifting = true;


            }
                
            else if (Input.GetKeyUp(driftKey))
            {
                isDrifting = false;
            }
              
        }

        foreach (WheelCollider wheel in steerWheels)
        {
            wheel.steerAngle = steering;
        }
    }

    void FixedUpdate()
    {
        if (isDrifting)
        {
            foreach (WheelCollider wheel in driveWheels)
            {
                wheel.motorTorque = driftTorque * Input.GetAxis("Vertical");
            }
        }
    }
}
