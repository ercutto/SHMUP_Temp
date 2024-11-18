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

    public void OnResumeButton()
    {
        GameManager.Instance.TogglePause();
        //TurnOff(false);
        //Time.timeScale = 1;
    }
    public void OnLoadButton()
    {
        if(SaveManager.instance.LoadExist(1))
        {
            SaveManager.instance.LoadGame(1);

        }
    }
    public void OnSaveButton()
    {
        SaveManager.instance.SaveGame(1);
    }
    public void OnOptionsButton()
    {

    }
    public void OnMainMenuButton()
    {

    }

}
