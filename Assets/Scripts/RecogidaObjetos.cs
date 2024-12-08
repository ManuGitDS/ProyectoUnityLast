using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Importa esto para cambiar de escena

public class RecogidaObjetos : MonoBehaviour
{
    private int monedasRecogidas = 0; // Contador de monedas

    void Update()
    {
        // Puedes dejar esto vacío si no necesitas usarlo por ahora.
    }

    // Recogida de monedas
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Moneda"))
        {
            Debug.Log("Contacto con moneda");
            Destroy(other.gameObject); // Destruye la moneda
            monedasRecogidas++; // Incrementa el contador
            Debug.Log("Monedas recogidas: " + monedasRecogidas);

            // Comprueba si se alcanzó el objetivo
            if (monedasRecogidas >= 15)
            {
                CambiarEscena();
            }
        }
    }

    // Método para cambiar de escena
    private void CambiarEscena()
    {
        Debug.Log("Has recogido todas las monedas. Cambiando de escena...");
        SceneManager.LoadScene(3); // Sustituye "NombreDeTuEscena" por el nombre real de tu escena
    }
}
