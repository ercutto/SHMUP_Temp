using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedChar : MonoBehaviour
{
    public Sprite[] charSprite;

    public SpriteRenderer spriteRenderer;
    public int digit = 0;
    private int frame = 0;
    public int noOfCharacters;
    public int noOfFrames;
    public float FPS = 10f;
    private float timer;
    public int offset = 0;
    void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer != null);
        timer = 1f / FPS;
        UpdateSprite(0);
    }
    private void UpdateSprite(int newFrame)
    {
        int loopedFrame =(newFrame+offset) % noOfFrames;
        int spriteIndex=digit +(loopedFrame * noOfCharacters);
        spriteRenderer.sprite = charSprite[spriteIndex];
    }

    // Update is called once per frame
    void Update()
    {
        timer-=Time.deltaTime;
        if (timer <= 0)
        {
            timer=1f/FPS;
            frame++;
            if(frame >= noOfFrames) frame = 0;

            UpdateSprite(frame);
            
        }
    }
}
