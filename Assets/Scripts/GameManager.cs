using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance=null;


    void Start()
    {
        if(Instance)
        {
            Debug.LogError("trying to create more than one Game Manager!");
            Destroy(gameObject);
            return;
        }

        Instance = this;    
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameManager created. ");

    }

}
