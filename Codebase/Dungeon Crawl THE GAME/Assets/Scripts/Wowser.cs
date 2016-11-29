using UnityEngine;
using System.Collections;

public class Wowser : MonoBehaviour
{
    public float WowX;
    public float WowY;
    public float MarX;
    public float MarY;
    public int tail;
    public bool IsIdle = true;
    public bool IsStunned = false;

    public void Start()
    {
        WowX = 4.0f;
        WowY = 5.0f;
        MarX = 10.0f;
        MarY = 15.0f;
        IsIdle = true;
        IsStunned = false;
    }
	

    public void Follow()
    {
        if(IsStunned == false)
        {
            while(WowX != MarX || WowY != MArY)
            {
                if (MarX > WowX)
                    ++WowX;
                else if (MarX < WowX)
                    --WowX;
                if (MarY > WowY)
                    ++WowY;
                else if (MarY < WowY)
                    --WowY;
            }
        }
    }
}
