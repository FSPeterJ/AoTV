using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Cooldown : MonoBehaviour
{

    public float seconds;
    public float max;
    public float secondaryseconds;
    public float secondarymax;

    Image Clockwedge;
    Image secondaryClockwedge;

    // Use this for initialization
    void Start()
    {
        Clockwedge = transform.GetChild(0).GetComponent<Image>();
        if (transform.childCount > 1)
            secondaryClockwedge = transform.GetChild(1).GetComponent<Image>();
    }

    // Update is called once per frame
    protected void Update()
    {
        Clockwedge.fillAmount = (max - seconds) / max;
        if (secondaryClockwedge)
            secondaryClockwedge.fillAmount = (secondarymax - secondaryseconds) / secondarymax;
    }

    protected void CooldownUpdate(float _seconds, float _max)
    {
        seconds = _seconds;
        max = _max;
    }

    protected void SecondaryCooldownUpdate(float _seconds, float _max)
    {
        secondaryseconds = _seconds;
        secondarymax = _max;

    }
}
