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
    public int stageNumber = 0;

    [Header("Game mode To Play")]
    public GameMode gameMode;
    [Space]
    public GameObject gameManagerPrefab = null;
    private bool initialised=false;

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
    private void Update()
    {
       if(!initialised)
        {
            if(gameMode == GameMode.INVALID)
                return;

            if(!displayScene.isLoaded)
            {
                SceneManager.LoadScene("DisplayScene",LoadSceneMode.Additive);

            }

            switch (gameMode)
            {
                case GameMode.Menus:
                   
                    MenuManager.instance.SwitchToMainMenuMenus();
                    GameManager.Instance.gameState = GameManager.GameState.InMenus;
                    break;


                case GameMode.Gameplay:
                   
                    MenuManager.instance.SwitchToGameplayMenus();
                    GameManager.Instance.gameState = GameManager.GameState.Playing;
                    GameManager.Instance.gameSession.stage = stageNumber;

                    break;
            };

            if (playMusicTrack != AudioManager.Tracks.None)
            {
                AudioManager.instance.PlayMusic(playMusicTrack, true, 1);
            }

            if (gameMode == GameMode.Gameplay)
            {
                SaveManager.instance.SaveGame(0);//0= autosave at beginning stage
                GameManager.Instance.SpawnPlayers();
            }

            initialised = true;
            
        }
    }

  
}
