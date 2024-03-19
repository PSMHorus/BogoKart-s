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
    public float frenado = 10f;
    public float derrapeForce = 50f;

    private float velocidadActual = 0;
    private Rigidbody rb;
    void Start()

    {
        rb =GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float inputVertical = Input.GetAxis("Vertical");

        float velMaxActual = inputVertical >= 0 ? velMaxFrente : velMaxRetro;

        if(inputVertical > 0)
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

        if(rb.velocity.magnitude > 3f)
        {
            transform.Rotate(0, giro * giroSpeed * Time.deltaTime, 0);
        }
        

        rb.velocity =transform.forward *velocidadActual;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.right * derrapeForce, ForceMode.Impulse);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
