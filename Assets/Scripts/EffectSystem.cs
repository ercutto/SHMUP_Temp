
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    public static EffectSystem instance=null;

    public GameObject craftExplotionPrefab = null;

    public ParticleSystem craftParticlesPrefab = null;
    public ParticleSystem craftDebrisParticlesPrefab = null;
    public ParticleSystem hitParticlesPrefab = null;
    public GameObject smallExplosion = null;
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
        Instantiate(craftParticlesPrefab,position,Quaternion.identity);
        Instantiate(craftDebrisParticlesPrefab,position,Quaternion.identity);
    }
   
    public void SpawnSparks(Vector3 pos)
    {
        Quaternion angle = Quaternion.Euler(0, 0, 45);
        Instantiate(hitParticlesPrefab,pos,angle);
    }

    public void SpawnLargeExplosion(Vector3 pos)
    {
        Instantiate(smallExplosion,pos,Quaternion.identity);
    }
    public void SpawnSmallExplosion(Vector3 pos)
    {
        Instantiate(smallExplosion, pos, Quaternion.identity);

    }
}
