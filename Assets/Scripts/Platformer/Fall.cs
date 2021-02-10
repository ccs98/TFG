using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D colision)
    {
        if(colision.gameObject.tag == "Player")
        {
            //PermanentUI.perm.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
