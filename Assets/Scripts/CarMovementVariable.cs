using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CarMovimiento;

public class CarMovementVariable : MonoBehaviour
{
    public float moveSpeed = 50;
    public float Drag = 0.98f;
    public float MaxSpeed = 15;
    public float Traction = 1;
    public float SteerAngle = 20;
    public Vector3 MoveForce;
    public List<wheel> wheels;
    public float turnSensitive = 1.0f;
    public float maxSteerAnlge = 30.0f;


    void Update()
    {
        MoveForce += transform.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += MoveForce * Time.deltaTime;


        float steerInput = Input.GetAxis("Space");   
        transform.Rotate(Vector3.up*steerInput*MoveForce.magnitude*SteerAngle*Time.deltaTime);

       


        MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);



        Debug.DrawRay(transform.position, MoveForce.normalized * 3);
        Debug.DrawRay(transform.position,transform.forward * 3,Color.blue);
        MoveForce = Vector3.Lerp(MoveForce.normalized,transform.forward,Traction*Time.deltaTime) *MoveForce.magnitude;

       

    }
    
   






}
