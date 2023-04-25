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
        xpContoller controller;
        public int HP = 100;
        [SerializeReference] public float CurrentXp;
        public Slider healthBar;

        public Animator animator;
        public Transform Player;
        public Transform Key;
        public GameObject Enemy_Skeleton;
        public GameObject Enemy_Dragon;
        bool isKill = false;


        public static bool checkAttack = false;
        public static bool checkAttack1 = false;
        /*  public Enemy(GameManager gm) {
              this.manager = gm;
          }

          public void setGameManager(GameManager gm)
          {
              this.manager = gm;
          }*/


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
            if (controller == null)
            {
                GameObject temp = GameObject.FindGameObjectWithTag("lv") as GameObject;
                controller = temp.GetComponent<xpContoller>();
            }
            nav = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            Skeleton1();
            Dragon1();
            isKill = false;
        }
        void Update()
        {

            healthBar.value = HP;
            nav.destination = target.position;

            if (isKill)
            {
                /*           manager.show();
                             manager.getExp();*/
                isKill = false;
                controller.getExp();
                controller.ExperienceController();
            }

            if (nav.remainingDistance <= nav.stoppingDistance)
            {
                animator.SetBool("isAttacking", true);
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
            /*
            if (Vector3.Distance(transform.position, Player.position) < 6)
            {
                if (Input.GetKeyDown("f"))
                {
                    Inventory.Refrence.KilledEnemys += 1;
                    if (Key)
                    {
                        KeyCode.gameObject.SetActiveRecursively(true);
                    }
                    Destroy(gameObject);
                }
            }
            */

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
            //int i = Input.GetKeyDown("k");
            if (HP <= 0)
            {
                isKill = true;
                GetComponent<Collider>().enabled = false;
                animator.SetTrigger("die");
                manager.killEnemy();
                manager.ExpLv();

                
                manager.getExp();
                //transform.gameObject.GetComponent<CurrentXp>();
                StartCoroutine(removeEnemy());
                Debug.Log("Kill Enemy");
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

        public void BeginAttack()
        {
            checkAttack = true;
            checkAttack1 = true;
        }
    }
}
