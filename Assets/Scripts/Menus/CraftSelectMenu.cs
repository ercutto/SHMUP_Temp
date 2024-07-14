using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftSelectMenu : Menu
{
    public static CraftSelectMenu instance = null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Craaft Select Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    public void OnBackButton()
    {
        TurnOff(true); //Simdi bu menuyu kapatiyoruz ve bir oncekine donuyoruz
    }
    public void OnPlayButton()
    {
        GameManager.Instance.StartGame();
    }
    public void OnCraftA_P1Button()
    {

    }
    public void OnCraftB_P1Button()
    {

    }
    public void OnCraftC_P1Button()
    {

    }
}
