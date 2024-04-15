using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting;
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
        Bullet4_Size1,
     
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
    public Bullet SpawnBullet(BulletType type,float x, float y,float dX,float dY,float angle)
    {
        int bulletIndex=NextFreeBulletIndex(type);
        if (bulletIndex > -1)
        {
            Bullet result = bullets[bulletIndex];
            result.gameObject.SetActive(true);
            bulletData[bulletIndex] = new BulletData(x,y,dX,dY,angle,(int)type,true);
            bullets[bulletIndex].gameObject.transform.position=new Vector3(x,y,0);

            return result;
        }
        return null;
    }
    private void FixedUpdate()
    {
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
        public void Execute(int index, TransformAccess transform)
        {
            bool active = bullets[index].active;
            if (!active) return;

            float dX= bullets[index].dX;
            float dY= bullets[index].dY;
            float x = bullets[index].positionX;
            float y = bullets[index].positionY;
            float angle = bullets[index].angle;
            int type= bullets[index].type;
            

            x = x + dX;
            y = y + dY;
            
            if (x < -320) active = false;
            if (x > 320) active = false;
            if (y < -180) active = false;
            if (y > 180) active = false;

            bullets[index] = new BulletData(x,y,dX,dY,angle,type,active);


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
        int type= bulletData[index].type;

        bulletData[index] = new BulletData(x,y,dX,dY,angle,type,false);
    }
}
