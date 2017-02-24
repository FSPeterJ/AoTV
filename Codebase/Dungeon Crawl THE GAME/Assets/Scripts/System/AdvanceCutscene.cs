using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AdvanceCutscene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.E) && SceneManager.GetActiveScene().name != "Closing Cutscene")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (Input.GetKeyDown(KeyCode.E) && SceneManager.GetActiveScene().name == "Closing Cutscene")
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
