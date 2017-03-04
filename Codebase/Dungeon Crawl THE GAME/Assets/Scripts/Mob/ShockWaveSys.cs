using UnityEngine;

public class ShockWaveSys : MonoBehaviour
{
    private float timePassed = 0;
    private float lifetime = 1.5f;

    // Use this for initialization
    private void Start()
    {
        transform.GetChild(0).GetComponent<IWeaponBehavior>().AttackStart();
        transform.GetChild(0).GetComponent<IWeaponBehavior>().ImpactAttack(true);
    }

    // Update is called once per frame
    private void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed > lifetime)
        {
            Destroy(gameObject);
        }
    }
}