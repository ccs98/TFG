using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvents", menuName = "Quiz/new GameEvents")]
public class GameEvents : ScriptableObject
{
    public delegate void ActualizarPreguntasCallback(Question pregunta);
    public ActualizarPreguntasCallback actualizarPreguntaIU;

    public delegate void ActualizarPreguntasResuestasCallback(AnswersData respuestaEscogida);
    public ActualizarPreguntasResuestasCallback actualizarPreguntasResuesta;

    public delegate void MostrarPantallaCallback(UIManager.tipoPantallaResolucion type, int score);
    public MostrarPantallaCallback mostrarPantalla;

    public delegate void ActualizarPuntuacionCallback(int puntuacion);
    public ActualizarPuntuacionCallback actualizarPuntuacion;
    
    [HideInInspector]
    public int puntuacionActual;
    [HideInInspector]
    public int StartupHighScore;
}
