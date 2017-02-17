using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        transform.gameObject.SetActive(true);
    }
}
