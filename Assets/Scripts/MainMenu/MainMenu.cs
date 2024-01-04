using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject settingsMenu;
    public GameObject creditsScreen;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    float musicVolume = 20f;
    float sfxVolume = 20f;
    Resolution[] resolutions;

    List<Resolution> resolutionList = new List<Resolution>
    {
        new Resolution {width = 1920, height = 1080 },
        new Resolution {width = 1600, height = 900 },
        new Resolution {width = 1280, height = 720 },
    };

    void Start()
    {
        creditsScreen.SetActive(false);
        settingsMenu.SetActive(false);
        InitializeDropdown();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // 1 is You vs You gameplay
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {
        settingsMenu.SetActive(true);
        creditsScreen.SetActive(false);
    }

    public void Credits()
    {
        creditsScreen.SetActive(true);
    }

    public void BackToMain()
    {
        settingsMenu.SetActive(false);
        creditsScreen.SetActive(false);
    }

    public void SetMusicVolume()
    {
        float volume = musicVolumeSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        musicVolume = volume;
        Debug.Log(musicVolume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxVolumeSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        sfxVolume = volume;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("Fullscreen", Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Resolution"))
            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
        else resolutionDropdown.value = 0;

        if (PlayerPrefs.HasKey("Fullscreen"))
            Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen"));
        else Screen.fullScreen = true;

        if (PlayerPrefs.HasKey("MusicVolume"))
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        else
        {
            musicVolumeSlider.value = musicVolumeSlider.maxValue;
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        else
        {
            sfxVolumeSlider.value = sfxVolumeSlider.maxValue;
            SetSFXVolume();
        }
    }

    void InitializeDropdown()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = resolutionList.ToArray();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings();
    }
}
