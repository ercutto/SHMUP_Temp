using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptionsMenu : Menu
{
    public static AudioOptionsMenu instance = null;
    public Slider masterSlider = null;
    public Slider FxSlider = null;
    public Slider musicSlider = null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than AudioOptions Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;

        float volume = PlayerPrefs.GetFloat("MasterVolume");
        masterSlider.value = volume;
        volume = PlayerPrefs.GetFloat("MusicVolume");
        musicSlider.value = volume;
        volume = PlayerPrefs.GetFloat("EffectsVolume");
        FxSlider.value = volume;
    }
    public void OnBackButton()
    {
        TurnOff(true); //Simdi bu menuyu kapatiyoruz ve bir oncekine donuyoruz
    }

    public void UpdateMasterVolume(float value)
    {
        float volume =Mathf.Clamp(value,0.0001f,1);
        AudioManager.instance.mixer.SetFloat("MasterVolume",Mathf.Log10(volume)*20);

        PlayerPrefs.SetFloat("MasterVolume",value);
        PlayerPrefs.Save();
    }
    public void UpdateSFXVolume(float value)
    {
        float volume = Mathf.Clamp(value, 0.0001f, 1);
        AudioManager.instance.mixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("EffectsVolume", value);
        PlayerPrefs.Save();

    }
    public void UpdateMusicVolume(float value)
    {
        float volume = Mathf.Clamp(value, 0.0001f, 1);
        AudioManager.instance.mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

    }
}
