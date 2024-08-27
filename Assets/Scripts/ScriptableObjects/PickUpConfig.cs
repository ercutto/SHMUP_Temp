using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="PickupConfig",menuName ="SHMUP/PickupConfig")]
public class PickUpConfig : ScriptableObject
{
    public PickUp.PickupType type;

    public int powerLevel = 1;
    public int bombPower = 1;
    public int medalvalue = 100;
    public int fallSpeed = 0;
    public int coinValue = 1;
    public int medalLevel = 1;
    public int surplusValue = 100;
}
