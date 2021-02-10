using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum tipoRespuesta {Multi, Single}

[Serializable()]
public class Respuesta
{
    public string info = string.Empty;
    public bool esCorrecta = false;
    public Respuesta(){}
}

[Serializable()]
public class Question
{
    
    public string info = null;
    public Respuesta[] respuestas = new Respuesta[0];
    public tipoRespuesta tipo = tipoRespuesta.Single;
    public Int32 puntuacion = 0;
    public Question(){}
    public List<int> GetRespuestaCorrecta()
    {
        List<int> RespuestasCorrectas = new List<int>();
        for(int i = 0;i < respuestas.Length; i++)
        {
            if(respuestas[i].esCorrecta){
                RespuestasCorrectas.Add(i);
            }
        }
        return RespuestasCorrectas;
    }
}
