
using UnityEngine;

public class Beam : MonoBehaviour
{
    public  LineRenderer lineRenderer = null;
    public float beamWidth = 20f;
    public Craft craft = null;
    private int layerMask;
    public byte playerIndex = 2;
    public GameObject beamFlash;
    public GameObject[] beamHits=new GameObject[5];
    const int MINIMUMCHARGE = 10;
    public AudioSource audioSource = null;
    void Start()
    {

        layerMask = ~LayerMask.GetMask("Player") & ~LayerMask.GetMask("PlayerBullets");
    }

    public void Fire()
    {
        CraftData craftData= GameManager.Instance.gameSession.craftDatas[craft.playerIndex];
        if (!craftData.beamFiring)
        {
            if (craftData.beamCharge > craftData.beamTimer)
            {
                Debug.Log(craftData.beamCharge);
                craftData.beamFiring = true;
                craftData.beamTimer = craftData.beamCharge;
                craftData.beamCharge = 0;
                
                UpdateBeam();

                gameObject.SetActive(true);
                beamFlash.SetActive(true);
                if (audioSource)
                    audioSource.Play();
            }
            else
            {
                UpdateBeam();
            }
        }
    }
    private void FixedUpdate()
    {
        CraftData craftData = GameManager.Instance.gameSession.craftDatas[craft.playerIndex];
        if (craftData.beamFiring)
        {
            
            UpdateBeam();
        }
            
    }

    void HideHits()
    {
        for (int beamhitseffect = 0; beamhitseffect < 5; beamhitseffect++)
        { beamHits[beamhitseffect].SetActive(false); }
    }
    void UpdateBeam()
    {
        CraftData craftData = GameManager.Instance.gameSession.craftDatas[craft.playerIndex];

        if (craftData.beamTimer > 0)
        {
            Debug.Log(craftData.beamTimer);
            craftData.beamTimer--;
        }
        if(craftData.beamTimer == 0)//beam finished
        {
            if (audioSource)
                audioSource.Stop();

            craftData.beamFiring = false;
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
        Collider2D[] hits= new Collider2D[maxColliders];
        float middleY=(craft.transform.position.y+ topY) *0.5f;
        Vector2 halfsize = new Vector2(beamWidth * 0.5f, (topY - craft.transform.position.y) * 0.5f);
        Vector3 center = new Vector3(craft.transform.position.x, middleY, 0);
        int noOfHits = Physics2D.OverlapBoxNonAlloc(center, halfsize,0, hits, layerMask);
        float lowest = topY;
        Shootable lowestShootable = null;
        Collider2D lowestCollider= null;
        const int MAXRAYHITS = 10;
        if(noOfHits > 0)
        {
            //find lowest hit
            for(int _hits = 0; _hits < noOfHits; _hits++)
            {
                Shootable shootable = hits[_hits].GetComponent<Shootable>();
                if (shootable && shootable.DamageByBeams)
                {
                    RaycastHit2D[] hitInfo=new RaycastHit2D[MAXRAYHITS];
                    Vector2 ray = Vector3.up;
                    float height = topY - craft.transform.position.y;    
                    if (hits[_hits].Raycast(ray,hitInfo, height)>0)
                    {

                        if (hitInfo[0].point.y < lowest)
                        {
                            lowest = hitInfo[0].point.y;
                            lowestShootable = hits[_hits].GetComponent<Shootable>();
                            lowestCollider = hits[_hits];


                        }

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
                    RaycastHit2D[] hitInfo=new RaycastHit2D[MAXRAYHITS];
                    Vector2 ray = Vector3.up;
                    if(lowestCollider.Raycast(ray, hitInfo, 360) > 0)
                    {
                        Vector3 pos =hitInfo[0].point;
                        pos.x += Random.Range(-3f, 3f);
                        pos.y += Random.Range(-3f, 3f);
                        beamHits[i].transform.position = pos;
                        beamHits[i].SetActive(true);
                        lowestShootable.TakeDamage(craftData.beamPower+1,playerIndex);
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
