using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeMenu : Menu
{
    public static PracticeMenu instance = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Practice Stage Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

}
