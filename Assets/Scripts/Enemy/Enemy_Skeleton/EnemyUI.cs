using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class EnemyUI : MonoBehaviour
    {
        private int HP = 100;
        public Slider healthBar;

        public Animator animator;

        // Update is called once per frame
        void Update()
        {
            healthBar.value = HP;
        }

        public void TakeDamage(int damageAmount)
        {
            HP -= damageAmount;
            if (HP <= 0)
            {
                animator.SetTrigger("die");
                GetComponent<Collider>().enabled = true;
            }
            else
            {
                animator.SetTrigger("damage");
            }
        }
    }
}
