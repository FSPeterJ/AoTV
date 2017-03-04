using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float time = 0;
    private Rigidbody body;

    // Use this for initialization
    private void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
        body.AddForce(transform.forward);
        //body.freezeRotation = true;
        if (time > 4)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().TakeDamage(2);
            other.gameObject.GetComponent<Animator>().SetTrigger("Take Damage");
        }
        Destroy(gameObject);
    }
}