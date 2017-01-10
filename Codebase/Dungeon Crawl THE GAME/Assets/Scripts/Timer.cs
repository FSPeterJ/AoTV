using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float sTime = 600;
    float rTime;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(StartCountdown());
    }
    
    public IEnumerator StartCountdown(float eTime = 0)
    {
        ++eTime;
        rTime = sTime - eTime;

        while (sTime > 0)
        {
            Debug.Log("Time: " + rTime);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
