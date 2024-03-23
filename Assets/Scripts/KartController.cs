using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartController : MonoBehaviour
{
    // Start is called before the first frame update
    public float velMaxFrente = 100f;
    public float velMaxRetro = 20f;
    public float aceleracion = 5f;
    public float giroSpeed = 100f;
    public float frenado = 5f; // Reducir la fuerza de frenado durante el derrape
    public float derrapeForce = 50f;

    private float velocidadActual = 0;
    private Rigidbody rb;
    private bool derrapando = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float inputVertical = Input.GetAxis("Vertical");

        float velMaxActual = inputVertical >= 0 ? velMaxFrente : velMaxRetro;

        if (inputVertical > 0)
        {
            velocidadActual += aceleracion * Time.deltaTime;
        }
        else if (inputVertical < 0)
        {
            velocidadActual -= frenado * Time.deltaTime;
        }
        else
        {
            if (velocidadActual > 0)
            {
                velocidadActual -= frenado * Time.deltaTime;
            }
            else if (velocidadActual < 0)
            {
                velocidadActual += frenado * Time.deltaTime;
            }
        }

        velocidadActual = Mathf.Clamp(velocidadActual, -velMaxFrente, velMaxFrente);

        float giro = Input.GetAxis("Horizontal");

        if (rb.velocity.magnitude > 3f && derrapando)
        {
            Vector3 rotation = transform.rotation.eulerAngles + new Vector3(0, giro * giroSpeed * Time.deltaTime, 0);
            rb.MoveRotation(Quaternion.Euler(rotation));
        }
        else
        {
            Quaternion deltaRotation = Quaternion.Euler(0, giro * giroSpeed * Time.deltaTime, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        rb.velocity = transform.forward * velocidadActual;

        if (Input.GetKeyDown(KeyCode.X) && Mathf.Abs(giro) > 0.1f)
        {
            derrapando = true;
        }
        else if (Input.GetKeyUp(KeyCode.X) || Mathf.Abs(giro) < 0.1f)
        {
            derrapando = false;
        }

        if (inputVertical != 0 || derrapando)
        {
            rb.angularDrag = 0.5f; // Reducir la fricción angular para permitir que derrape mientras acelera o durante el derrape
        }
        else
        {
            rb.angularDrag = 5f; // Restaurar la fricción angular normal cuando no se está acelerando y no se está derrapando
        }

        // Aplicar fuerza de derrape cuando se está derrapando
        if (derrapando)
        {
            rb.AddForce(transform.right * derrapeForce, ForceMode.Force);
        }
    }


    // Update is called once per frame
    void Update()
{
        
}

}

