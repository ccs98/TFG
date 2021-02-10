using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswersData : MonoBehaviour
{
    [Header("Elementos de la IU")]
    [SerializeField] TextMeshProUGUI objetoTextoInfo;
    [SerializeField] Image boton;
    
    [Header("Textures")]
    [SerializeField] Sprite botonNoPulsado;
    [SerializeField] Sprite botonPulsado;

    [Header("References")]
    [SerializeField] GameEvents eventos;

    private RectTransform rect;
    public RectTransform Rect
    {
        get
        {
            if (rect == null)
            {
                rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
            }
            return rect;
        }
    }

    private int indiceRespuesta = -1;
    public int IndiceRespuesta { get { return indiceRespuesta; } }

    private bool Check = false;

    public void ActualizarDatos(string info, int indice)
    {
        objetoTextoInfo.text = info;
        indiceRespuesta = indice;
    }

    public void Reset()
    {
        Check = false;
        ActualizarIU();
    }
    public void CambiarEstado()
    {
        Check = !Check;
        ActualizarIU();
        if (eventos.actualizarPreguntasResuesta != null)
        {
            eventos.actualizarPreguntasResuesta(this);
        }
    }
    public void ActualizarIU()
    {
        if(boton == null) return;
        boton.sprite = (Check) ? botonPulsado : botonNoPulsado;
    }
}
