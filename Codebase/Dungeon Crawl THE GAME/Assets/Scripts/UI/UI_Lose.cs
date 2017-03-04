using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Lose : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnEnable()
    {
        EventSystem.onPlayerLose += Lose;
    }

    private void OnDisable()
    {
        EventSystem.onPlayerLose -= Lose;
    }

    private void Lose()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Quitter()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Continue()
    {
        EventSystem.Cont();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}