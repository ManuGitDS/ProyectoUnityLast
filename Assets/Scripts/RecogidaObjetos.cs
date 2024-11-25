using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogidaObjetos : MonoBehaviour
{
 
    void Update()
    {

        
        

    }
////////////////Recogida de monedas////////////////////////
    private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Moneda"))
            {
                Debug.Log("Contacto");
                Destroy(gameObject);
            }

        }
}
