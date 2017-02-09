using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenus : MonoBehaviour
{
    [SerializeField]
    GameObject pause, options, saveCheck;

    GameObject _cs;
    GameObject currentScreen
    {

        get
        {
            return _cs;
        }
        set
        {
            if (value != _cs)
            {
                if (_cs != null)
                    _cs.SetActive(false);
                _cs = value;
                if (_cs != null)
                    _cs.SetActive(true);
            }
        }
    }



    bool hasSaved;

    void OnEnable()
    {
        EventSystem.onGamePausedToggle += PausedToggle;
        EventSystem.onUI_Back += Return;
    }
    //unsubscribe from player movement
    void OnDisable()
    {
        EventSystem.onGamePausedToggle -= PausedToggle;
        EventSystem.onUI_Back -= Return;
    }

    void Start()
    {
        pause = transform.GetChild(0).gameObject;
        saveCheck = transform.GetChild(1).gameObject;
        options = transform.GetChild(2).gameObject;
        //?
        hasSaved = false;
    }

    void Update()
    {

    }


    //Pause menu should control time since pausing the game is for the menu
    void PausedToggle()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            currentScreen = null;
        }
        else
        {
            Time.timeScale = 0;
            currentScreen = pause;
        }
        //set timescale back to 1 when pause menu is left code already handles p and escape return to game
    }

    public void Resume()
    {
        EventSystem.GamePausedToggle();

    }

    public void Options()
    {
        currentScreen = options;
    }

    public void Return()
    {
        if (currentScreen != pause)
            currentScreen = pause;
        else
        {
            EventSystem.GamePausedToggle();
        }
    }

    public void MainMenu(bool quit = false)
    {
        if (hasSaved || quit)
            SceneManager.LoadScene("MainMenu");
        else
        {
            currentScreen = saveCheck;
        }
    }

    public void exit()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Cancel()
    {
        currentScreen = pause;
    }



}
