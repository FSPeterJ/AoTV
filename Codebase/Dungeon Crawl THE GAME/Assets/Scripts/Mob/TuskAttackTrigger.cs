using UnityEngine;

public class TuskAttackTrigger : MonoBehaviour
{
    private bool playerHit = false;

    //pick one or the other can condition the stay around time flow smoother
    //   void OnTriggerEnter(Collider col)
    //   {
    //
    //
    //       if (col.tag == "Player")
    //       {
    //           Debug.Log("Player Hit");
    //       }
    //
    //   }
    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (playerHit == false)
            {
                playerHit = true;
                //Debug.Log("Player Hit");
                col.GetComponent<Player>().TakeDamage();
            }
        }
    }
}