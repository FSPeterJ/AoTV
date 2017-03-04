using UnityEngine;

public class BeginBossBattle : MonoBehaviour
{
    public Dragon drag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            drag.SendMessage("StartFight");
        }
    }
}