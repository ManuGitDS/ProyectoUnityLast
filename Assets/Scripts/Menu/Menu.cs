using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void ComenzarNivel(string NombreNivel)
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Salir()
    {
        Application.Quit();
        Debug.Log("Se cerro el juego");
    }
}
