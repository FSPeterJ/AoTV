using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe_Ranged : MonoBehaviour, IRangedPlayerAttack {

    float timePassed;
    float turnspeed = 2f;
    float speed = .25f;
    Vector3 playerPos;
    IWeaponBehavior weaponScript;
    float scaleFactor;

    // Use this for initialization
    void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdatePlayerPosition;
        EventSystem.onPlayerDeath += PlayerDeath;
    }
    //unsubscribe from player movement
    void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdatePlayerPosition;
        EventSystem.onPlayerDeath -= PlayerDeath;
    }

    void Start () {
        weaponScript = FindWeapon(transform).GetComponent<IWeaponBehavior>();
        weaponScript.AttackStart();
    }
	
	// Update is called once per frame
	void Update () {
        transform.localPosition += transform.forward * speed * scaleFactor;

        if (timePassed > .5 && timePassed < 1)
        {
            transform.Rotate(0, 40f * Time.deltaTime, 0);
        }
        else if ( timePassed > 1)
        {
            RotateToFaceTarget(playerPos, turnspeed);
        }
        if(timePassed > 4)
        {
            //Bugfix - https://trello.com/c/GgpQodPs/28-scythe-can-get-into-orbit
            turnspeed += 4f * Time.deltaTime * scaleFactor;
            speed += .1f * scaleFactor * Time.deltaTime;
        }
        timePassed += Time.deltaTime;
        float heightdiff = playerPos.y - transform.position.y + .25f * scaleFactor;
        if (Mathf.Abs(heightdiff) > .5 * scaleFactor)
        {
            transform.position += transform.up * heightdiff * Time.deltaTime;
        }
    }

    void UpdatePlayerPosition(Vector3 _pos)
    {
        playerPos = _pos;
    }

    GameObject FindWeapon(Transform obj)
    {
        foreach (Transform tr in obj)
        {
            if (tr.tag == "Weapon")
            {
                return tr.gameObject;
            }
            if (tr.childCount > 0)
            {
                GameObject temp = FindWeapon(tr);
                if (temp)
                {
                    return temp;
                }
            }
        }
        return null;
    }
    void RotateToFaceTarget(Vector3 _TargetPosition, float _LerpSpeed = 4f, float _AngleAdjustment = -90f)
    {
        Vector3 lookPos = (transform.position - _TargetPosition);
        lookPos.y = 0;
        float angle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, -(Mathf.Atan2(lookPos.z, lookPos.x) * Mathf.Rad2Deg) + _AngleAdjustment, Time.deltaTime * _LerpSpeed);
        //float angle = -(Mathf.Atan2(lookPos.z, lookPos.x) * Mathf.Rad2Deg) - 90;
        transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
    }

    public void ResetAttackStack()
    {
        weaponScript.ResetAttack();
    }
    public void PlayerDeath()
    {
        Destroy(this);
    }

    public void ScaleFactor(float num)
    {
        scaleFactor = num;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
}
