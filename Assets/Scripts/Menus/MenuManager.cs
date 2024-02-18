
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance = null;

    internal Menu activeMenu = null;

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
        Debug.Log("Switch to game play");
        SceneManager.LoadScene("PauseMenu",             LoadSceneMode.Additive);
        SceneManager.LoadScene("GraphicsOptionsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("AudioOptionsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("ControlsOptionsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("YesNoMenu", LoadSceneMode.Additive);

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
        SceneManager.LoadScene("TitleSceneMenu", LoadSceneMode.Additive);


    }
}
