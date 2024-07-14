using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public  LineRenderer lineRenderer = null;
    public float beamWidth = 20f;
    public Craft craft = null;
    private int layerMask;
    public GameObject beamFlash;
    public GameObject[] beamHits=new GameObject[5];
    void Start()
    {

        layerMask = ~LayerMask.GetMask("Player") & ~LayerMask.GetMask("PlayerBullets");
    }

    public void Fire()
    {
        if (!craft.craftData.beamFiring)
        {
            craft.craftData.beamFiring=true;
            craft.craftData.beamTimer=craft.craftData.beamCharge;
            UpdateBeam();
            
            gameObject.SetActive(true);
            beamFlash.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        if (craft.craftData.beamFiring)
            UpdateBeam();
    }

    void HideHits()
    {
        for (int beamhitseffect = 0; beamhitseffect < 5; beamhitseffect++)
        { beamHits[beamhitseffect].SetActive(false); }
    }
    void UpdateBeam()
    {
        craft.craftData.beamTimer--;
        if(craft.craftData.beamTimer == 0)
        {
            craft.craftData.beamFiring = false;
            HideHits();
            gameObject.SetActive(false);
            beamFlash.SetActive(false);
            return;

            
        }

        float scale = beamWidth / 30f;
        beamFlash.transform.localScale = new Vector3(scale, scale, 1);

        float topY = 180;
        if (GameManager.Instance && GameManager.Instance.progressWindow)
            topY += GameManager.Instance.progressWindow.transform.position.y;

        int maxColliders = 20;
        Collider[] hits= new Collider[maxColliders];
        float middleY=(craft.transform.position.y+180)*0.5f;
        Vector2 halfsize = new Vector2(beamWidth * 0.5f, (180 - craft.transform.position.y) * 0.5f);
        Vector3 center = new Vector3(craft.transform.position.x, middleY, 0);
        int noOfHits = Physics.OverlapBoxNonAlloc(center, halfsize, hits, Quaternion.identity, layerMask);
        float lowest = topY;
        Shootable lowestShootable = null;
        Collider lowestCollider= null;
        if(noOfHits > 0)
        {
            //find lowest hit
            for(int _hits = 0; _hits < noOfHits; _hits++)
            {
                RaycastHit hitInfo;
                Ray ray = new Ray(craft.transform.position, Vector3.up);
                float height = 180 - craft.transform.position.y;
                if (hits[_hits].Raycast(ray, out hitInfo, height))
                {
                    
                    if (hitInfo.point.y < lowest)
                    {
                        lowest = hitInfo.point.y;
                        lowestShootable = hits[_hits].GetComponent<Shootable>();
                        lowestCollider = hits[_hits].GetComponent<Collider>();

                        
                    }

                }
            }

            //find hits on collider
            if (lowestShootable != null)
            {
                Vector3 start=craft.transform.position;
                start.x -= (beamWidth / 5);
                // fire 5 rays to find each hits
                for (int i = 0; i < 5; i++)
                {
                    RaycastHit hitInfo;
                    Ray ray = new Ray(start, Vector3.up);
                    if(lowestCollider.Raycast(ray,out hitInfo, 360))
                    {
                        Vector3 pos =hitInfo.point;
                        pos.x += Random.Range(-3f, 3f);
                        pos.y += Random.Range(-3f, 3f);
                        beamHits[i].transform.position = pos;
                        beamHits[i].SetActive(true);
                        lowestShootable.TakeDamage(craft.craftData.beamPower+1);
                    }
                    else
                    {
                        beamHits[i].SetActive(false);
                    }

                    start.x += (beamWidth / 5);

                }
            }
            else
            {
                HideHits();
            }
        }
        else
        {
            HideHits();
        }

        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;

        lineRenderer.SetPosition(0,transform.position);
        Vector3 top = transform.position;
        top.y = lowest;
        lineRenderer.SetPosition(1,top);

    }
}
