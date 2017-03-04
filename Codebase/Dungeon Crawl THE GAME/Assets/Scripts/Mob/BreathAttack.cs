using UnityEngine;

public class BreathAttack : MonoBehaviour
{
    private float timePassed = 0;
    private float lifetime = 2f;
    private ParticleSystem.EmissionModule particlesEM;
    private IWeaponBehavior damage;

    // Use this for initialization
    private void Start()
    {
        particlesEM = transform.GetComponent<ParticleSystem>().emission;
        damage = transform.GetComponent<IWeaponBehavior>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void StartAttack()
    {
        damage.AttackStart();
        particlesEM.enabled = true;
    }

    public void EndAttack()
    {
        damage.AttackEnd();
        particlesEM.enabled = false;
    }
}