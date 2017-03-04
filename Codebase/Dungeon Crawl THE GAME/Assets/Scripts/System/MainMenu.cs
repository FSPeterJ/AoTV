using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //public Button[] menuButtons;
    private void Start()
    {
        PlayerPrefs.SetInt("lives", 3);
        PlayerPrefs.SetInt("score", 0);
    }

    private void Update()
    {
        //if (Input.GetKey(KeyCode.Return))
        //{
        //    for (int i = 0; i < menuButtons.Length; i++)
        //    {
        //    }
        //}
    }

    public void MainMenuLoader()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void NewGameLoader()
    {
        SceneManager.LoadScene("Opening Scene", LoadSceneMode.Single);
    }

    public void OptionsLoader()
    {
        SceneManager.LoadScene("Options", LoadSceneMode.Single);
    }

    public void CreditsLoader()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    public void exitLoader()
    {
        Application.Quit();
    }

    public void FullScreenToggle()
    {
        if (Screen.fullScreen == true)
            Screen.fullScreen = false;
        else
            Screen.fullScreen = true;
    }
}