using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu
{
    public static PauseMenu instance = null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Pause Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
