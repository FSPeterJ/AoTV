using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour {
    
    //public Button[] menuButtons;
    void Start()
    {
        
    }

    void Update()
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
        SceneManager.LoadScene("Opening Cutscene", LoadSceneMode.Single);
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
