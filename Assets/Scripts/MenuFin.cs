using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFin : MonoBehaviour
{
   public void VolverEmpezar()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Cambi a escena 0!");
    }
    public void inicio()
    {
       SceneManager.LoadScene(0);
        Debug.Log("Cambi a escena 0!");
    }
}
