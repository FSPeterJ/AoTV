using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIsGrounded : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    int collisionCount = 0;
    bool isGrounded = false;
    bool collisionOccured = false;

    List<Collider> collisionList = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        collisionList.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        collisionList.Remove(other);
    }

    void Update()

    {
        collisionList.RemoveAll(IsEMpty);
        if (isGrounded)
        {
            if (collisionList.Count < 1)
            {
                EventSystem.PlayerGrounded(false);
                isGrounded = false;
            }

        }
        else
        {
            if (collisionList.Count > 0)
            {
                EventSystem.PlayerGrounded(true);
                isGrounded = true;
            }
        }
    }

    private bool IsEMpty(Collider col)
    {
        return col == null;
    }
}
