using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenMenu : Menu
{
    public  void OnFireButton()
    {
        TurnOff(true);
        MainMenu.instance.TurnOn(this);
    }
}
