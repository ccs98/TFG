using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Points : MonoBehaviour
{
    [SerializeField] private GameEvents events = null;
    private int hits = 0;
    private Points points;

    private void Awake()
    {
        events.actualizarPuntuacion += CargarEscena;
    }
    void CargarEscena(int puntuacion)
    {
        if(puntuacion >=3)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
    void Ondestroy()
    {
        events.actualizarPuntuacion -= CargarEscena;
    }

}
