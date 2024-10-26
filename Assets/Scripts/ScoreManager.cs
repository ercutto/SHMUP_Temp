using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance=null;

    int currentMultiplier = 1;
    void Start()
    {
        if(instance)
        {
            Debug.Log("trying to create more than 1 scoreManager instance!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    //vurulan objelerde oyuncunun indexine göre puan eklmek icin 
    public void ShootableHit(int playerIndex,int score)
    {
        GameManager.Instance.playerCrafts[playerIndex].IncreaseScore(score*currentMultiplier);
    }
    //yokedilen objelerde oyuncunun indexine göre puan eklmek icin 
    public void ShootableDestroyed(int playerIndex, int score)
    {
        GameManager.Instance.playerCrafts[playerIndex].IncreaseScore(score * currentMultiplier);
    }
    //yokedilen bosslardan oyuncunun indexine göre puan eklmek icin 
    public void BossDestroyed(int playerIndex, int score)
    {
        GameManager.Instance.playerCrafts[playerIndex].IncreaseScore(score * currentMultiplier);
    }
    //toplanan pickupdardan oyuncunun indexine göre puan eklmek icin 
    public void PickupCollected(int playerIndex, int score)
    {
        GameManager.Instance.playerCrafts[playerIndex].IncreaseScore(score * currentMultiplier);
    }
    //yokedilen dusman kursunlarindan oyuncunun indexine göre puan eklmek icin 
    public void BulletDestroyed(int playerIndex, int score)
    {
        GameManager.Instance.playerCrafts[playerIndex].IncreaseScore(score * currentMultiplier);
    }
    //toplanan madalyalardan oyuncunun indexine göre puan eklmek icin 
    public void MedalCollected(int playerIndex, int score)
    {
        GameManager.Instance.playerCrafts[playerIndex].IncreaseScore(score * currentMultiplier);
    }

    public void UpdateChaninMultiplier(int playerIndex)
    {
        int chain=GameManager.Instance.playerDatas[playerIndex].chain;
        currentMultiplier = (int)Math.Pow((chain + 1), 1.5);
    }

}
