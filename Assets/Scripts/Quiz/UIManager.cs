using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;


[Serializable()]
public struct UIManagerParametros
{
    [Header("Opciones(Respuestas)")]
    [SerializeField] float margins;
    public float Margins { get {return margins; } }

    [Header("Opciones(Pantalla de resultados)")]
    [SerializeField] Color colorCorrecto;
    public Color ColorCorrecto { get {return colorCorrecto; } }
    [SerializeField] Color colorIncorrecto;
    public Color ColorIncorrecto { get {return colorIncorrecto; } }
    [SerializeField] Color colorFinal;
    public Color ColorFinal { get {return colorFinal; } }
}
[Serializable()]
public struct UIElementos
{
    [SerializeField] RectTransform areaRespuestasContenido;
    public RectTransform  AreaRespuestasContenido {get {return areaRespuestasContenido;}}
    [SerializeField] TextMeshProUGUI infoPreguntaObjetoTexto;
    public TextMeshProUGUI  InfoPreguntaObjetoTexto {get {return infoPreguntaObjetoTexto;}}
    [SerializeField] TextMeshProUGUI textoPuntuacion;
    public TextMeshProUGUI  TextoPuntuacion {get {return textoPuntuacion;}}
    [Space]
    [SerializeField] Animator pantallaRespuestasAnimator;
    public Animator  PantallaRespuestasAnimator {get {return pantallaRespuestasAnimator;}}
    [SerializeField] Image resolutionBackground;
    public Image  ResolutionBackground {get {return resolutionBackground;}}
    [SerializeField] TextMeshProUGUI textoEstadoResolucion;
    public TextMeshProUGUI  TextoEstadoResolucion {get {return textoEstadoResolucion;}}
     [SerializeField] TextMeshProUGUI textoPuntuacionResolucion;
    public TextMeshProUGUI  TextoPuntuacionResolucion {get {return textoPuntuacionResolucion;}}
    [Space]
    [SerializeField] CanvasGroup main;
    public CanvasGroup  Main {get {return main;}}
    [SerializeField] RectTransform elementosFinales;
    public RectTransform  ElementosFinales {get {return elementosFinales;}}
    [SerializeField] CanvasGroup rbg;
    public CanvasGroup  rBg {get {return rbg;}}
}
public class UIManager : MonoBehaviour
{
    public enum tipoPantallaResolucion { Correcto, Incorrecto, Final }

    [Header("Referencias")]
    [SerializeField] GameEvents eventos;

    [Header("IU Elementos (Prefabs)")]
    [SerializeField] AnswersData prefabRespuesta;
    [SerializeField] UIElementos elementos;

    [Space]
    [SerializeField] UIManagerParametros parametros;

    List<AnswersData> respuestaActual = new List<AnswersData>();

    private int resStateParaHash = 0;

    private IEnumerator IE_DisplayTimeResolution;

    void OnEnable()
    {
        eventos.actualizarPreguntaIU += ActualizarPreguntaIU;
        eventos.mostrarPantalla += MostrarResolucion;
        eventos.actualizarPuntuacion += ActualizarPuntuacionIU;
    }
    void OnDisable()
    {
        eventos.actualizarPreguntaIU -= ActualizarPreguntaIU;
        eventos.mostrarPantalla -= MostrarResolucion;
        eventos.actualizarPuntuacion -= ActualizarPuntuacionIU;
    }
    void Start()
    {
        ActualizarPuntuacionIU(0);
        resStateParaHash = Animator.StringToHash("ScreenState");
    }
    void ActualizarPreguntaIU(Question pregunta)
    {
        elementos.InfoPreguntaObjetoTexto.text = pregunta.info;
        CrearRespuestas(pregunta);
    }
    void MostrarResolucion(tipoPantallaResolucion tipo, int puntuacion)
    {
        ActualizaResIU(tipo, puntuacion);
        elementos.PantallaRespuestasAnimator.SetInteger(resStateParaHash,2);
        elementos.Main.blocksRaycasts = false;
        if(tipo != tipoPantallaResolucion.Final)
        {
            if(IE_DisplayTimeResolution != null)
            {
                StopCoroutine(IE_DisplayTimeResolution);
            }
            IE_DisplayTimeResolution = MostrarResolucionTiempo();
            StartCoroutine(IE_DisplayTimeResolution);
        }
    }

    IEnumerator MostrarResolucionTiempo()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        elementos.PantallaRespuestasAnimator.SetInteger(resStateParaHash, 0);
        elementos.Main.blocksRaycasts = true;
        elementos.rBg.blocksRaycasts = false;
    }

    void ActualizaResIU(tipoPantallaResolucion tipo, int puntuacion)
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        switch (tipo)
        {
            case tipoPantallaResolucion.Correcto:
                elementos.ResolutionBackground.color = parametros.ColorCorrecto;
                elementos.TextoEstadoResolucion.text = "CORRECTO";
                elementos.TextoPuntuacionResolucion.text = ":"+ puntuacion;
            break;
            case tipoPantallaResolucion.Incorrecto:
                elementos.ResolutionBackground.color = parametros.ColorIncorrecto;
                elementos.TextoEstadoResolucion.text = "INCORRECTO";
                elementos.TextoPuntuacionResolucion.text = ":"+ 0;
            break;
            case tipoPantallaResolucion.Final:
                elementos.ResolutionBackground.color = parametros.ColorFinal;
                elementos.TextoEstadoResolucion.text = "FINAL";
                StartCoroutine(CalcularPuntuacion());
                elementos.ElementosFinales.gameObject.SetActive(true);
                elementos.TextoPuntuacion.gameObject.SetActive(true); 
                elementos.TextoPuntuacion.text = ((highscore > eventos.StartupHighScore) ? "<color=yellow>new</color>" : string.Empty);
            break;
        }
    }

    IEnumerator CalcularPuntuacion()
    {
        if(eventos.puntuacionActual ==0)
        {
            elementos.TextoPuntuacionResolucion.text = 0.ToString();
            yield break;
        }
        var puntuacionValor = 0;
        var scoreMoreThanZero = eventos.puntuacionActual > 0;
        while ((scoreMoreThanZero) ? puntuacionValor < eventos.puntuacionActual : puntuacionValor > eventos.puntuacionActual)
        {
            puntuacionValor += scoreMoreThanZero ? 1 : -1;
            elementos.TextoPuntuacionResolucion.text = puntuacionValor.ToString();
            yield return null;
        }
    }
    void CrearRespuestas(Question pregunta)
    {
        BorrarRespuestas();
        float offset = 0 - parametros.Margins;
        for(int i = 0; i< pregunta.respuestas.Length; i++)
        {
            AnswersData nuevaRespuesta = (AnswersData)Instantiate(prefabRespuesta, elementos.AreaRespuestasContenido);
            nuevaRespuesta.ActualizarDatos(pregunta.respuestas[i].info, i);
            nuevaRespuesta.Rect.anchoredPosition = new Vector2(0, offset);
            offset -= (nuevaRespuesta.Rect.sizeDelta.y + parametros.Margins);
            elementos.AreaRespuestasContenido.sizeDelta = new Vector2(elementos.AreaRespuestasContenido.sizeDelta.x, offset*-1);
            respuestaActual.Add(nuevaRespuesta);
        }
    }
    void BorrarRespuestas()
    {
        foreach(var respuesta in respuestaActual)
        {
            Destroy(respuesta.gameObject);
        }
        respuestaActual.Clear();
    }

    void ActualizarPuntuacionIU(int puntuacion)
    {
        elementos.TextoPuntuacion.text = "Score:" + puntuacion;
    }
}
