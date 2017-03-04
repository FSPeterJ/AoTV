using UnityEngine;

public class TornadoAttack : MonoBehaviour
{
    private float timepassed;
    public float timeout = 5f;
    private ParticleSystem.MainModule particles;
    private IWeaponBehavior damage;

    // Use this for initialization
    private void Start()
    {
        particles = transform.GetComponent<ParticleSystem>().main;
        damage = transform.GetComponent<IWeaponBehavior>();
        damage.AttackStart();
    }

    // Update is called once per frame
    private void Update()
    {
        if ((timepassed += Time.deltaTime) > timeout)
        {
            particles.loop = false;
            damage.AttackEnd();
        }
    }
}