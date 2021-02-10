using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoints : MonoBehaviour
{  
    private CheckMaster cm;
    private Transform pl;
    void Start()
    {
        cm = GameObject.FindGameObjectWithTag("CheckMaster").GetComponent<CheckMaster>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            cm.posicionUltimoPuntoControl.x = transform.position.x;
            cm.posicionUltimoPuntoControl.y = transform.position.y + 1;
            Destroy(gameObject);
        }
    }
    /*void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
 
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
 
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // here you can use scene.buildIndex or scene.name to check which scene was loaded
        pl = GameObject.FindGameObjectWithTag("Player").transform;
        cm = GameObject.FindGameObjectWithTag("CheckMaster").GetComponent<CheckMaster>();
        if (scene.name == "SampleScene"){
            if(cm.posicionUltimoPuntoControl.x == pl.position.x){
                Destroy(gameObject);
            }
        }
    }*/
}
