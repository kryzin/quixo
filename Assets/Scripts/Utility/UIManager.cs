using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public TMP_Text gameClock;
    public GameManager gameManager;
    public GameObject gameOverPopUp;
    public TMP_Text winner;
    public TMP_Text winMessage;
    public Board board;
    public Button PauseButton;
    public GameObject pausePopUp;
    public AudioManager audioManager;
    public GameObject tutorial;
    public Button HelpButton;
    public GameObject settingsPopUp;
    public Button SettingsButton;

    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public AudioMixer audioMixer;

    private float timer = 0.0f;
    private bool isTimer = false;

    float musicVolume = 20f;
    float sfxVolume = 20f;

    void Start()
    {
        tutorial.SetActive(false);
        gameOverPopUp.SetActive(false);
        settingsPopUp.SetActive(false);
        pausePopUp.SetActive(false);
        StartClock();
        LoadSettings();
    }

    void Update()
    {
        if (isTimer)
        {
            timer += Time.deltaTime;
            DisplayClock();
        }
    }

    public void SettingsMenu()
    {
        gameManager.isPaused = !gameManager.isPaused;
        isTimer = !isTimer;
        HandleSettingsPopUp(!isTimer);
        // pause pop up
        // change text on button to play/pause
    }

    public void HandleSettingsPopUp(bool active)
    {
        audioManager.PauseSound();
        settingsPopUp.SetActive(active);
    }

    public void DisablePause()
    {
        PauseButton.interactable = false;
        HelpButton.interactable = false;
        SettingsButton.interactable = false;
    }

    public void EnablePause()
    {
        PauseButton.interactable = true;
        HelpButton.interactable = true;
        SettingsButton.interactable = true;
    }

    void DisplayWinner()
    {
        winner.text = "WIN FOR " + gameManager.winner.ToString();
    }

    void DisplayEndMessage(int i)
    {
        if (i == 1)
        {
            winMessage.text = "5 in a row!";
        }
        else
        {
            if (board.lostWin) { winMessage.text = "Next time try to not complete your oponent's line!"; }
            else { winMessage.text = "Wow! Double winner!"; }
        }
    }

    void DisplayClock()
    {
        int minutes = Mathf.FloorToInt(timer / 60.0f);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);

        gameClock.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartClock()
    {
        isTimer = true;
    }

    public void StopClock()
    {
        isTimer = false;
    }

    public void ShowEndPopUp(int i)
    {
        if (gameManager.winner == gameManager.playerSymbol)
        {
            audioManager.WinGameOver();
        }
        else audioManager.LoseGameOver();

        gameOverPopUp.SetActive(true);
        DisplayWinner();
        DisplayEndMessage(i);
    }

    public void HandlePausePopUp(bool active)
    {
        audioManager.PauseSound();
        pausePopUp.SetActive(active);
    }

    public void ResetGame() // use for new round also
    {
        timer = 0.0f;
        SceneManager.LoadScene(1);
    }

    public void PauseGame()
    {
        gameManager.isPaused = !gameManager.isPaused;
        isTimer = !isTimer;
        HandlePausePopUp(!isTimer);
        // pause pop up
        // change text on button to play/pause
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void HandleHelpPopUp(bool active)
    {
        audioManager.PauseSound();
        settingsPopUp.SetActive(active);
    }

    public void HelpMenu()
    {
        gameManager.isPaused = !gameManager.isPaused;
        isTimer = !isTimer;
        HandleHelpPopUp(!isTimer);
    }

    public void GoToTutorial()
    {
        SceneManager.LoadScene(2);
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

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        else musicVolumeSlider.value = 0.5f;

        if (PlayerPrefs.HasKey("SFXVolume"))
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        else sfxVolumeSlider.value = 0.5f;

        SetMusicVolume();
        SetSFXVolume();
    }
}
