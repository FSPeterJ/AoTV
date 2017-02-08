using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransfer : MonoBehaviour {

    public Scene nextLevel;
    public GameObject key1;
    public GameObject key2;
    public GameObject key3;
    public GameObject key4;

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
        if (other.tag == "Player" && key1.activeSelf && key2.activeSelf && key3.activeSelf && key4.activeSelf)
        {
            SceneManager.LoadScene(nextLevel.ToString());
            SceneManager.LoadScene("Swamp");
        }
    }
}
