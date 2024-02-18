using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YesNoMenu : Menu
{
    public static YesNoMenu instance = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one YesNo Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

}
