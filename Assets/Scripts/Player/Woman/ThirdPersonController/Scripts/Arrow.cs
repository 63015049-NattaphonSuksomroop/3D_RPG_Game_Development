using UnityEngine;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class Arrow : MonoBehaviour
    {
        public int damageAmount = 35;


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
            
            /*
            if (other.gameObject.CompareTag("GameController"))
            {
                other.gameObject.GetComponent<GameManager>().ExpLv();
                Destroy(gameObject);
            }*/

            Destroy(gameObject, 1);

        }
    }

}
