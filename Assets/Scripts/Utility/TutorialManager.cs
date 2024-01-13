using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public int wasTutorial = 0;
    public GameObject initialPopUp;
    public GameObject tutorial1;
    public GameObject tutorial2;
    public GameObject tutorial3;

    // Start is called before the first frame update
    void Start()
    {
        SaveTutorial();
        tutorial1.SetActive(false);
        tutorial2.SetActive(false);
        tutorial3.SetActive(false);

        initialPopUp.SetActive(true);
    }

    public void SaveTutorial()
    {
        wasTutorial = 1;
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    public void SkipTutorial()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayTutorial()
    {
        initialPopUp.SetActive(false);
        tutorial2.SetActive(false);
        tutorial1.SetActive(true);
    }

    public void Slide2()
    {
        tutorial1.SetActive(false);
        tutorial2.SetActive(true);
        tutorial3.SetActive(false);
    }

    public void Slide3()
    {
        tutorial2.SetActive(false);
        tutorial3.SetActive(true);
    }
}
