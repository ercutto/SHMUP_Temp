
using System;

using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance = null;
    public AnimatedNumber[]playerScore=new AnimatedNumber[2];
    public AnimatedNumber topScore;
    public GameObject player2Start;
    public PlayerHud[] playerHuds=new PlayerHud[2];

    public Image FadeScreenImage = null;

    public GameObject player2HUD=null;
    public void Start()
    {
        if (Instance)
        {
            Debug.LogError("Trying to create more than 1 HUD!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        TurnOnP2(GameManager.Instance.twoPlayer);
    }


    private void FixedUpdate()
    {
        UpdateHud();
    }
    public void UpdateHud()
    {
        if (!GameManager.Instance) return;

        //score
        int p1Score = GameManager.Instance.playerDatas[0].score;
        int p2Score = GameManager.Instance.playerDatas[1].score;
        if (playerScore[0])
        {
            
            playerScore[0].UpdateNumber(p1Score);
        }

        int hardness=(int)GameManager.Instance.gameSession.hardness;
        int highScore = ScoreManager.instance.TopScore(hardness);

        if (p1Score > highScore)
        {
            topScore.UpdateNumber(p1Score);
        }
        else if(p2Score>highScore)
        {
            topScore.UpdateNumber(p2Score);
        }
        else
        {
            topScore.UpdateNumber(highScore);
        }

        UpdateLives(0);
        UpdateBombs(0);
        UpdatePower(0);
        UpdateBeam(0);
        UpdateControls(0);
        UpdateStats(0);
        UpdateStats(0);
        UpdateProgress(0);
        UpdateChain(0);



        if (GameManager.Instance.twoPlayer)
        {
            if (player2Start)
            {
                player2Start.SetActive(false);
            }

            if (playerScore[1])
            {
                p1Score = GameManager.Instance.playerDatas[1].score;
                playerScore[1].UpdateNumber(p1Score);
            }

            UpdateLives(1);
            UpdateBombs(1);
            UpdatePower(1);
            UpdateBeam(1);
            UpdateControls(1);
            UpdateStats(1);
            UpdateStats(1);
            UpdateProgress(1);
            UpdateChain(1);

        }
        else
        {
            if (player2Start)
            {
                player2Start.SetActive(true);
            }
        }
    }

    private void UpdateLives(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerData data= GameManager.Instance.playerDatas[playerIndex];
        PlayerHud hud=playerHuds[playerIndex];
        int lives= data.lives;
        for(int i= 0; i < 5; i++)
        {
            if (lives > i)
            {
                hud.lives[i].SetActive(true);
            }
            else
            {
                hud.lives[i].SetActive(false);

            }
        }
    }
    public void UpdateBombs(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHud hud=playerHuds[playerIndex];
        if (!GameManager.Instance.playerCrafts[playerIndex])
        {
            for (int i = 0; i < 5; i++)
            {
                hud.bigBombs[i].SetActive(false);
            }
            for (int i = 0; i < 8; i++)
            {
                hud.smallBombs[i].SetActive(false);
            }
            return;
        }

            CraftData data=GameManager.Instance.gameSession.craftDatas[playerIndex];

            int largeBomb = data.largeBombs;
            int smallBomb = data.smallBombs;

            for (int i = 0; i < 5; i++)
            {
                if(largeBomb > i)
                    hud.bigBombs[i].SetActive(true);
                else 
                    hud.bigBombs[i].SetActive(false);

            }
            for (int i = 0; i < 8; i++)
            {
                if (smallBomb > i)
                    hud.smallBombs[i].SetActive(true);
                else
                    hud.smallBombs[i].SetActive(false);
            }
        
    }

    public void UpdatePower(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHud hud = playerHuds[playerIndex];
        if (!GameManager.Instance.playerCrafts[playerIndex])
        {
            for (int i = 0;i < 8;i++)hud.powerMarks[i].SetActive(false);
            return;
        }

        CraftData data = GameManager.Instance.gameSession.craftDatas[playerIndex];
        //CraftData data = GameManager.Instance.playerCrafts[playerIndex].craftData;
        int power=data.ShotPower;
        for (int i = 0; i < 8; i++)
            if(power>i)hud.powerMarks[i].SetActive(true);
            else hud.powerMarks[i].SetActive(false);
    }
    private void UpdateBeam(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHud hud = playerHuds[playerIndex];
        if (!GameManager.Instance.playerCrafts[playerIndex])
        {
            for(int i = 0;i<5;i++)hud.beamMarks[i].SetActive(false);
            hud.beamGradient.fillAmount = 0;
            return;
        }

        CraftData data = GameManager.Instance.gameSession.craftDatas[playerIndex];
        //CraftData data = GameManager.Instance.playerCrafts[playerIndex].craftData;
        int beamPower=data.beamPower;

        for (int i = 0; i < 5; i++) 
            if(beamPower>i) hud.beamMarks[i].SetActive(true);
            else hud.beamMarks[i].SetActive(false);

        hud.beamGradient.fillAmount=(float)data.beamTimer/(float)Craft.MAXIMUMBEAMCHARGE;
    }
    private void UpdateControls(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHud hud = playerHuds[playerIndex];
        if (!GameManager.Instance.playerCrafts[playerIndex])
        {
            for (int i = 0; i < 4; i++) hud.buttons[i].SetActive(false);
            hud.left.SetActive(false);
            hud.right.SetActive(false);
            hud.up.SetActive(false);
            hud.down.SetActive(false);
            hud.joystick.SetActive(false);
            return;
        }

        InputState state = InputManager.instance.playerState[playerIndex];

        if(state.shoot) hud.buttons[0].SetActive(true);
        else hud.buttons[0].SetActive(false);

        if (state.beam) hud.buttons[1].SetActive(true);
        else hud.buttons[1].SetActive(false);

        if (state.bomb) hud.buttons[2].SetActive(true);
        else hud.buttons[2].SetActive(false);

        if (state.options) hud.buttons[3].SetActive(true);
        else hud.buttons[3].SetActive(false);

        if(state.left)hud.left.SetActive(true);
        else hud.left.SetActive(false);

        if (state.right) hud.right.SetActive(true);
        else hud.right.SetActive(false);

        if (state.down) hud.down.SetActive(true);
        else hud.down.SetActive(false);

        if (state.up) hud.up.SetActive(true);
        else hud.up.SetActive(false);

        hud.joystick.SetActive(true);
        hud.joystick.transform.localPosition=new Vector2(-338,-167)+state.movement*3;


    }
    private void UpdateStats(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHud hud = playerHuds[playerIndex];
        if (!GameManager.Instance.playerCrafts[playerIndex])
        {
            hud.speedStat.fillAmount = 0;
            hud.powerStat.fillAmount = 0;
            hud.beamStat.fillAmount = 0;
            hud.optionStat.fillAmount = 0;
            hud.bombStat.fillAmount = 0;
            return;
        }

        CraftConfiguration config = GameManager.Instance.playerCrafts[playerIndex].config;
        hud.speedStat.fillAmount=config.speed/(float)CraftConfiguration.MAX_SPEED;
        hud.powerStat.fillAmount=config.bulletStrength/(float)CraftConfiguration.MAX_SHOT_POWER;
        hud.beamStat.fillAmount=config.beamPower/(float)CraftConfiguration.MAX_BEAM_POWER;
        hud.optionStat.fillAmount=config.optionPower/(float)CraftConfiguration.MAX_OPTION_POWER;
        hud.bombStat.fillAmount=config.bombPower/(float)CraftConfiguration.MAX_BOMB_POWER;

    }

    private void UpdateStageScore(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHud hud = playerHuds[playerIndex];
        if (!GameManager.Instance.playerCrafts[playerIndex])
        {
            hud.stageScore.UpdateNumber(0);
            return;

        }

        hud.stageScore.UpdateNumber(GameManager.Instance.playerDatas[playerIndex].stageScore);
    }

    private void UpdateProgress(int playerIndex)
    {
        Debug.Assert(playerIndex < 2);
        PlayerHud hud = playerHuds[playerIndex];
        if (!GameManager.Instance||!GameManager.Instance.progressWindow)
        {
            hud.progressGradient.fillAmount=0;
            return;
        }

        float progress = GameManager.Instance.progressWindow.data.positionY/(float)GameManager.Instance.progressWindow.levelSize;
        hud.progressGradient.fillAmount=1-progress;

    }

    private void UpdateChain(int playerIndex)
    {
        Debug.Assert(playerIndex <2);
        PlayerHud hud = playerHuds[playerIndex];
        if (!GameManager.Instance.playerCrafts[playerIndex])
        {
            hud.chainScore.UpdateNumber(0);
            return;
        }

        hud.chainScore.UpdateNumber(GameManager.Instance.playerDatas[playerIndex].chain);
        hud.chainGradient.fillAmount = (float)GameManager.Instance.playerDatas[playerIndex].chainTimer / (float)PlayerData.MAXCHAINTIMER;
    }

    public void TurnOnP2(bool turnOn)
    {
        if (turnOn)
        {
            player2Start.gameObject.SetActive(false);
            playerScore[1].gameObject.SetActive(true);
            player2HUD.SetActive(true);
        }
        else
        {
            player2Start.gameObject.SetActive(true);
            playerScore[1].gameObject.SetActive(false);
            player2HUD.SetActive(false);
        }
    }

    public void FadeOutScreen()
    {
        FadeScreenImage.gameObject.SetActive(true);
        FadeScreenImage.color=Color.black;
    }
    public void FadeInScreen()
    {
        FadeScreenImage.gameObject.SetActive(false);
        FadeScreenImage.color = new Color(0,0,0,0);
    }
    //--
    [Serializable]
    public class PlayerHud
    {
        public GameObject[]lives = new GameObject[5];
        public GameObject[]bigBombs = new GameObject[5];
        public GameObject[]smallBombs = new GameObject[8];
        public AnimatedNumber chainScore;
        public Image chainGradient;
        public GameObject[] powerMarks=new GameObject[8];
        public GameObject[] beamMarks=new GameObject[5];
        public Image beamGradient;
        public Image progressGradient;
        public AnimatedNumber stageScore;

        public GameObject[]buttons=new GameObject[4];
        public GameObject up;
        public GameObject down;
        public GameObject left;
        public GameObject right;
        public GameObject joystick;

        public Image speedStat;
        public Image powerStat;
        public Image beamStat;
        public Image optionStat;
        public Image bombStat;
    }
}

