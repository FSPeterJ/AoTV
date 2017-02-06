using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dragon : MonoBehaviour, IEnemyBehavior
{
    public enum AI
    {
        Idle, Bite, Firebreath, TakeDamage, Die, Fly_Bite, Fly_Forward, Fly_Idle, Fly_Firebreath
    }

    public AI cDs;
    public AI CurrentState
    {
        get { return cDs; }
        set
        {
            switch (value)
            {
                case AI.Idle:
                    {
                        NavAgent.enabled = false;
                        Bcollider.enabled = true;
                        Anim.SetBool("Idle", true);
                    }
                    break;
                case AI.Bite:
                    break;
                case AI.Firebreath:
                    {

                    }
                    break;
                case AI.TakeDamage:
                    {

                    }
                    break;
                case AI.Die:
                    {
                        NavAgent.enabled = false;
                        Bcollider.enabled = false;
                    }
                    break;
                case AI.Fly_Bite:
                    break;
                case AI.Fly_Forward:
                    break;
                case AI.Fly_Idle:
                    break;
                case AI.Fly_Firebreath:
                    break;
                default:
                    break;
            }
        }
    }


    

    //variables
    Animator Anim;
    Vector3 Targetposition;
    float TargetDist;
    bool flystates = false;

    //wamdering variables
    Vector3 wanderSphere;
    Vector3 Originpos;
    NavMeshHit NavhitPos;

    //Stat Variables
    public int HP = 100;
    bool Dead = false;

    //References
    NavMeshAgent NavAgent;
    BoxCollider Bcollider;
    //IWeaponBehavior weaponscript;
    GameObject Mouththing;

    float idleTime = 0;

    void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdatetargetPos;
        EventSystem.onPlayerDeath += PlayerDied;
    }

    void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdatetargetPos;
        EventSystem.onPlayerDeath -= PlayerDied;
    }

	// Use this for initialization
	void Start()
    {
        
        Anim = GetComponent<Animator>();
        Originpos = transform.position;
        NavAgent = GetComponent<NavMeshAgent>();
        Bcollider = GetComponent<BoxCollider>();
        Mouththing = transform.Find("RigHeadGizmo").gameObject;
        CurrentState = AI.Idle;
        NavhitPos.hit = true;
	}
	
	// Update is called once per frame
	void Update()
    {
        TargetDist = Vector3.Distance(Targetposition, transform.position);

        Debug.Log(CurrentState.ToString());
        Debug.Log("Distance to target is "+ TargetDist.ToString());
        switch (CurrentState)
        {
            case AI.Idle:
                break;
            case AI.Bite:
                {

                }
                break;
            case AI.Firebreath:
                break;
            case AI.TakeDamage:
                break;
            case AI.Die:
                break;
            case AI.Fly_Bite:
                break;
            case AI.Fly_Forward:
                break;
            case AI.Fly_Idle:
                break;
            case AI.Fly_Firebreath:
                break;
            default:
                break;
        }

        //setting the condition to make
        //the dragon go into the air when
        //his health is less than or equal to 50
        //if (flystates == true)
        //    NavAgent.baseOffset += 5;
        //if (HP <= 50)
        //    flystates = true;

    }
    
    void UpdatetargetPos(Vector3 Pos)
    {
        Targetposition = Pos;
    }

    public void Kill()
    {
        CurrentState = AI.Die;
    }

    public void TakeDamage(int damage  = 1)
    {
        if (!Dead)
        {
            HP -= damage;

            if (HP < 1)
            {
                Kill();
            }
            else
            {
                CurrentState = AI.TakeDamage;
            }
        }
    }

    void PlayerDied()
    {
        EventSystem.onPlayerPositionUpdate -= UpdatetargetPos;
        Targetposition = new Vector3(Targetposition.x, 999999, Targetposition.z);
    }

    void RotateTofacetarget(Vector3 _targetPosition, float _Lspeed = .2f, float _AA = -90f)
    {
        Vector3 PTL = transform.position - _targetPosition;
        PTL.y = 0;
        float Angle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, -(Mathf.Atan2(PTL.z, PTL.x) * Mathf.Rad2Deg) + _AA, _Lspeed);
        transform.rotation = Quaternion.AngleAxis(Angle, new Vector3(0, 1, 0));
    }

    Ray AttkRngeFwd;
    Ray AttkRngeLft;
    Ray AttkRngeRght;
    float AttkRnge = 2.0f;
    bool AttkRngeCheck()
    {
        Debug.DrawRay(Mouththing.transform.position + Vector3.up, -Mouththing.transform.right * AttkRnge, Color.red);
        Debug.DrawRay(Mouththing.transform.position + Vector3.up, (-Mouththing.transform.right + Mouththing.transform.forward * 0.25f) * AttkRnge, Color.red);
        Debug.DrawRay(Mouththing.transform.position + Vector3.up, (-Mouththing.transform.right - Mouththing.transform.forward * 0.25f) * AttkRnge, Color.red);
        AttkRngeFwd = new Ray(Mouththing.transform.position + Vector3.up, -Mouththing.transform.right * 3f);
        AttkRngeRght = new Ray(Mouththing.transform.position + Vector3.up, (-Mouththing.transform.right - Mouththing.transform.forward * 0.25f) * 3f);
        AttkRngeLft = new Ray(Mouththing.transform.position + Vector3.up, (-Mouththing.transform.right + Mouththing.transform.forward * 0.25f) * 3f);
        RaycastHit fwdHit;
        RaycastHit LftHit;
        RaycastHit RghtHit;

        if (Physics.Raycast(AttkRngeFwd, out fwdHit, AttkRnge) && fwdHit.transform.tag == "Player" || Physics.Raycast(AttkRngeLft, out LftHit, AttkRnge) && LftHit.transform.tag == "Player" || Physics.Raycast(AttkRngeRght, out RghtHit, AttkRnge) && RghtHit.transform.tag == "Player")
        {
            return true;
        }
        return false;
    }

    public void ResetToIdle()
    {
        CurrentState = AI.Idle;
    }

    public int RemainingHealth()
    {
        return HP;
    }

}
