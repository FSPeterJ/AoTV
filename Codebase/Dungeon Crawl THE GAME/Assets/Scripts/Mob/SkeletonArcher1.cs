using System.Collections;
using UnityEngine;

public class SkeletonArcher1 : MonoBehaviour
{
    private Animator anim;
    public GameObject arrow;
    public GameObject arrowSpawn;
    private Vector3 arrowPos;
    private Quaternion arrowQuat;

    private void Start()
    {
        anim = GetComponent<Animator>();
        arrowQuat = new Quaternion(-90, transform.rotation.y, gameObject.transform.rotation.z, transform.rotation.w);
    }

    private void Update()
    {
        StartCoroutine(ShootArrow());
    }

    private IEnumerator ShootArrow()
    {
        anim.SetTrigger("Arrow Attack");
        yield return new WaitForSeconds(0.4f);

        //AudioSource.PlayClipAtPoint(shootSound, transform.position, PlayerPrefs.GetFloat("SFXVolume"));
        //GameObject tempBullet = Instantiate(arrow, arrowSpawn.transform.position, arrowQuat);
        Instantiate(arrow, arrowSpawn.transform.position, arrowQuat);
        //tempBullet.GetComponent<Rigidbody>().velocity = gameObject.transform.forward;
    }
}