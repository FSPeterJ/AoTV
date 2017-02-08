using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void MainMenuLoader()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

   public void NewGameLoader()
    {
        SceneManager.LoadScene("Graveyard", LoadSceneMode.Single);
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
}
