using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher1 : MonoBehaviour
{
    Animator anim;
    public GameObject arrow;
    public GameObject arrowSpawn;
    Vector3 arrowPos;
    Quaternion arrowQuat;
    void Start()
    {
        anim = GetComponent<Animator>();
        arrowQuat = new Quaternion(-90, transform.rotation.y, gameObject.transform.rotation.z, transform.rotation.w);
    }

    void Update()
    {
        StartCoroutine(ShootArrow());
    }

    IEnumerator ShootArrow()
    {
        anim.SetTrigger("Arrow Attack");
        yield return new WaitForSeconds(0.4f);
        
        //AudioSource.PlayClipAtPoint(shootSound, transform.position, PlayerPrefs.GetFloat("SFXVolume"));
        GameObject tempBullet = Instantiate(arrow, arrowSpawn.transform.position, arrowQuat);
        //tempBullet.GetComponent<Rigidbody>().velocity = gameObject.transform.forward;
    }

}
