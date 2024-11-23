using System;
using System.IO;

[Serializable]
public class Session
{
   public enum Hardness
    {
        Easy,
        Normal,
        Hard,
        Insane
    };

    public CraftData[] craftDatas= new CraftData[2];
    public Hardness hardness =Hardness.Normal;
    public int stage = 1;
    public bool practise = false;
    public bool arenaPractise = false;
    public bool stagePractise = false;

    //Cheats

    public bool infiniteLives = false;
    public bool infiniteContinues = false;
    public bool infiniteBombs =false;
    public bool invincible = false;
    public bool halfspeed =false;
    public bool doubleSpeedPractise = false;

    public void Save(BinaryWriter writer)
    {
        craftDatas[0].Save(writer);
        if(GameManager.Instance.twoPlayer)
            craftDatas[1].Save(writer);

        writer.Write((Byte)hardness);
        writer.Write(stage);
    }
    public void Load(BinaryReader reader)
    {
        craftDatas[0].Load(reader);
        if(GameManager.Instance.twoPlayer)
            craftDatas[1].Load(reader);

        hardness=(Hardness)reader.ReadByte();
        stage=reader.ReadInt32();
    
    }
}
