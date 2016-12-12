using UnityEngine;
using System.Collections;

public class Canon1 : MonoBehaviour
{
    public Transform ballcannon;
    public float foceshot;
    private Transform pointShot;

	// Use this for initialization
	void Start()
    {
        pointShot = transform.Find("Mario");
	}

    float lastime = 0;

    void Update()
    {
        if (Time.time - lastime >= 5)
        {
            GameObject instancia = (GameObject) Instantiate(ballcannon, pointShot.position, pointShot.rotation);
            
        }
    }
}
