using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PermanentUI : MonoBehaviour
{
    public int puntos = 0;
    public int vida = 5;
    public TextMeshProUGUI puntosTexto;
    public Text cantidadVida;
    public Text info;
    public int cont = 0;
    public bool daño = false;

    public static PermanentUI perm;
    private PermanentUI current;
    private void Awake()
    {
        current = this;
        if(perm == null)
        {
            perm = this;   
            DontDestroyOnLoad(gameObject);
        }
        else if(perm != current){cont++; Destroy(gameObject);}
        
    }
    /*
    public void Reset()
    {
        puntos = 0;
        puntosTexto.text = puntos.ToString();
    }*/
    public void ResetearVida()
    {
        vida = 5;
        cantidadVida.text = vida.ToString();
    }
    public void Informacion()
    {
        perm.info.gameObject.SetActive(true);
    }
}
