using UnityEngine;

public class menu : MonoBehaviour
{
    private bool pausegame = false;

    public CanvasGroup PauseMenu;

    // Use this for initialization
    private void Start()
    {
        //PauseMenu = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausegame = !pausegame;
            if (pausegame)
            {
                PauseMenu.alpha = 1;
                PauseMenu.interactable = true;

                Time.timeScale = 0;
            }
            else
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        pausegame = false;
        PauseMenu.alpha = 0;
        PauseMenu.interactable = false;
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }
}