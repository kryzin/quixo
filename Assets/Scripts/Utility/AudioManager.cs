using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource moveSound;
    public AudioSource selectSound;
    public AudioSource pauseSound;
    public AudioSource winSound;
    public AudioSource loseSound;

    public void WinGameOver()
    {
        winSound.Play();
    }

    public void LoseGameOver()
    {
        loseSound.Play();
    }

    public void PauseSound()
    {
        pauseSound.Play();
    }

    public void MoveSound()
    {
        moveSound.Play();
    }

    public void SelectSound()
    {
        selectSound.Play();
    }
}
