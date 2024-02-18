using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalsMenu : Menu
{
    public static MedalsMenu instance = null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Medals Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
   
}
