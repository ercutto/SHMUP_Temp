using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject ROOT = null;
    public virtual void TurnOn()
    {
        if (ROOT)
        {
            ROOT.SetActive(true);
        }
        else
        {
            Debug.LogError(" Root object is not set!");
        }
    }
    public virtual void TurnOff()
    {
        if (ROOT)
        {
            ROOT.SetActive(false);
        }
        else
        {
            Debug.LogError(" Root object is not set!");
        }
    }
}
