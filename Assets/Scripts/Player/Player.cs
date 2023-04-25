using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class Player : MonoBehaviour
    {
        public GameObject Woman;
        public GameObject Man;

        //health
        public int healthPlayer = 100;
        public Slider slider;

        public static bool isAlive = false;

        void Start()
        {
            Woman_Archer_With_Bow_Arrow();
            Man_Paladin();
            isAlive = true;
        }
        void Update()
        {
            if (healthPlayer <= 0 && isAlive)
            {
                healthPlayer = 0;
                isAlive = false;
                Debug.Log(isAlive);
            }
        }
        public void Woman_Archer_With_Bow_Arrow()
        {
            Debug.Log(Woman + "Start Running");
        }
        public void Man_Paladin()
        {
            Debug.Log(Man + "Start Running");
        }
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "EnemyWeapon" && Enemy.checkAttack)
            {
                healthPlayer -= 10;
                slider.value = healthPlayer;
                Enemy.checkAttack = false;
                Debug.Log(healthPlayer + "DamageEnemy");
            }
            if (other.gameObject.tag == "EnemyAttack")
            {
                healthPlayer -= 5;
                slider.value = healthPlayer;
                Enemy.checkAttack1 = false;
                Debug.Log(healthPlayer + "DamageEnemy1");
            }
        }
    }
}
