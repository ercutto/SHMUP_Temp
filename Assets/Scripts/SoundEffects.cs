
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioClip[] sounds;

    public void Play()
   {
        if (AudioManager.instance && sounds.Length > 0)
        {
            int r = Random.Range(0, sounds.Length);
            AudioManager.instance.PlaySounEffects(sounds[r]);
        }
        //AudioManager.instance.PlaySounEffects(sounds[0]);
    }
}
