using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class Enemy : MonoBehaviour
    {
        Transform target;
        NavMeshAgent nav;
        GameManager manager;
        public int HP = 100;
        public Slider healthBar;

        public Animator animator;

        public GameObject Enemy_Skeleton;
        public GameObject Enemy_Dragon;
        void Start()
        {
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            if (manager == null)
            {
                GameObject temp = GameObject.FindGameObjectWithTag("GameController") as GameObject;
                manager = temp.GetComponent<GameManager>();
            }
            nav = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            Skeleton1();
            Dragon1();
        }
        void Update()
        {
            healthBar.value = HP;
            nav.destination = target.position;
            if (nav.remainingDistance <= nav.stoppingDistance)
            {
                animator.SetBool("isAttacking", true);
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
        }
        public void Scream()
        {
            FindObjectOfType<AudioManager>().Play("DragonScream");
        }

        public void Attack()
        {
            FindObjectOfType<AudioManager>().Play("DragonAttack");
        }
        /*
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Weapon")
            {
                Destroy(other.gameObject);
                nav.isStopped = true;
                animator.SetTrigger("die");
            }
        }
        */
        public void Skeleton1()
        {
            Debug.Log(Enemy_Skeleton + "Start Running");
        }

        public void Dragon1()
        {
            Debug.Log(Enemy_Dragon + "Start Running");
        }

        public void TakeDamage(int damageAmount)
        {
            HP -= damageAmount;
            if (HP <= 0)
            {
                GetComponent<Collider>().enabled = false;
                animator.SetTrigger("die");
                manager.killEnemy();
                StartCoroutine(removeEnemy());
            }
            else
            {
                animator.SetTrigger("damage");
            }
        }
        IEnumerator removeEnemy()
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }
}
