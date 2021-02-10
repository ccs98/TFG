using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string nombreEscena;

    private void OnTriggerEnter2D(Collider2D colision)
    {
        if(PermanentUI.perm.puntos >= 4){
            SceneManager.LoadScene(nombreEscena);
        }else{
            PermanentUI.perm.Informacion();
        }
    }
}
