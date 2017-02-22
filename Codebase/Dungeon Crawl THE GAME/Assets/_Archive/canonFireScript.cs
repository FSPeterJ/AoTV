using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canonFireScript : MonoBehaviour
{
    GameObject fireball;
    
    float passedTime;
    // Use this for initialization
    void Start()
    {
        fireball = (GameObject)Resources.Load("Prefabs/Projectiles/Fireball Projectile");
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
                Instantiate(fireball, pos, transform.rotation);
                //Instantiate(fireball, transform.position, transform.rotation * Quaternion.Euler(0, -90, 0));
                passedTime = 0;
            }
        }
    }    
}
