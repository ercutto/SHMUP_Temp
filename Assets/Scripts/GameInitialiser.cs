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
    void Start()
    {
        if (GameManager.Instance == null)
        {
            if (gameManagerPrefab)
            {
                Instantiate(gameManagerPrefab);
            }
            else
            {
                Debug.LogError("Game Manager prefab is not set! ");
            }
        }

        Invoke(nameof(Check), 7);
       
    }
    private void Update()
    {
       
    }

    void Check()
    {
        gameMode=GameMode.Menus;
       
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

    }
}
