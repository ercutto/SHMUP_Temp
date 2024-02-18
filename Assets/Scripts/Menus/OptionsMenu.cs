using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : Menu
{
    public static OptionsMenu instance = null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Options Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void OnBackButton()
    {
        TurnOff(true); //Simdi bu menuyu kapatiyoruz ve bir oncekine donuyoruz
    }

    public void OnGraphicsButton()
    {
        TurnOff(false);
        GraphicsOptionsMenu.instance.TurnOn(this);
    }
    public void OnAudioButton()
    {
        TurnOff(false);
        AudioOptionsMenu.instance.TurnOn(this);
    }
    public void OnControlsButton()
    {
        TurnOff(false);
        ControlsOptionsMenu.instance.TurnOn(this);
    }
}
