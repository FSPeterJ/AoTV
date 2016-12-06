using UnityEngine;
using System.Collections;

public class stompTrigger : MonoBehaviour
{


    public GameObject Wowser;
    public GameObject Mario;

    //bool PlayerInRange;
    bool isCoroutineExecuting = false;
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Entered");
        if (col.tag == "Player")
        {
            if (Wowser.GetComponent<Wowser>().CurrentState == BossStates.Moving)
            {
                Wowser.GetComponent<Wowser>().PlayerInRange = true;
                Wowser.GetComponent<Wowser>().CurrentState = BossStates.Stomp;
               StartCoroutine(waitForWowserToLand());
                Debug.Log("Entered");
            }
            else if (Wowser.GetComponent<Wowser>().CurrentState == BossStates.Moving)
                Wowser.GetComponent<Wowser>().PlayerInRange = true;
        }
    }
    public void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            Wowser.GetComponent<Wowser>().PlayerInRange = false;
        }
    }
    public void OnTriggerStay(Collider col)
    {
        if(col.tag == "Player")
        Wowser.GetComponent<Wowser>().PlayerInRange = true;
    }
    public void EnableParticleSystem()
    {
        GetComponent<ParticleSystem>().enableEmission = true;
    }
    public void DisableParticleSystem()
    {
        GetComponent<ParticleSystem>().enableEmission = false;
    }
    IEnumerator waitForWowserToLand()
    {
        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;
        yield return new WaitForSeconds(2.65f);
        if (Wowser.GetComponent<Wowser>().PlayerInRange)
            Mario.GetComponent<BasePlayer>().TakeDamage();
        isCoroutineExecuting = false;

    }
}
