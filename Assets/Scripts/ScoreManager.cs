using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance=null;

    int currentMultiplier = 1;

    public int[,] scores = new int[8, 4];
    public string[,]names = new string[8, 4];
    void Start()
    {
        if(instance)
        {
            Debug.Log("trying to create more than 1 scoreManager instance!");
            Destroy(gameObject);
            return;
        }
        instance = this;

        for (int h = 0; h < 4; h++)
        {
            for (int s = 0; s < 8; s++)
            {
                names[s, h] = "";
                scores[s, h] = 0;
            }
        }
      
        LoadScores();
       
    }

    public void AddScore(int score,int hardness,string name)
    {
        for (int s = 0; s < 8; s++)
        {
            if (score > scores[s,hardness])// insert here
            {
                ShuffleScoresDown(s, hardness);
                scores[s,hardness] = score;
                names[s,hardness] = name;
                return;
            }
        
        }

    }

    public bool IsTopScore(int score,int hardness)
    {
        if (score > scores[0, hardness])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsHiScore(int score, int hardness)
    {
        for (int s = 7; s >=0; s--)
        {
            if (score > scores[s, hardness])
                return true;
           
        }
        return false;

    }

    void ShuffleScoresDown(int scoreIndex, int hardness)
    {

        for(int s = 7;s > scoreIndex; s--)
        {
            scores[s,hardness]=scores[s-1,hardness];
            names[s,hardness]=names[s-1,hardness];
        }
    }

    public void SaveScore()
    {
        string savePath=Application.persistentDataPath+"/scrs.dat";
        Debug.Log("loadPath= "+savePath);

        FileStream fileStream=new FileStream(savePath, FileMode.OpenOrCreate);
        
        if (fileStream!=null)
        {
            BinaryWriter writer=new BinaryWriter(fileStream);
            if (writer != null)
            {
                for(int h = 0; h < 4; h++)
                {
                    for(int s=0; s < 8; s++)
                    {
                        writer.Write(names[s,h]);
                        writer.Write(scores[s,h]);
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to create Binary writer for saving HiScore!");
            }
        }
        else
        {
            Debug.LogError("Failed to create or open fileStream for saving hiscores!");
        }
    }
    public void LoadScores()
    {
        string loadPath = Application.persistentDataPath + "/scrs.dat";
        Debug.Log("loadPath= " + loadPath);

        try
        {
            FileStream fileStream = new FileStream(loadPath, FileMode.Open);

            BinaryReader reader = new BinaryReader(fileStream);
            if (reader != null)
            {
                for (int h = 0; h < 4; h++)
                {
                    for (int s = 0; s < 8; s++)
                    {
                        names[s, h] = reader.Read().ToString();
                        scores[s, h] = reader.ReadInt32();
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to create Binary writer for saving HiScore!");
            }
        }
        catch (System.Exception e) 
        { 
        Debug.LogWarning(e.Message+"Failed to create or open fileStream for saving hiscores!");
        }
        
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
