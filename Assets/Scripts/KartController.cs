using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartController : MonoBehaviour
{
    public float velocidadMaxima = 10f;
    public float aceleracion = 2f;
    public float frenado = 1f;
    public float velocidadRetroceso = 5f;
    public float giroSpeed = 100f;
    public float fuerzaDerrapeBase = 150f;
    public float fuerzaGiroDerrape = 200f;
    public float maxAngleForDrift = 30f; // Máximo ángulo para el derrape
    public ParticleSystem humoParticulasPrefact;

    private float velocidadActual = 0f;
    private bool derrapando = false;
    private ParticleSystem humoParticulas;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        humoParticulas = Instantiate(humoParticulasPrefact, transform.position, Quaternion.identity);
        humoParticulas.Stop();
    }

    void Update()
    {
        // Manejo de aceleración y frenado
        float inputVertical = Input.GetAxis("Vertical");
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
        velocidadActual = Mathf.Clamp(velocidadActual, -velocidadRetroceso, velocidadMaxima);

        // Manejo de la dirección
        float inputHorizontal = Input.GetAxis("Horizontal");
        if (rb.velocity.magnitude > 0.1f)
        {
            transform.Rotate(Vector3.up * inputHorizontal * giroSpeed * Time.deltaTime);
        }

        // Manejo del derrape
        if (Input.GetKeyDown(KeyCode.Space))
        {
            derrapando = true;
            humoParticulas.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            derrapando = false;
            humoParticulas.Stop();
        }

        if (derrapando && rb.velocity.magnitude > 1f)
        {
            float angle = Vector3.Angle(rb.velocity, transform.forward);
            if (angle > maxAngleForDrift)
            {
                float angleFactor = Mathf.Clamp01((angle - maxAngleForDrift) / (180f - maxAngleForDrift));
                float driftForce = fuerzaDerrapeBase * angleFactor;
                rb.AddForce(transform.right * driftForce * inputHorizontal, ForceMode.Force);
            }
        }

        // Actualizar la posición de las partículas de humo
        humoParticulas.transform.position = transform.position;
    }

    void FixedUpdate()
    {
        // Aplicar la velocidad al Rigidbody
        rb.velocity = transform.forward * velocidadActual;
    }
}





