using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class CarMovimiento : MonoBehaviour
{
    public enum Axel
    {
        Front,
        rear,
    }
    [Serializable]
    public struct wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;

    }

    public float MaxAcceleration = 30.0f;
    public float breakAcceleration = 50.0f;


    public float turnSensitive = 1.0f;
    public float maxSteerAnlge = 30.0f;

    public Vector3 _centerOfMass;

    public List<wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRig;

    void Start()
    {
        carRig = GetComponent<Rigidbody>();
        carRig.centerOfMass = _centerOfMass;
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
            wheel.wheelCollider.motorTorque = moveInput * 600 * MaxAcceleration * Time.deltaTime;

        }



    }
    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAnlge = steerInput * turnSensitive * maxSteerAnlge;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAnlge, 0.6f);
            }
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 500 * breakAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
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
