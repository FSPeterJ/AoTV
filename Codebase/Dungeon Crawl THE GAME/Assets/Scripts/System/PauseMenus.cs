using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenus : MonoBehaviour
{
    [SerializeField]
    GameObject options, saveCheck;

    GameObject currentScreen;
    bool hasSaved;

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void Options()
    {
        gameObject.SetActive(false);
        options.SetActive(true);
        currentScreen = options;
    }

    public void Return()
    {
        if (currentScreen != gameObject)
            gameObject.SetActive(true);
        else
        {
            //unpause
        }

        currentScreen.SetActive(false);
    }

    public void MainMenu()
    {
        if (hasSaved)
            SceneManager.LoadScene("MainMenu");
        else
        {
            gameObject.SetActive(false);
            saveCheck.SetActive(true);
        }
    }

    public void exit()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Cancel()
    {
        saveCheck.SetActive(false);
        gameObject.SetActive(true);
    }

    void Start()
    {
        currentScreen = gameObject;
        hasSaved = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
            if (currentScreen != gameObject)
                currentScreen.SetActive(false);
    }
    
}
