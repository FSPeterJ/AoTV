using UnityEngine;
using System.Collections;


public enum BossStates
{
    Idle,
    Moving,
    Stomp,
    FireBreath,
    Stunned

}

public class Wowser : MonoBehaviour
{
    float MarX;
    float MarY;
    float WowX;
    float WowY;
    float tail;
    bool IsIdle = true;
    bool IsStunned = false;
    bool lowHP = false;
    BossStates CurrentState = BossStates.Idle;
    float timeElapsed = 0.0f;
    void Start()
    {

    }

    void Update()
    {
        switch (CurrentState)
        {
            case BossStates.Idle:
                IdleState();
                break;
            case BossStates.Moving:
                MovingState();
                break;
            case BossStates.Stomp:
                StompState();
                break;
            case BossStates.FireBreath:
                FirebreathState();
                break;
            case BossStates.Stunned:
                StunndedState();
                break;
            default:
                break;
        }
    }
    public void Follow()
    {
        if (IsStunned == false)
        {
            while (WowX != MarX || WowY != MarY)
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
    void IdleState()
    {
        //controls duration of IdleState // change hard coded 1 eventually
        if(timeElapsed>1)
        {
            CurrentState = BossStates.Moving;
            timeElapsed = 0;
        }
       
        while(timeElapsed<1)
        {
            timeElapsed += Time.deltaTime;
            continue;
        }
       

    }
    void MovingState()
    {

    }
    void StompState()
    {


    }
    void FirebreathState()
    {


    }
    void StunndedState()
    {


    }
    void SetCurrentState(BossStates NewState)
    {
        CurrentState = NewState;
    }
}
