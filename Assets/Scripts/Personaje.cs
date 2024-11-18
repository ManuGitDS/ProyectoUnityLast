using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Personaje : MonoBehaviour
{

    public float velocidadMovimiento = 5.0f;
    public float velocidadRotacion = 200.0f;

    private Animator anim;
    public float x,y;


    public Rigidbody rb;
    public float fuerzaSalto = 5f;
    public bool puedoSaltar;


    //////////////movimiento con mouse///////////////
    public bool rotacionConMouse;
    public float sensibilidadMouse = 1000.0f;

    //variable para la c�mara
    public Transform camara;
    private float rotacionCamaraX = 0f; // Para manejar la rotaci�n hacia arriba y hacia abajo






    void Start()
    {
        
        puedoSaltar = false;
        anim = GetComponent<Animator>();

        puedoSaltar = false;
        anim = GetComponent<Animator>();

        // Verifica que la c�mara haya sido asignada
        if (camara == null)
        {
            camara = Camera.main.transform; // Asigna autom�ticamente la Main Camera
        }


    }

    //Estandariza los frames en todos los PC
    private void FixedUpdate()
    {
        //MOVERSE LATERALMENTE CON 'X' Y HACIA ADELANTE/ATR�S CON 'Y'
        Vector3 movimientoLateral = transform.right * x * velocidadMovimiento * Time.deltaTime;
        Vector3 movimientoAdelante = transform.forward * y * velocidadMovimiento * Time.deltaTime;
        transform.position += movimientoLateral + movimientoAdelante;


        //ROTACI�N DEL PERSONAJE CON 'Q' Y 'E'

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, -velocidadRotacion * Time.deltaTime, 0); // Rotaci�n a la izquierda
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, velocidadRotacion * Time.deltaTime, 0); // Rotaci�n a la derecha
        }

        if (rotacionConMouse)
        {
            // Rotaci�n horizontal con el mouse (afecta al personaje)
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(0, mouseX * sensibilidadMouse * Time.deltaTime, 0);

            // Rotaci�n vertical con el mouse (afecta solo a la c�mara)
            float mouseY = Input.GetAxis("Mouse Y");
            rotacionCamaraX -= mouseY * sensibilidadMouse * Time.deltaTime;
            rotacionCamaraX = Mathf.Clamp(rotacionCamaraX, -90f, 90f); // Limita el �ngulo vertical de la c�mara

            camara.localRotation = Quaternion.Euler(rotacionCamaraX, 0, 0); // Aplica la rotaci�n vertical a la c�mara
        }

        /////////////////////////////
        //////////////////////////
    }
    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        anim.SetFloat("VelX",x);
        anim.SetFloat("VelY", y);

        // Activar rotaci�n con mouse mientras el bot�n derecho est� presionado
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
                rb.AddForce(new Vector3(x, fuerzaSalto, 0), ForceMode.Impulse);
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
        anim.SetBool("Salto",false);
    }



    //muerte del personaje con suelo
    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto con el que colisionaste es el suelo
        if (collision.gameObject.CompareTag("Suelo"))
        {
            Muerte(); // Llama al m�todo Muerte
        }
    }
    private void Muerte()
    {
        // Aqu� puedes definir lo que ocurre cuando el personaje muere
        Debug.Log("�Has muerto!");

        // Ejemplo: Reiniciar la escena
        SceneManager.LoadScene(1); //SceneManager.GetActiveScene().name carga el inicio de la escena actual
    }
}
