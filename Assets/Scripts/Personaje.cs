using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    //variable para la c�mara
    public Transform camara;
    private float rotacionCamaraX = 0f; // Para manejar la rotaci�n hacia arriba y hacia abajo

    //puntuacion
  
    private int puntuacion;
    public TextMeshProUGUI puntuacionText;






    void Start()
    {
        puntuacion = 0;
        estaEnElSuelo = true;
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
        //MOVERSE LATERALMENTE CON 'X' Y HACIA ADELANTE/ATR�S CON 'Y'

        if (!estaEnElSuelo)
        {
            // Moverse en la direcci�n guardada al saltar
            Vector3 movimientoLateral = direccionSalto * velocidadMovimiento * Time.deltaTime;
            transform.position += movimientoLateral;
        }
        else
        {
            // Solo permite el movimiento normal cuando est� en el suelo
            Vector3 movimientoLateral = transform.right * x * velocidadMovimiento * Time.deltaTime;
            Vector3 movimientoAdelante = transform.forward * y * velocidadMovimiento * Time.deltaTime;
            transform.position += movimientoLateral + movimientoAdelante;
        }


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

        anim.SetFloat("VelX", x);
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
                // Al saltar, guardamos la direcci�n en la que nos mov�amos
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
            Muerte(); // Llama al m�todo Muerte
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
            estaEnElSuelo = false; // Est� en el aire
          
        }
    }
    private void Muerte()
    {
        
        Debug.Log("�Has muerto!");

        // Ejemplo: Reiniciar la escena
        SceneManager.LoadScene(2); //SceneManager.GetActiveScene().name carga el inicio de la escena actual
    }

     public int maxMonedas = 15;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Moneda"))
        {
            Debug.Log("Contacto");
            other.gameObject.SetActive(false); // Desactiva la moneda
            puntuacion++; // Incrementa la puntuación
            puntuacionText.text = puntuacion.ToString(); // Actualiza el texto de puntuación
            Destroy(other.gameObject); // Destruye la moneda para liberar memoria

            // Comprueba si has alcanzado el número máximo de monedas
            if (puntuacion >= maxMonedas)
            {
                hasGanado(); // Llama al método para cambiar de escena
            }
        }
    }
    private void hasGanado()
    {
        Debug.Log("Has recogido todas las monedas. Cambiando de escena...");
        SceneManager.LoadScene(3); // Sustituye "NombreDeTuEscena" por el nombre de tu escena
    }

}
