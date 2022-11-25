using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damageAmount = 15;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(transform.GetComponent<Rigidbody>());
        Destroy(transform.GetComponent<Collider>());
        transform.parent = other.transform;
        
        if (other.tag == "Dragon")
        {
            other.GetComponent<Dragon>().TakeDamage(damageAmount);
        }
        Destroy(gameObject, 1);
    }
}
