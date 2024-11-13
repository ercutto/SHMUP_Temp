using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitialiser : MonoBehaviour
{
    public enum GameMode
    {
        INVALID,
        Menus,
        Gameplay
    }
    [Header("Game mode To Play")]
    public GameMode gameMode;
    [Space]
    public GameObject gameManagerPrefab = null;
    private bool menuLoaded=false;

    private Scene displayScene;

    public AudioManager.Tracks playMusicTrack=AudioManager.Tracks.None; 
    void Start()
    {
        if (GameManager.Instance == null)
        {
            if (gameManagerPrefab)
            {
                Instantiate(gameManagerPrefab);
                displayScene = SceneManager.GetSceneByName("DisplayScene");
            }
            else
            {
                Debug.LogError("Game Manager prefab is not set! ");
            }
        }

      
       
    }
    private void FixedUpdate()
    {
       if(!menuLoaded)
        {
            if(!displayScene.isLoaded)
            {
                SceneManager.LoadScene("DisplayScene",LoadSceneMode.Additive);

            }

            ChangeGameMode();
        }
    }

    void ChangeGameMode()
    {
       
        switch (gameMode)
        {
            case GameMode.Menus:
                Debug.Log("Menu");
                MenuManager.instance.SwitchToMainMenuMenus();
                break;
        

            case GameMode.Gameplay:
                Debug.Log("Game");
                MenuManager.instance.SwitchToGameplayMenus();
                break;
        };

        if (playMusicTrack != AudioManager.Tracks.None)
        {
            AudioManager.instance.PlayMusic(playMusicTrack, true, 1);
        }

        if (gameMode == GameMode.Gameplay)
        { 
            GameManager.Instance.SpawnPlayers();
        }

        menuLoaded = true;
    }
}
