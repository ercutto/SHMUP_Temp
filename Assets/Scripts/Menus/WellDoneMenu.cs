using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WellDoneMenu : Menu
{

    public static WellDoneMenu Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance)
        {
            Debug.Log("Trying to create more than one WellDoneMenu!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OnContinueButton()
    {
        SceneManager.LoadScene("MainMenusScene");
    }


}
