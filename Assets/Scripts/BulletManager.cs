using System;

using Unity.Collections;
using Unity.Jobs;

using UnityEngine;
using UnityEngine.Jobs;

public class BulletManager : MonoBehaviour
{
    public Bullet[] bulletPrefabs = null;
    // Start is called before the first frame update
    public enum BulletType
    {
        Bullet1_Size1,
        Bullet1_Size2,
        Bullet1_Size3,
        Bullet1_Size4,
        Bullet1_Size5,
        Bullet2_Size1,
        Bullet2_Size2,
        Bullet2_Size3,
        Bullet2_Size4,
        Bullet3_Size1,
        Bullet3_Size2,
        Bullet3_Size3,
        Bullet3_Size4,
        Bullet4_Size1,
        Bullet4_Size2,
        Bullet4_Size3,
        Bullet4_Size4,
        Bullet6_Size1,
        Bullet6_Size2,
        Bullet6_Size3,
        Bullet6_Size4,
        Bullet7_Size1,
        Bullet7_Size2,
        Bullet7_Size3,
        Bullet8_Size1,
        Bullet8_Size2,
        Bullet8_Size3,
        Bullet8_Size4,
        Bullet9_Size1,
        Bullet9_Size2,
        Bullet9_Size3,
        Bullet9_Size4,
        Bullet10_Size1,
        Bullet10_Size2,
        Bullet10_Size3,
        Bullet11_Size1,
        Bullet11_Size2,
        Bullet11_Size3,
        Bullet11_Size4,
        Bullet12_Size1,
        Bullet12_Size2,
        Bullet12_Size3,
        Bullet12_Size4,





        MAX_TYPES

    }
    const int MAX_BULLET_PER_TYPE = 500;
    const int MAX_BULLET_COUNT = MAX_BULLET_PER_TYPE *(int) BulletType.MAX_TYPES;
    private Bullet[] bullets= new Bullet[MAX_BULLET_COUNT];
    private TransformAccessArray bulletTransform;

    ProcessBulletJob jobProcessor;
    private NativeArray<BulletData> bulletData;
    void Start()
    {
        bulletData = new NativeArray<BulletData>(MAX_BULLET_COUNT, Allocator.Persistent);
        bulletTransform=new TransformAccessArray(MAX_BULLET_COUNT);
        int index = 0;
        for (int bulletType= (int)BulletType.Bullet1_Size1;bulletType < (int)BulletType.MAX_TYPES; bulletType++){
            for(int b = 0; b < MAX_BULLET_PER_TYPE; b++)
            {
                Bullet newBullet = Instantiate(bulletPrefabs[bulletType]).GetComponent<Bullet>();
                newBullet.index = index;
                newBullet.gameObject.SetActive(false);
                newBullet.transform.SetParent(transform);
                bullets[index]= newBullet;
                bulletTransform.Add(bullets[index].transform);
                index++;
            }
        }

        jobProcessor = new ProcessBulletJob { bullets = bulletData };
    }

    private void OnDestroy()
    {
        bulletData.Dispose();
        bulletTransform.Dispose();
    }

    private int NextFreeBulletIndex(BulletType type)
    {
        int startIndex = (int)type * MAX_BULLET_PER_TYPE;
        for (int b = 0; b < MAX_BULLET_PER_TYPE; b++)
        {
            if (!bulletData[startIndex+b].active)
                return startIndex+b;
        }
        return -1;
    }
    public Bullet SpawnBullet(BulletType type,float x, float y,float dX,float dY,float angle,float dAngle,bool hooming)
    {
        int bulletIndex=NextFreeBulletIndex(type);
        if (bulletIndex > -1)
        {
            Bullet result = bullets[bulletIndex];
            result.gameObject.SetActive(true);
            bulletData[bulletIndex] = new BulletData(x,y,dX,dY,angle,dAngle,(int)type,true,hooming);
            bullets[bulletIndex].gameObject.transform.position=new Vector3(x,y,0);

            return result;
        }
        return null;
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance && GameManager.Instance.playerOneCraft)
            jobProcessor.player1Position = GameManager.Instance.playerOneCraft.transform.position;
        else
            jobProcessor.player1Position = new Vector2(-9999, -9999);

        if (GameManager.Instance && GameManager.Instance.progressWindow)
            jobProcessor.progressY = GameManager.Instance.progressWindow.transform.position.y;
        else
            jobProcessor.progressY = 0;

        ProcessBullets();

        for(int b=0; b<MAX_BULLET_COUNT; b++)
        {
            if (!bulletData[b].active)
                bullets[b].gameObject.SetActive(false);
        }
    }

    private void ProcessBullets()
    {
        JobHandle handler=  jobProcessor.Schedule(bulletTransform);
        handler.Complete();
    }
    public struct ProcessBulletJob : IJobParallelForTransform
    {
        public NativeArray<BulletData> bullets;

        public Vector2 player1Position;

        public float progressY;
        public void Execute(int index, TransformAccess transform)
        {
            bool active = bullets[index].active;
            if (!active) return;

            float dX= bullets[index].dX;
            float dY= bullets[index].dY;
            float x = bullets[index].positionX;
            float y = bullets[index].positionY;
            float angle = bullets[index].angle;
            float dAngle = bullets[index].dAngle;
            int type= bullets[index].type;
            bool hooming = bullets[index].hooming;
            
            //hooming
            if(player1Position.x< -1000)
                active = false;
            else if(hooming)
            {
                Vector2 velocity =new Vector2(dX, dY);
                float speed =velocity.magnitude;
                Vector2 toPlayer=new Vector2(player1Position.x-x, player1Position.y-y);
                Vector2 newVelocity = Vector2.Lerp(velocity.normalized, toPlayer.normalized, 0.05f).normalized;
                newVelocity *= speed;
                dX = newVelocity.x;
                dY = newVelocity.y;
            }

            x = x + dX;
            y = y + dY;
            
            if (x < -320) active = false;
            if (x > 320) active = false;
            if (y-progressY < -260) active = false;
            if (y-progressY > 260) active = false;

            bullets[index] = new BulletData(x,y,dX,dY,angle,dAngle,type,active,hooming);


            if (active)
            {
                Vector3 newPosition = new Vector3(x, y, 0);
                transform.position = newPosition;
                //facing rotation
                transform.rotation=Quaternion.LookRotation(Vector3.forward, new Vector3(dX,dY,0));
            }
        }
    }

    public void DeActivateBullet(int index)
    {
        bullets[index].gameObject.SetActive(false);
        float x = bulletData[index].positionX;
        float y = bulletData[index].positionY;
        float dX = bulletData[index].dX;
        float dY= bulletData[index].dY;
        float angle = bulletData[index].angle;
        float dAngle = bulletData[index].dAngle;
        int type= bulletData[index].type;
        bool hooming = bulletData[index].hooming;

        bulletData[index] = new BulletData(x,y,dX,dY,angle,dAngle,type,false, hooming);
    }
}
