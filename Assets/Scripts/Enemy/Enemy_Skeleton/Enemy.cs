using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class Enemy : MonoBehaviour
    {
        // Start is called before the first frame update
        Transform target;
        NavMeshAgent nav;
        Animator anim;

        public float lookRadius = 10f;
        //public LayerMask hitLayers;
        void Start()
        {
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            nav = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame  
        void Update()
        {
            {
                nav.destination = target.position;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Arrow")
            {
                Destroy(other.gameObject);
                nav.isStopped = true;
                anim.SetTrigger("die");
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, lookRadius);
        }

    }

}