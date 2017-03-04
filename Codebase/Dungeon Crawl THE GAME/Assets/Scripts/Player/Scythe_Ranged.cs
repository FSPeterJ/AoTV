using UnityEngine;

public class Scythe_Ranged : MonoBehaviour, IRangedPlayerAttack
{
    private float timePassed;
    private float turnspeed = 2f;
    private float speed = 20f;
    private Vector3 playerPos;
    private IWeaponBehavior weaponScript;
    private float scaleFactor;

    // Use this for initialization
    private void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdatePlayerPosition;
        EventSystem.onPlayerDeath += PlayerDeath;
    }

    //unsubscribe from player movement
    private void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdatePlayerPosition;
        EventSystem.onPlayerDeath -= PlayerDeath;
    }

    private void Start()
    {
        weaponScript = FindWeapon(transform).GetComponent<IWeaponBehavior>();
        weaponScript.AttackStart();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.localPosition += transform.forward * speed * scaleFactor * Time.deltaTime;

        if (timePassed > .5 && timePassed < 1)
        {
            transform.Rotate(0, 40f * Time.deltaTime, 0);
        }
        else if (timePassed > 1)
        {
            RotateToFaceTarget(playerPos, turnspeed);
        }
        if (timePassed > 4)
        {
            turnspeed += 8f * Time.deltaTime;
            speed += 4f * scaleFactor * Time.deltaTime;
        }
        timePassed += Time.deltaTime;
        float heightdiff = playerPos.y - transform.position.y + .25f * scaleFactor;
        if (Mathf.Abs(heightdiff) > .5 * scaleFactor)
        {
            transform.position += transform.up * heightdiff * Time.deltaTime;
        }
    }

    private void UpdatePlayerPosition(Vector3 _pos)
    {
        playerPos = _pos;
    }

    private GameObject FindWeapon(Transform obj)
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

    private void RotateToFaceTarget(Vector3 _TargetPosition, float _LerpSpeed = 4f, float _AngleAdjustment = -90f)
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