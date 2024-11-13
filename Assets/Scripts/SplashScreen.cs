using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    private float timer = 0;
  
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > 5) { VideoFinished(); timer = 0; return;  }

    }

    private void VideoFinished()
    {
        SceneManager.LoadScene("MainMenusScene");
    }
}
