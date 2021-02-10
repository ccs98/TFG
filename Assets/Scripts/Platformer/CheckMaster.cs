using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckMaster : MonoBehaviour
{
   private static CheckMaster instance;
   public Vector2 posicionUltimoPuntoControl;
   private static CheckMaster now;
   private
   void Awake()
   {
       now = GameObject.FindGameObjectWithTag("CheckMaster").GetComponent<CheckMaster>();
       if(instance == null)
       {
           instance = this;
           DontDestroyOnLoad(instance);
       }
       else
       {
           Destroy(this);
       }
   }
}
