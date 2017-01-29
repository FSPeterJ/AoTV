using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransfer : MonoBehaviour {

    public Scene nextLevel;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //SceneManager.LoadScene(nextLevel.ToString());
            SceneManager.LoadScene("Swamp");
        }
    }
}
