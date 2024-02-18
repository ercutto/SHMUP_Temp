using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaysMenu : Menu
{
    public static ReplaysMenu instance = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Replays Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

}
