using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable()]
public struct ParametrosSonido
{
    [Range(0,1)]
    public float volumen;
    [Range(-3,3)]
    public float tono;
    public bool bucle;
}
[System.Serializable()]
public class Sonido
{
    [SerializeField] string nombre = string.Empty;
    public string Nombre {get{return nombre;}}
    [SerializeField] AudioClip clip = null;
    public AudioClip Clip {get{return clip;}}
    [SerializeField] ParametrosSonido parametros = new ParametrosSonido();
    public ParametrosSonido Parametros {get{return parametros;}}
    [HideInInspector]
    public AudioSource Source = null;
    public void Play()
    {
        Source.clip = Clip;
        Source.volume = Parametros.volumen;
        Source.pitch = Parametros.tono;
        Source.loop = Parametros.bucle;
        Source.Play();
    }
    public void Stop()
    {
        Source.Stop();
    }
}


public class AudioManager : MonoBehaviour
{
    #region variables
    public static AudioManager instancia = null;
    [SerializeField]Sonido[] sonidos = null;
    [SerializeField]AudioSource sourcePrefab = null;
    [SerializeField] string primeraTrack = string.Empty;
    #endregion

    #region metodos
    void Awake()
    {
        if(instancia != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        Iniciar();
    }
    void Start()
    {
        if(string.IsNullOrEmpty(primeraTrack) != true)
        {
            PlaySonido(primeraTrack);
        }
    }

    void Iniciar()
    {
        foreach (var sonido in sonidos)
        {
            AudioSource source = (AudioSource) Instantiate(sourcePrefab, gameObject.transform);
            source.name = sonido.Nombre;
            sonido.Source = source;
        }   
    }

    public void PlaySonido(string nombre)
    {
        var sonido = ObtenerSonido(nombre);
        if(sonido != null)
        {
            sonido.Play();
        }
        else
        {
            Debug.LogWarning("Sound "+ sonido+ " not found!");
        }
    }
    public void StopSonido(string nombre)
    {
        var sonido = ObtenerSonido(nombre);
        if(sonido != null)
        {
            sonido.Stop();
        }
        else
        {
            Debug.LogWarning("Sound "+ sonido+ " not found!");
        }
    }
    Sonido ObtenerSonido(string nombre)
    {
        foreach (var sonido in sonidos)
        {
            if(sonido.Nombre == nombre)
            {
                return sonido;
            }
        }
        return null;
    }
    #endregion
}
