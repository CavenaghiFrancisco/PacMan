using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] List<AudioSource> sounds;
    [SerializeField] List<AudioSource> music;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    private float soundVolume;
    private float musicVolume;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetFloat("Music", 0.5f);
        }
        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetFloat("Sound", 0.5f);
        }
        musicVolume = PlayerPrefs.GetFloat("Music");
        soundVolume = PlayerPrefs.GetFloat("Sound");
    }

    private void Update()
    {
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        foreach (AudioSource sound in sounds)
        {
            sound.volume = PlayerPrefs.GetFloat("Sound");
        }
        foreach (AudioSource music in music)
        {
            music.volume = PlayerPrefs.GetFloat("Music");
        }
        if (soundSlider)
            soundSlider.value = PlayerPrefs.GetFloat("Sound");
        if (musicSlider)
            musicSlider.value = PlayerPrefs.GetFloat("Music");
    }

    public void ChangeSoundsVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("Sound", newVolume);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ChangeMusicVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("Music", newVolume);
        EventSystem.current.SetSelectedGameObject(null);
    }
}