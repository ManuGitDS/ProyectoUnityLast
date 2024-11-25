using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Personaje : MonoBehaviour
{
    public float velocidadNormal = 5.0f;
    public float velocidadMovimiento;
    public float turbo = 10.0f;
    public float velocidadRotacion = 200.0f;
    public Vector3 direccionSalto;

 

    private Animator anim;
    public float x, y;
    public bool estoySaltando = false;

    public Rigidbody rb;
    public float fuerzaSalto = 5f;
    public bool puedoSaltar;
    public bool estaEnElSuelo;


    //////////////movimiento con mouse///////////////
    public bool rotacionConMouse;
    public float sensibilidadMouse = 1000.0f;

    //variable para la cámara
    public Transform camara;
    private float rotacionCamaraX = 0f; // Para manejar la rotación hacia arriba y hacia abajo






    void Start()
    {

        estaEnElSuelo = true;
        puedoSaltar = false;
        anim = GetComponent<Animator>();

        puedoSaltar = false;
        anim = GetComponent<Animator>();


        // Verifica que la cámara haya sido asignada
        if (camara == null)
        {
            camara = Camera.main.transform; // Asigna automáticamente la Main Camera
        }


    }

    //Estandariza los frames en todos los PC
    private void FixedUpdate()
    {
        if (puedoSaltar)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                velocidadMovimiento = turbo;
            }
            else
            {
                velocidadMovimiento = velocidadNormal;

            }


        }
        //MOVERSE LATERALMENTE CON 'X' Y HACIA ADELANTE/ATRÁS CON 'Y'

        if (!estaEnElSuelo)
        {
            // Moverse en la dirección guardada al saltar
            Vector3 movimientoLateral = direccionSalto * velocidadMovimiento * Time.deltaTime;
            transform.position += movimientoLateral;
        }
        else
        {
            // Solo permite el movimiento normal cuando está en el suelo
            Vector3 movimientoLateral = transform.right * x * velocidadMovimiento * Time.deltaTime;
            Vector3 movimientoAdelante = transform.forward * y * velocidadMovimiento * Time.deltaTime;
            transform.position += movimientoLateral + movimientoAdelante;
        }


        //ROTACIÓN DEL PERSONAJE CON 'Q' Y 'E'

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, -velocidadRotacion * Time.deltaTime, 0); // Rotación a la izquierda
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, velocidadRotacion * Time.deltaTime, 0); // Rotación a la derecha
        }

        if (rotacionConMouse)
        {
            // Rotación horizontal con el mouse (afecta al personaje)
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(0, mouseX * sensibilidadMouse * Time.deltaTime, 0);

            // Rotación vertical con el mouse (afecta solo a la cámara)
            float mouseY = Input.GetAxis("Mouse Y");
            rotacionCamaraX -= mouseY * sensibilidadMouse * Time.deltaTime;
            rotacionCamaraX = Mathf.Clamp(rotacionCamaraX, -90f, 90f); // Limita el ángulo vertical de la cámara

            camara.localRotation = Quaternion.Euler(rotacionCamaraX, 0, 0); // Aplica la rotación vertical a la cámara
        }

        /////////////////////////////
        //////////////////////////
    }
    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);

        // Activar rotación con mouse mientras el botón derecho está presionado
        if (Input.GetMouseButtonDown(1)) // Click derecho presionado
        {
            rotacionConMouse = true;
        }
        if (Input.GetMouseButtonUp(1)) // Soltar click derecho
        {
            rotacionConMouse = false;
        }

        if (puedoSaltar)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("Salto", true);
                // Al saltar, guardamos la dirección en la que nos movíamos
                direccionSalto = transform.forward * y + transform.right * x;
                rb.AddForce(new Vector3(x, fuerzaSalto, 0), ForceMode.Impulse);
                estoySaltando = true;
               


            }

            anim.SetBool("tocaSuelo", true);
         
        }
        else
        {
            estoyCayendo();

        }
    }

    public void estoyCayendo()
    {
        anim.SetBool("tocaSuelo", false);
        anim.SetBool("Salto", false);
    }



    //muerte del personaje con suelo

    private void OnCollisionEnter(Collision collision)
    {
      
       // Debug.Log("estaEnElSuelo = true");/////////////////////////////////////
        
        // Verifica si el objeto con el que colisionaste es el suelo
        if (collision.gameObject.CompareTag("Suelo"))
        {
            Muerte(); // Llama al método Muerte
        }

        if (collision.gameObject.CompareTag("Plataforma"))
        {
            estaEnElSuelo = true;
            estoySaltando = false;
        }

    }
    private void OnCollisionExit(Collision collision)
    {
      
        //Debug.Log("estaEnElSuelo = false");/////////////////////////////////////
        // Verifica si deja de tocar el suelo
        if (collision.gameObject.CompareTag("Plataforma") & estoySaltando)
        {
            estaEnElSuelo = false; // Está en el aire
          
        }
    }
    private void Muerte()
    {
        
        Debug.Log("¡Has muerto!");

        // Ejemplo: Reiniciar la escena
        SceneManager.LoadScene(1); //SceneManager.GetActiveScene().name carga el inicio de la escena actual
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Moneda"))
        {
            Debug.Log("Contacto");
            Destroy(other.gameObject);
        }

    }

}
