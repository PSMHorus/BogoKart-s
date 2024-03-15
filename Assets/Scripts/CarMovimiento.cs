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
        rear
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

    public List<wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRig;

    void Start()
    {
        carRig = GetComponent<Rigidbody>(); 
    }

    private void Update()
    {
        GetInput();
    }
    void LateUpdate()
    {
        Move();
        Steer();
    }

    void GetInput()
    {
        moveInput=Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }
    void Move()
    {
        foreach (wheel wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * 600 * MaxAcceleration * Time.deltaTime;

        }
        


    }
    void Steer()
    {
        foreach(wheel wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAnlge = steerInput * turnSensitive * maxSteerAnlge;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAnlge, 0.6f);
            }



        }
    }


}
