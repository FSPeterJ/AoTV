using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveController : MonoBehaviour {
   
    [SerializeField] bool shockwaveActive = false;
    CapsuleCollider cCollider;
    float velosity = 2;
    float newScale = 0;
	// Use this for initialization
	void Start () {
        cCollider = GetComponent<CapsuleCollider>();
        cCollider.transform.localScale = new Vector3(newScale, 1, newScale);
    }
	
	// Update is called once per frame
	void Update () {
		if(shockwaveActive==true)
        {
            if (cCollider.transform.localScale.x <1)
            {
                newScale += velosity * Time.deltaTime;
                cCollider.transform.localScale = new Vector3(newScale, 1, newScale);  
            }
        }
	}
}
