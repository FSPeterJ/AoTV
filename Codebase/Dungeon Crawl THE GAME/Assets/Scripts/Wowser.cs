using UnityEngine;
using System.Collections;

public class Wowser : MonoBehaviour
{
    public float WowX;
    public float WowY;
    public float MarX;
    public float MarY;
    public int WHealth;
    public int tail;
    public bool IsIdle;
    public bool IsStunned;
    public bool IsAlive;

    public void Start()
    {
        WowX = 4.0f;
        WowY = 5.0f;
        MarX = 10.0f;
        MarY = 15.0f;
        WHealth = 100;
        IsIdle = true;
        IsStunned = false;
        IsAlive = true;
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

    public void Life()
    {
        while (IsAlive == true)
        {
            Follow();
        }
    }

    public void Stun()
    {
        
    }
}
