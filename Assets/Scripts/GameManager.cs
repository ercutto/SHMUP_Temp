using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance=null;

    public bool twoPlayer = false;
    public GameObject[] craftPrefab;

    public BulletManager bulletManager=null;

    //public Craft playerOneCraft = null;
    public Craft[] playerCrafts = new Craft[2];

    public LevelProgress progressWindow = null;

    public PlayerData[] playerDatas = null;

    public Session gameSession =new Session();

    public PickUp[] cylicDrops=new PickUp[15];
    public PickUp[] Medals=new PickUp[10];
    private int currentDropIndex = 0;
    public int currentMedalIndex = 0;
    public PickUp option = null;
    public PickUp powrup = null;
    public PickUp beamup = null;

    //Craft playerTwoCraft = null;

    public enum GameState
    {
        INVALID,
        InMenus,
        Playing,
        Paused

    }
    public GameState gameState = GameState.INVALID; 
    void Start()
    {
        if(Instance)
        {
            Debug.LogError("trying to create more than one Game Manager!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        playerDatas = new PlayerData[2];
        playerDatas[0] = new PlayerData();
        playerDatas[1] = new PlayerData();

        
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameManager created. ");
        bulletManager = GetComponent<BulletManager>();

        Application.targetFrameRate = 60;

    }
    public void SpawnPlayer(int playerIndex,int craftType)
    {
        Debug.Assert(craftType < craftPrefab.Length);

        playerCrafts[playerIndex] = Instantiate(craftPrefab[craftType]).GetComponent<Craft>();
        playerCrafts[playerIndex].playerIndex = playerIndex;
       


    }

    public void SpawnPlayers()
    {
        if(twoPlayer==false)
            SpawnPlayer(0, 0);
       

        if (twoPlayer)
        {
            SpawnPlayer(1, 0);
        }
    }

    public void DelayedRespawn(int playerIndex)
    {
        StartCoroutine(RespawnCoroutine(playerIndex));
    }

    IEnumerator RespawnCoroutine(int playerIndex)
    {
        yield return new WaitForSeconds(1.5f);
        SpawnPlayer(playerIndex, 0);// get craft type
        yield return null;
    }
    public void ResetState(int playerIndex)
    {
        
        CraftData craftData = gameSession.craftDatas[playerIndex];
        craftData.positionX = 0;
        craftData.positionY = 0;
        craftData.ShotPower = 0;
        craftData.noOfEnableOptions = 0;
        craftData.optionsLayout = 0;
        craftData.beamFiring = false;
        craftData.beamCharge = 0;
        craftData.beamPower = 0;
        craftData.beamTimer = 0;
        craftData.smallBombs = 3;
        craftData.largeBombs = 0;
        


    }
    public void RestoreState(int playerIndex)
    {
        int number = gameSession.craftDatas[playerIndex].noOfEnableOptions;
        gameSession.craftDatas[playerIndex].noOfEnableOptions = 0;

        gameSession.craftDatas[playerIndex].positionX = 0;
        gameSession.craftDatas[playerIndex].positionY = 0;
        for (int o = 0; o < number; o++)
        {
            playerCrafts[playerIndex].AddOption(0);
        }
    }
    public void Update()
    {
        CraftData craftData = gameSession.craftDatas[0];
        Debug.Log(playerDatas[0].lives);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        //if (Input.GetKeyDown(KeyCode.Alpha1)){
        //    if (!playerCrafts[0]) SpawnPlayer(0, 0);
        //}

        //if (Input.GetKeyDown(KeyCode.P))
        //{
 
        //    if (playerCrafts[0] && (int)playerCrafts[0].craftData.ShotPower < CraftConfiguration.MAX_SHOT_POWER - 1)
        //        playerCrafts[0].craftData.ShotPower++;
        //}

        if (Input.GetKeyDown(KeyCode.S))
        {
           if(bulletManager)
            {
                
               // bulletManager.SpawnBullet(BulletManager.BulletType.Bullet1_Size3,0,150,Random.Range(-10f,10), Random.Range(-10f, 10),0);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (playerCrafts[0])
            {
                playerCrafts[0].IncreaseBeamStrength(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EnemyPattern testPattern = GameObject.FindAnyObjectByType<EnemyPattern>();
            testPattern.Spawn();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (playerCrafts[0])
                playerCrafts[0].AddOption(0);
        }

        if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.BackQuote))
        {
            DebugManager.Instance.ToggleHUD();   
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AudioManager.instance.PlayMusic(AudioManager.Tracks.Level01, true, 2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AudioManager.instance.PlayMusic(AudioManager.Tracks.Level02, true, 2);
        }

      
    }
    public void TogglePause()
    {
        if (gameState == GameState.Playing)
        {
            gameState = GameState.Paused;
            AudioManager.instance.PauseMusic();
            PauseMenu.instance.TurnOn(null);
            if (DebugManager.Instance.displaying)
            {
                DebugManager.Instance.ToggleHUD();
            }
            Time.timeScale = 0;
        }
        else//vurretnly paused,so unpause
        {
            gameState = GameState.Playing;
            AudioManager.instance.ResumeMusic();
            PauseMenu.instance.TurnOff(false);
            Time.timeScale = 1;

        }
    }
    public void StartGame()
    {
        gameState=GameState.Playing;
        
        ResetState(0);
        ResetState(1);
       
        playerDatas[0].score = 0;
        playerDatas[1].score = 0;
       
        SceneManager.LoadScene("Stage01");
    }
    public void PickUpFallOffScreen(PickUp pickUp)
    {
        if (pickUp.config.type == PickUp.PickupType.Medal)
        {
            currentMedalIndex = 0;
        }
    }
    public PickUp GetNextDrop()
    {
        PickUp result = cylicDrops[currentDropIndex];

        if (result.config.type == PickUp.PickupType.Medal)
        {
            result =Medals[currentMedalIndex];
            currentMedalIndex++;
            if (currentMedalIndex > 9)
            {
                currentDropIndex = 0;
            }
        }

        currentDropIndex++;
        if (currentDropIndex > 14)
        {
            currentDropIndex = 0;
        }

        return result;
    }

    public PickUp SpawnPickup(PickUp pickUpPrefab, Vector2 pos)
    {
       
        PickUp p = Instantiate(pickUpPrefab, pos, Quaternion.identity);
        if (p)
        {
            p.transform.SetParent(GameManager.Instance.transform);
        }

        return p;   
    }

    public void ResumeGameFromLoad()
    {
        gameState = GameState.Playing;
        switch (gameSession.stage)
        {
            case 1:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Stage01");
                break;
            case 2:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Stage02");

                break;


        }
    }
}
