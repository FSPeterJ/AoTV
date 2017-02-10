using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransfer : MonoBehaviour {

    public Scene nextLevel;
    bool HasKey;

    private void OnEnable()
    {
        EventSystem.onUI_KeyCount += KeyChange;
    }

    private void OnDisable()
    {
        EventSystem.onUI_KeyCount -= KeyChange;
    }

    void KeyChange(int keys)
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
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && HasKey)
        {
            SceneManager.LoadScene(nextLevel.ToString());
            SceneManager.LoadScene("Swamp");
        }
    }
}
