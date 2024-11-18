using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    const int SAVE_VERSION = 1;
    public static SaveManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            Debug.Log("Trying to create more than one save manager!");
            Destroy(instance);
            return;
        }

        instance = this;
        
    }

    public void SaveGame(int slot)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter writer= new BinaryWriter(memStream);  

        writer.Write(SAVE_VERSION);
        writer.Write(GameManager.Instance.twoPlayer);

        GameManager.Instance.gameSession.Save(writer);
        GameManager.Instance.playerDatas[0].Save(writer);
        if(GameManager.Instance.twoPlayer)
            GameManager.Instance.playerDatas[1].Save(writer);

        string savePath = Application.persistentDataPath + "/slot" + slot + ".dat";
        Debug.Log("SaveGame,savePath="+savePath);

        FileStream fileStream = new FileStream(savePath, FileMode.OpenOrCreate);
        memStream.WriteTo(fileStream);
        fileStream.Close();

        writer.Close();
        memStream.Close();
    }

    public void LoadGame(int slot)
    {
        string loadPath= Application.persistentDataPath + "/slot" + slot + ".dat";

        MemoryStream memStream = new MemoryStream();
        FileStream fileStream=new FileStream(loadPath, FileMode.Open);
        if (fileStream != null)
        {
            BinaryReader reader= new BinaryReader(memStream);
            fileStream.CopyTo(memStream);
            memStream.Position = 0;

            int version =reader.ReadInt32();
            if (version == SAVE_VERSION)
            {
                GameManager.Instance.twoPlayer = reader.ReadBoolean();
                GameManager.Instance.gameSession.Load(reader);
                GameManager.Instance.playerDatas[0].Load(reader);
                if(GameManager.Instance.twoPlayer)
                    GameManager.Instance.playerDatas[1].Load(reader);

                reader.Close();
                fileStream.Close();

                GameManager.Instance.ResumeGameFromLoad();
            }
            else
            {
                Debug.LogError("SaveFile version is not correct!");
            }
        }

        memStream.Close ();

    }

    public void CopySaveToSlot(int slot)
    {
        Debug.Log(slot > 0);

        string LoadPath = Application.persistentDataPath + "/slot0.dat";
        string destPath = Application.persistentDataPath + "/slot" + slot + ".dat";

        File.Copy(LoadPath, destPath);
    
    }

    public bool LoadExist(int slot) 
    {
        string loadPath = Application.persistentDataPath + "/slot" + slot + ".dat";

        if(File.Exists(loadPath))
            return true;
        return false;

    }


}
