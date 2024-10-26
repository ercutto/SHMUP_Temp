using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance = null;

    private bool displaying = false;

    public GameObject ROOT = null;

    public Toggle invincibleToggle = null;
    void Start()
    {
        if (Instance)
        {
            Debug.Log("Trying to create more than one DebugManager!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }
    
    public void ToggleHUD()
    {
        if (!displaying)//turn on
        {
            if (!ROOT)
            {
                Debug.LogError("ROOT Object is not Set");
            }
            else
            {
                ROOT.SetActive(true);
                displaying = true;
                Time.timeScale = 0;
            }
        }
        else // turn off
        {
            if (!displaying)//turn on
            {
                if (!ROOT)
                {
                    Debug.LogError("ROOT Object is not Set");
                }
                else
                {
                    ROOT.SetActive(false);
                    displaying = false;
                    Time.timeScale = 1;

                }
            }
        }
    }

    public void ToggleIOnvincibility() {

        if (invincibleToggle)
        {
            GameManager.Instance.gameSession.invincible = invincibleToggle.isOn;
        }
    }

   
}
