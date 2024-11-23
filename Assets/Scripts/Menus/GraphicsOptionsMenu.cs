
using UnityEngine;
using UnityEngine.UI;

public class GraphicsOptionsMenu : Menu
{
    public static GraphicsOptionsMenu instance = null;
    public Toggle fullScreenToggle = null;

    public Button nextButton = null;
    public Button prevButton = null;
    public Text resolutionText = null;
    bool fullScreenToApply = true;

    Resolution resolutionToAplly;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Graphics Options Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (fullScreenToggle)
            fullScreenToggle.isOn = ScreenManager.instance.fullScreen;
        fullScreenToApply= ScreenManager.instance.fullScreen;

        resolutionToAplly = ScreenManager.instance.currentResolution;
        if (resolutionText)
            resolutionText.text = resolutionToAplly.width.ToString()+" - "+ resolutionToAplly.height.ToString();
    }
  
    public void OnApplyButton()
    {
        ScreenManager.instance.fullScreen=fullScreenToApply;
        Screen.fullScreen = fullScreenToApply;
        if (fullScreenToApply)
        {
            Debug.Log("Going FullScreen");
            PlayerPrefs.SetInt("FullScreen", 1);
        }
        else
        {
            Debug.Log("Going windowed");

            PlayerPrefs.SetInt("FullScreen", 0);
        }
        PlayerPrefs.Save();

    }

    public void OnBackButton()
    {
        TurnOff(true); //Simdi bu menuyu kapatiyoruz ve bir oncekine donuyoruz
    }
    public void OnFullScreenToggle()
    {
        fullScreenToApply = !fullScreenToApply;
    }
    public void OnVSyncButton()
    {

    }
    public void OnNexButton()
    {
        resolutionToAplly= ScreenManager.instance.NextResolution(resolutionToAplly);
        if (resolutionText)
            resolutionText.text = resolutionToAplly.width.ToString() + " - " + resolutionToAplly.height.ToString();

    }
    public void OnPrevButton()
    {
        resolutionToAplly = ScreenManager.instance.PrevResolution(resolutionToAplly);
        if (resolutionText)
            resolutionText.text = resolutionToAplly.width.ToString() + " - " + resolutionToAplly.height.ToString();
    }
}
