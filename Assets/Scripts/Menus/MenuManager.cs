
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance = null;

    internal Menu activeMenu = null;

    private bool titleMenuShown = false;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one menu manager!");
            Destroy(gameObject);
            return;
        }


        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SwitchToGameplayMenus()
    {        
        SceneManager.LoadScene("PauseMenu",             LoadSceneMode.Additive);
        SceneManager.LoadScene("GraphicsOptionsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("AudioOptionsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("ControlsOptionsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("YesNoMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("ControllerMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("GameOverMenu", LoadSceneMode.Additive);
        //Debug Scene
        SceneManager.LoadScene("DebugHUDScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("WellDoneMenu", LoadSceneMode.Additive);

    }
    public void SwitchToMainMenuMenus()
    {
        Debug.Log("Switch to Main Menu");
        SceneManager.LoadScene("MainMenu",       LoadSceneMode.Additive);
        SceneManager.LoadScene("ScoresMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("MedalsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("CreditsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("PractiseMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("PractiseArenaMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("PractiseStageMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("PlayMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("CraftSelectMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("AudioOptionsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("ReplaysMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("ControlsOptionsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("GraphicsOptionsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("YesNoMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("ControllerMenu", LoadSceneMode.Additive);
       


        //Debug Scene
        SceneManager.LoadScene("DebugHUDScene", LoadSceneMode.Additive);

        if (!titleMenuShown)
        {
            SceneManager.LoadScene("TitleSceneMenu", LoadSceneMode.Additive);
        }
        else
        {
            StartCoroutine(ShowMainMenu());
        }
        


    }

    IEnumerator ShowMainMenu()
    {
        while (MainMenu.instance == null)
        {
            yield return null;
        }
        MainMenu.instance.TurnOn(null);
    }
}
