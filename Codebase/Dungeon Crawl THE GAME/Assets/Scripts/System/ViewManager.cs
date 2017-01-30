using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("NewGame");
    }
    public void Continue()
    {
        //SceneManager.LoadScene("Continue");
    }
    public void LoadInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }
    public void LoadHighscores()
    {
        //SceneManager.LoadScene("Highscores");
    }
    public void LoadOptions ()
    {
        SceneManager.LoadScene("Options");
    }
    public void Exit()
    {
        Application.Quit();
    }


    //SubMenus
    public void Return()
    {
        SceneManager.LoadScene("Main Menu");
    }


    //New Game
    public void StartGame()
    {
        SceneManager.LoadScene("Forest");
    }
}