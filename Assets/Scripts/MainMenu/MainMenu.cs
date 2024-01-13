using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject settingsMenu;
    public GameObject creditsScreen;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public GameManager gameManager;

    public int wasTutorial;

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
        if (PlayerPrefs.HasKey("Tutorial"))
            wasTutorial = PlayerPrefs.GetInt("Tutorial");
        else wasTutorial = 0;
        if (wasTutorial == 0) SceneManager.LoadScene(2);
        else SceneManager.LoadScene(1);
    }

    public void Tutorial()
    {
        SceneManager.LoadScene(2);
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
        float mappedVolume = Mathf.Lerp(-80f, 20f, volume);
        audioMixer.SetFloat("Music", mappedVolume);
        musicVolume = volume;
    }

    public void SetSFXVolume()
    {
        float volume = sfxVolumeSlider.value;
        float mappedVolume = Mathf.Lerp(-80f, 20f, volume);
        audioMixer.SetFloat("SFX", mappedVolume);
        sfxVolume = volume;
    }

    public void SetFullscreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        Debug.Log(fullscreenToggle.isOn + " " + Screen.fullScreen);
    }

    public void SetResolution()
    {
        Resolution resolution = resolutions[resolutionDropdown.value];
        Debug.Log(resolutionDropdown.value + " " + resolution.ToString());
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
        else musicVolumeSlider.value = musicVolumeSlider.maxValue;

        if (PlayerPrefs.HasKey("SFXVolume"))
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        else sfxVolumeSlider.value = sfxVolumeSlider.maxValue;

        SetMusicVolume();
        SetSFXVolume();
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
