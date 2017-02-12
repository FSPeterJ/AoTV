using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Interact : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        EventSystem.onUI_Interact += En;
    }
    private void OnDisable()
    {
        EventSystem.onUI_Interact -= En;
    }

    void En(bool stat)
    {
        transform.GetChild(0).gameObject.SetActive(stat);
    }
}
