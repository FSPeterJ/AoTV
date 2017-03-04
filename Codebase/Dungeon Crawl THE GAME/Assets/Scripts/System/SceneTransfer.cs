using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour
{
    private bool HasKey;

    private void OnEnable()
    {
        EventSystem.onUI_KeyCount += KeyChange;
    }

    private void OnDisable()
    {
        EventSystem.onUI_KeyCount -= KeyChange;
    }

    private void KeyChange(int keys)
    {
        if (keys > 3)
        {
            HasKey = true;
        }
        else
        {
            HasKey = false;
        }
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && HasKey)
        {
            //SceneManager.LoadScene(nextLevel.ToString());

            PlayerPrefs.SetInt("lives", other.gameObject.GetComponent<Player>().GetLives());
            PlayerPrefs.SetInt("score", other.gameObject.GetComponent<Player>().GetScore());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}