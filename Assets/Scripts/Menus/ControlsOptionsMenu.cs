using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsOptionsMenu : Menu
{
    public static ControlsOptionsMenu instance = null;
    public Button[] p1_buttons=new Button[8];
    public Button[] p2_buttons=new Button[8];
    public Button[] p1_keys=new Button[8];
    public Button[] p2_keys=new Button[8];

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Constrols Options Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    private void OnEnable()
    {
        UpdateButtons();
    }
    void UpdateButtons()
    {
        for (int _btn = 0; _btn < 8; _btn++)
        {
            p1_buttons[_btn].GetComponentInChildren<Text>().text=InputManager.instance.GetButtonName(0,_btn);
            p2_buttons[_btn].GetComponentInChildren<Text>().text=InputManager.instance.GetButtonName(1,_btn);
        }

        for (int _key = 0; _key < 8; _key++)
        {
            p1_keys[_key].GetComponentInChildren<Text>().text = InputManager.instance.GetKeyName(0, _key);
            p2_keys[_key].GetComponentInChildren<Text>().text = InputManager.instance.GetKeyName(1, _key);
        }

        for (int _axis = 0; _axis < 4; _axis++)
        {
            p1_keys[_axis+8].GetComponentInChildren<Text>().text = InputManager.instance.GetAxisName(0, _axis);
            p2_keys[_axis+8].GetComponentInChildren<Text>().text = InputManager.instance.GetAxisName(1, _axis);
        }
    }
    public void OnBackButton()
    {
        TurnOff(true); //Simdi bu menuyu kapatiyoruz ve bir oncekine donuyoruz
    }
}
