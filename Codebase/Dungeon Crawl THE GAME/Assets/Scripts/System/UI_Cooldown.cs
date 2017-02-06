using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Cooldown : MonoBehaviour {

    public float seconds;

    public float max;

    Image Clockwedge;

	// Use this for initialization
	void Start () {
        Clockwedge = transform.GetChild(0).GetComponent<Image>();
	}

    // Update is called once per frame
    protected void Update () {
        Clockwedge.fillAmount = (max -seconds) / max;
    }

    protected void CooldownUpdate(float _seconds, float _max)
    {
        seconds = _seconds;
        max = _max;
    }
}
