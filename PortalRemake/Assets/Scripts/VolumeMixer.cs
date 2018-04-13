using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeMixer : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;
    public AudioMixer masterMixer;

    private void Start()
    {
        if (sfxSlider != null)
        {
            if (PlayerPrefs.HasKey("SfxVolume") == true)
            {
                sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
            }
            else
            {
                PlayerPrefs.SetFloat("SfxVolume", -30f);
                sfxSlider.value = -30f;
            }
        }
        else
        {
            Debug.LogWarning("SfxSlider not present in this scene.");
        }

        if (musicSlider != null)
        {
            if (PlayerPrefs.HasKey("MusicVolume") == true)
            {
                musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            }
            else
            {
                PlayerPrefs.SetFloat("MusicVolume", -30f);
                musicSlider.value = -30f;
            }
        }
        else
        {
            Debug.LogWarning("MusicSlider not present in this scene.");
        }
    }

    public void SetSfxVolume(float volume)
    {
        masterMixer.SetFloat("SfxVolume", volume);
        PlayerPrefs.SetFloat("SfxVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);

    }
}
