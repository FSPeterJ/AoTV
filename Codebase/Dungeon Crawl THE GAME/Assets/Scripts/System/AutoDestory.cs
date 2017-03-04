using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDestory : MonoBehaviour
{
    public float delaytime = 0.5f;

    private void OnEnable()
    {
        StartCoroutine("CheckIfAlive");
    }

    private IEnumerator CheckIfAlive()
    {
        while (true)
        {
            yield return new WaitForSeconds(delaytime);
            if (!GetComponent<ParticleSystem>().IsAlive(true))
                Destroy(gameObject);
        }
    }
}