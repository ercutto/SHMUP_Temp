using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    public static MainMenu instance=null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one MainMenu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    public void OnPlayButton()
    {
        TurnOff(false);
        PlayMenu.instance.TurnOn(this);
    }
    public void OnPracticeButton()
    {
        TurnOff(false);
        PracticeMenu.instance.TurnOn(this);
    }
    public void OnOptionsButton()
    {
        TurnOff(false);
        OptionsMenu.instance.TurnOn(this);
    }
    public void OnScoreButton()
    {
        TurnOff(false);
        ScoresMenu.instance.TurnOn(this);
    }
    public void OnMedalsButton()
    {
        TurnOff(false);
        MedalsMenu.instance.TurnOn(this);
    }
    public void OnReplaysButton()
    {
        TurnOff(false);
        ReplaysMenu.instance.TurnOn(this);
    }
    public void OnCreditsButton()
    {
        TurnOff(false);
        CreditsMenu.instance.TurnOn(this);
    }
    public void Quit()
    {
        TurnOff(false);
        YesNoMenu.instance.TurnOn(this);

    }
}
