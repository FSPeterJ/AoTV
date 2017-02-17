using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Lose : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        EventSystem.onPlayerLose += Lose;
    }

    void Lose()
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
    }
}
