using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsOptionsMenu : Menu
{
    public static ControlsOptionsMenu instance = null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Constrols Options Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
