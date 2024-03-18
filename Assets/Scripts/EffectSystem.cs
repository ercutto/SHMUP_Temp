using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    public static EffectSystem instance=null;

    public GameObject craftExplotionPrefab = null;
    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            Debug.Log("Trying to create more than one EffectSystem! ");
        }
     
        instance = this;
        
    }

    public void CraftExplosion(Vector3 position)
    {
        Instantiate(craftExplotionPrefab,position,Quaternion.identity);
    }
   
}
