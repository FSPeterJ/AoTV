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
            if(Wowser.GetComponent<Wowser>().CurrentState==BossStates.Charge)
            {
                Mario.GetComponent<BasePlayer>().TakeDamage();
            }
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
        {

       
            //Vector information subtraction from here:
            //http://answers.unity3d.com/questions/24830/create-a-vector-from-one-point-to-another-point-.html

            //Adds an impact force
            if (Mario.GetComponent<CharacterController>().isGrounded)
            {
                Mario.GetComponent<BasePlayer>().AddImpact(Mario.transform.position - Wowser.transform.position, 150);
            }
            Mario.GetComponent<BasePlayer>().TakeDamage();
        }
        isCoroutineExecuting = false;

    }
}
