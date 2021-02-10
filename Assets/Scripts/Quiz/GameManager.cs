using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    #region variables
   private Data datos = new Data();

   [SerializeField] GameEvents eventos = null;
   public GameEvents Eventos {get{return eventos;}}

   private List<AnswersData> respuestasEscogidas = new List<AnswersData>();
   private List<int> preguntasFinalizadas = new List<int>();
   private int preguntaActual = 0;
   private IEnumerator IEWaitTillNextRound = null;
   private int puntoControl = 0;
   private bool terminado
   {
       get{
           return (preguntasFinalizadas.Count < datos.preguntas.Length) ? false : true;
       }
   }
   #endregion

    #region Metodos
   void OnEnable()
   {
       eventos.actualizarPreguntasResuesta += ActualizarRespuestas;
   }
   void OnDisable()
   {
       eventos.actualizarPreguntasResuesta -= ActualizarRespuestas;
   }

   void Awake()
   {
        eventos.puntuacionActual = 0; 
        puntoControl = PermanentUI.perm.puntos;
   }
   void Start()
   {
       puntoControl = PermanentUI.perm.puntos;
       eventos.StartupHighScore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
       LoadData();
       var seed = UnityEngine.Random.Range(int.MinValue,int.MaxValue);
       UnityEngine.Random.InitState(seed);
       Display();  
   }

   public void ActualizarRespuestas(AnswersData nuevaRespuesta)
   {
       if(datos.preguntas[preguntaActual].tipo == tipoRespuesta.Single)
       {
           foreach(var respuesta in respuestasEscogidas)
           {
               if(respuesta != nuevaRespuesta)
               {
                   respuesta.Reset();
               }
           }
            respuestasEscogidas.Clear();
            respuestasEscogidas.Add(nuevaRespuesta);
       }
       else
       {
           bool cogida = respuestasEscogidas.Exists(x => x == nuevaRespuesta);
           if(cogida)
           {
               respuestasEscogidas.Remove(nuevaRespuesta);
           }
           else
           {
               respuestasEscogidas.Add(nuevaRespuesta);
           }
       }
   }

   public void BorrarRespuestas()
   {
       respuestasEscogidas = new List<AnswersData>();
   }
   void Display()
   {
       BorrarRespuestas();
       var pregunta = GetPreguntaAleatoria();
       if(eventos.actualizarPreguntaIU != null)
       {
           eventos.actualizarPreguntaIU(pregunta);
       }
       else
       {
           Debug.LogWarning("Something went wrong");
       }
   }

   public void Aceptar()
   {
       bool esCorrecto = VerRespuestas();
       preguntasFinalizadas.Add(preguntaActual);
       ActualizarPuntuacion((esCorrecto) ? datos.preguntas[preguntaActual].puntuacion : 0);
       if(terminado)
       {
           SetPuntuacionMaxima();
       }
       var tipo = (terminado) ? UIManager.tipoPantallaResolucion.Final : (esCorrecto) ? UIManager.tipoPantallaResolucion.Correcto : UIManager.tipoPantallaResolucion.Incorrecto;
       
       eventos.mostrarPantalla?.Invoke(tipo, datos.preguntas[preguntaActual].puntuacion);
       
       AudioManager.instancia.PlaySonido((esCorrecto) ? "correcto" : "incorrecto");
       if(tipo != UIManager.tipoPantallaResolucion.Final)
       {
           if (IEWaitTillNextRound != null)
           {
               StopCoroutine(IEWaitTillNextRound);
            }
            IEWaitTillNextRound = Espera();
            StartCoroutine(IEWaitTillNextRound);
       }
       
   }


   IEnumerator Espera()
   {
       yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
       Display();
   }

    bool VerRespuestas()
    {
        if(!CompararRespuestas())
        {
            return false;
        }
        return true;
    }

    bool CompararRespuestas()
    {
        if(respuestasEscogidas.Count > 0)
        {
            List<int> c = datos.preguntas[preguntaActual].GetRespuestaCorrecta();
            List<int> p = respuestasEscogidas.Select(x => x.IndiceRespuesta).ToList();
            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();
            return !f.Any() && !s.Any();
        }
        return false;
    }

    Question GetPreguntaAleatoria()
   {
       var indiceAleatorio = GetPreguntaAleatoriaIndice();
       preguntaActual = indiceAleatorio;
       return datos.preguntas[preguntaActual];
   }

   int GetPreguntaAleatoriaIndice()
   {
       var random = 0;
       if(preguntasFinalizadas.Count < datos.preguntas.Length)
       {
           do
           {
               random = UnityEngine.Random.Range(0,datos.preguntas.Length);
           }while(preguntasFinalizadas.Contains(random) || random == preguntaActual);
       }
       return random;
   }
   void LoadData()
   {
       var archive = PermanentUI.perm.puntos-1;
       var path = Path.Combine(GameUtility.archivoXmlPath, GameUtility.archivoXmlNombre + archive.ToString() + ".xml");
       datos = Data.Fetch(path, PermanentUI.perm.puntos);
   }

   private void SetPuntuacionMaxima()
   {
       var maxima = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
       if(maxima < eventos.puntuacionActual)
       {
           PlayerPrefs.SetInt(GameUtility.SavePrefKey, eventos.puntuacionActual);
       }
   }

   public void RestarGame()
   {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }

   public void QuitarGame()
   {
       if(eventos.puntuacionActual < 3){
           Application.Quit();
       }
       SceneManager.LoadScene("SampleScene");
   }
   private void ActualizarPuntuacion(int añadir)
   {
       eventos.puntuacionActual += añadir;
       eventos.actualizarPuntuacion?.Invoke(eventos.puntuacionActual);
   }
   #endregion
}
