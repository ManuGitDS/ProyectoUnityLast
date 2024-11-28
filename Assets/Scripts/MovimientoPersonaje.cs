//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NewBehaviourScript : MonoBehaviour
//{
    //public float runSpeed = 7;
    //public float RotationSpeed = 250;
    //public Animator animator;
    //private float x, y;
    
    //public Rigidbody rb;
    //public float jumpHeight = 3;
    //public Transform groundCheck;
    //public float groundDistance = 0.1f;
    //public LayerMask groundMask;

    //bool isGrounded;

    //void Update()
    //{
        //// Movimiento y rotación del personaje
        //x = Input.GetAxis("Horizontal");
        //y = Input.GetAxis("Vertical");

        //transform.Rotate(0, x * Time.deltaTime * RotationSpeed, 0);
        //transform.Translate(0, 0, y * Time.deltaTime * runSpeed);

        //// Actualización del animator
        //animator.SetFloat("VelX", x);
        //animator.SetFloat("VelY", y);

        //// Verificación de si el personaje está en el suelo
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //// Salto
        //if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        //{
            //Jump(); //// Llamar a Jump directamente sin Invoke
        //}
    //}

    //void Jump()
    //{
        //// Asegúrate de resetear la velocidad en Y antes de aplicar la fuerza de salto
        //rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //// Añadir fuerza de salto
        //rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);

        //// Animación de salto
        //animator.Play("Jump");
    //}
//}
