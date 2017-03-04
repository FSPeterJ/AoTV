using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenus : MonoBehaviour
{
    [SerializeField]
    private GameObject pause, options, saveCheck;

    private GameObject _cs;

    private GameObject currentScreen
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

    private bool hasSaved;

    private void OnEnable()
    {
        EventSystem.onGamePausedToggle += PausedToggle;
        EventSystem.onUI_Back += Return;
    }

    //unsubscribe from player movement
    private void OnDisable()
    {
        EventSystem.onGamePausedToggle -= PausedToggle;
        EventSystem.onUI_Back -= Return;
    }

    private void Start()
    {
        pause = transform.GetChild(0).gameObject;
        saveCheck = transform.GetChild(1).gameObject;
        options = transform.GetChild(2).gameObject;
        //?
        hasSaved = false;
    }

    private void Update()
    {
    }

    //Pause menu should control time since pausing the game is for the menu
    private void PausedToggle()
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
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            currentScreen = saveCheck;
        }
    }

    public void exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void Cancel()
    {
        currentScreen = pause;
    }

    public void ReloadCheckpoint()
    {
        currentScreen = null;
        EventSystem.Player_ReloadCheckpoint();
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}