using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsOptionsMenu : Menu
{
    public static GraphicsOptionsMenu instance = null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Graphics Options Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
  
    public void OnApplyButton()
    {

    }
    public void OnBackButton()
    {
        TurnOff(true); //Simdi bu menuyu kapatiyoruz ve bir oncekine donuyoruz
    }
}
