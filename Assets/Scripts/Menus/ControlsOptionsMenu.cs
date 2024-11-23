
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlsOptionsMenu : Menu
{
    public static ControlsOptionsMenu instance = null;
    public Button[] p1_buttons=new Button[8];
    public Button[] p2_buttons=new Button[8];
    public Button[] p1_keys=new Button[8];
    public Button[] p2_keys=new Button[8];
    public GameObject bindingPanel = null;
    public Text bindingText = null;
    public EventSystem eventSystem = null;

    private bool bindingKey = false;
    private bool bindingAxis = false;
    private bool bindingButton = false;
    private int actionBinding = -1;
    private int playerBinding = -1;

    private bool waiting = false;
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
    public void Update()
    {
       
        if(bindingKey||bindingButton)
        {
            if (waiting)
            {
                if (Input.anyKey) return;
                if (InputManager.instance.DetectButtonPress() > -1) return;
                waiting= false;
            }
            else
            {
                if (bindingKey)
                {
                   
                    foreach (KeyCode key in KeyCode.GetValues(typeof(KeyCode)))
                    {
                        if (!key.ToString().Contains("Joystick"))
                        {
                          
                            if (Input.GetKeyDown(key))//tusa basildi
                            {
                               
                                if (bindingAxis)
                                    InputManager.instance.BindingPlayerAxisKey(playerBinding, actionBinding, key);
                                else
                                    InputManager.instance.BindingPlayerKey(playerBinding, actionBinding, key);
                                bindingPanel.SetActive(false);
                                bindingKey = false;
                                bindingButton = false;
                                eventSystem.gameObject.SetActive(true);
                                UpdateButtons();
                               
                            }
                        }
                    }
                }
                else if (bindingButton)
                {
                  
                    int button = InputManager.instance.DetectButtonPress();
                    if (button > -1)//button pressed
                    {
                        InputManager.instance.BindPlayerButton(playerBinding, actionBinding, (byte)button);
                        bindingPanel.SetActive(false);
                        bindingKey = false;
                        bindingButton = false;
                        eventSystem.gameObject.SetActive(true);
                        UpdateButtons();
                    }
                }
            }
        }
       
           
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

    public void BindP1Key(int actionID)
    {
       
        eventSystem.gameObject.SetActive(false);
        bindingText.text="Press a key for player 1 " + InputManager.actionNames[actionID];
        bindingPanel.SetActive(true);

        bindingKey    = true;
        bindingAxis   = false;
        bindingButton = false;
        playerBinding = 0;
        actionBinding = actionID;

        waiting = true;
    }
    public void BindP1AxisKey(int actionID)
    {
        eventSystem.gameObject.SetActive(false);
        bindingText.text = "Press a key for player 1 " + InputManager.axisNames[actionID];
        bindingPanel.SetActive(true);

        bindingKey = true;
        bindingAxis = true;
        bindingButton = false;
        playerBinding = 0;
        actionBinding = actionID;

        waiting = true;
    }
    public void BindP1Button(int actionID)
    {
        eventSystem.gameObject.SetActive(false);
        bindingText.text = "Press a button for player 1 " + InputManager.actionNames[actionID];
        bindingPanel.SetActive(true);

        bindingKey = false;
        bindingAxis = false;
        bindingButton = true;
        playerBinding = 0;
        actionBinding = actionID;

        waiting = true;
    }
    public void BindP2Key(int actionID)
    {
      
        eventSystem.gameObject.SetActive(false);
        bindingText.text = "Press a key for player 2 " + InputManager.actionNames[actionID];
        bindingPanel.SetActive(true);

        bindingKey = true;
        bindingAxis = false;
        bindingButton = false;
        playerBinding = 1;
        actionBinding = actionID;

        waiting = true;
    }
    public void BindP2AxisKey(int actionID)
    {
        eventSystem.gameObject.SetActive(false);
        bindingText.text = "Press a key for player 2 " + InputManager.axisNames[actionID];
        bindingPanel.SetActive(true);

        bindingKey = true;
        bindingAxis = true;
        bindingButton = false;
        playerBinding = 1;
        actionBinding = actionID;
        waiting = true;
    }
    public void BindP2Button(int actionID)
    {
        eventSystem.gameObject.SetActive(false);
        bindingText.text = "Press a button for player 2 " + InputManager.actionNames[actionID];
        bindingPanel.SetActive(true);

        bindingKey = false;
        bindingAxis = false;
        bindingButton = true;
        playerBinding = 1;
        actionBinding = actionID;
        waiting = true;
    }

}
