using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerMenu : Menu
{
    public static ControllerMenu instance = null;
    public int whichPlayer = 0;
    public Text playerText = null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Controller Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    private void Update()
    {
        if (ROOT.gameObject.activeInHierarchy)
        {
            if (InputManager.instance.CheckForPlayerInput(whichPlayer))
            {
                TurnOff(false);
                //GameManager.instance.ResumeGameplay();
            }
        }
    }
}
