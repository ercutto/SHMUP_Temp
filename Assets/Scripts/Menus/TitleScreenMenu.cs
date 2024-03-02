using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenMenu : Menu
{
    public static TitleScreenMenu instance = null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Title Screen Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    private void Update()
    { if (ROOT.gameObject.activeInHierarchy)
        {
            if (InputManager.instance.CheckForPlayerInput(0))
            {
                TurnOff(true);
                MainMenu.instance.TurnOn(this);
            }
        }
    }
    //public  void OnFireButton()
    //{
    //    TurnOff(true);
    //    MainMenu.instance.TurnOn(this);
    //}
}
