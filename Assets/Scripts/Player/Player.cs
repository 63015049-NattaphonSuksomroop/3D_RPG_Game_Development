using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class Player : IsAlive
    {
        public GameObject Woman;
        public GameObject Man;

        //public bool isAlive = true;

        //public static bool isAlive = false;

        void Start()
        {
            Woman_Archer_With_Bow_Arrow();
            Man_Paladin();
        }
        void Update()
        {


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
                HealthP -= 10;
                slider.value = HealthP;
                Enemy.checkAttack = false;
                Debug.Log(HealthP);

            }
            if (other.gameObject.tag == "EnemyAttack")
            {
                HealthP -= 5;
                slider.value = HealthP;
                Enemy.checkAttack1 = false;
                Debug.Log(HealthP);
            }
        }
    }
}
