
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public AnimatedNumber[]playerScore=new AnimatedNumber[2];
    public AnimatedNumber topScore;
    public GameObject player2Start;
    public PlayerHud[] playerHuds=new PlayerHud[2];
    private void FixedUpdate()
    {
        UpdateHud();
    }
    public void UpdateHud()
    {
        if (!GameManager.Instance) return;

        //score
        if (playerScore[0])
        {
            int p1Score = GameManager.Instance.playerDatas[0].score;
            playerScore[0].UpdateNumber(p1Score);
        }

        UpdateLives(0);

        if (GameManager.Instance.twoPlayer)
        {
            if (player2Start)
            {
                player2Start.SetActive(false);
            }

            if (playerScore[1])
            {
                int p1Score = GameManager.Instance.playerDatas[1].score;
                playerScore[1].UpdateNumber(p1Score);
            }

            UpdateLives(1);
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

