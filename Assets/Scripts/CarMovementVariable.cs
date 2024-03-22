using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovementVariable : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear,
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public float maxAcceleration = 30.0f;
    public float breakAcceleration = 50.0f;
    public float turnSensitive = 1.0f;
    public float maxSteerAngle = 30.0f;
    public Vector3 centerOfMass;
    public List<Wheel> wheels;

    private float moveInput;
    private float steerInput;
    private Rigidbody carRig;

    void Start()
    {
        carRig = GetComponent<Rigidbody>();
        carRig.centerOfMass = centerOfMass;
    }

    private void Update()
    {
        GetInput();
        AnimationWheels();
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    void GetInput()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                float steerAngle = steerInput * turnSensitive * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
            }
        }
    }

    void Brake()
    {
        float brakeTorque = Input.GetKey(KeyCode.Space) ? 800 * breakAcceleration * Time.deltaTime : 0;

        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.brakeTorque = brakeTorque;
        }
    }

    void AnimationWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }
}
