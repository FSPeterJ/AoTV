using UnityEngine;
using System.Collections;

public class bombCol : MonoBehaviour
{
    public GameObject wowser;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Explosive")
        {
            wowser.GetComponent<Wowser>().bHealth--;
            destroyBomb(col.gameObject);
        }
    }

    IEnumerator destroyBomb(GameObject go)
    {
        go.GetComponent<Renderer>().enabled = false;
        go.GetComponent<ParticleSystem>().enableEmission = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(go);

    }
}