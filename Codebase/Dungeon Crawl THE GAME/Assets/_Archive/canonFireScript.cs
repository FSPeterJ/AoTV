using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canonFireScript : MonoBehaviour
{
    [SerializeField]
    GameObject fireball;
    
    float passedTime;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        passedTime += Time.deltaTime;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (passedTime > 0.5)
            {
                Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z) + transform.forward * 1.5f;
                GameObject fb = Instantiate(fireball, pos, Quaternion.identity) as GameObject;
                fb.GetComponent<Rigidbody>().AddForce(transform.forward * 700);
                passedTime = 0;
            }
        }
    }    
}
