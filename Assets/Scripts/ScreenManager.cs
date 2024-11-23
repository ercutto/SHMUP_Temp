using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    
    public static ScreenManager instance=null;
    public bool fullScreen = true;

    public Resolution currentResolution;
    public Resolution[] allResolution;
   

    void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Screen manager!");
            Destroy(gameObject);
            return;
        }
        instance = this;

        currentResolution=Screen.currentResolution;
        allResolution=Screen.resolutions;
    }
    public void SetResolution(Resolution res)
    {   
        if(fullScreen)
            Screen.SetResolution(res.width, res.height,FullScreenMode.ExclusiveFullScreen);
        else
            Screen.SetResolution(res.width, res.height, FullScreenMode.Windowed);

        PlayerPrefs.SetInt("ScreenWidth",res.width);
        PlayerPrefs.SetInt("ScreenHeight",res.height);

        Cursor.visible = false;
        

    }

    void RestoreSetting()
    {
        //restore resolution
        int width = 1280;
        int height = 720;
        if( PlayerPrefs.HasKey("ScreenWidth"))
            width = PlayerPrefs.GetInt("ScreenWidth");
        if(PlayerPrefs.HasKey("ScreenHeight"))
            width = PlayerPrefs.GetInt("ScreenHeight");
        //Restore screenSettings
        Resolution res = FindResolution(width,height);

        if (!PlayerPrefs.HasKey("FullScreen"))
        {
            int fullScreenInt = PlayerPrefs.GetInt("FullScreen");
            if (fullScreenInt==0)
            {
                fullScreen = false;
            }
            else if (fullScreenInt == 1)
            {
                fullScreen=true;
            }
            else
            {
                Debug.LogError("fullScreen preference is invalid!");
            }
        }
        Screen.fullScreen = fullScreen;
    }

    Resolution FindResolution(int width,int height)
    {
        foreach(Resolution res in allResolution)
        {
            if(res.width==width && res.height==height)
                return res;
        }
        return currentResolution;
    }
   
    public Resolution NextResolution(Resolution currentResolution)
    {
        int currentIndex = FindResolution(currentResolution);
        currentIndex++;
        if(currentIndex>=allResolution.Length)
            currentIndex = 0;
        return allResolution[currentIndex];

    }

    public Resolution PrevResolution(Resolution currentResolution)
    {
        int currentIndex = FindResolution(currentResolution);
        currentIndex--;
        if (currentIndex >0)
            currentIndex = allResolution.Length-1;
        return allResolution[currentIndex];

    }

    int FindResolution(Resolution currentResolution)
    {
        int index = 0;
        foreach (Resolution res in allResolution)
        {
            if(currentResolution.width==res.width &&
               currentResolution.height == res.height)
            {
                return index;
            }
            else
            {
                index++;
            }
        }
        return -1;
    }
}
