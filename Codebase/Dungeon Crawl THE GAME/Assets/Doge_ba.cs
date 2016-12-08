using UnityEngine;
using System.Collections;

public class Doge_ba : MonoBehaviour
{
    public float DogeSpeed = 5.0f;
    public float attack;
    public bool IsAlive = true;
    public bool AttackMode = false;
    Vector3 dogebaVec = new Vector3(0, 0, 7.0f);
    void Movement()
    {
        while (IsAlive)
        {
            transform.position += dogebaVec; 
        }
    }
}
