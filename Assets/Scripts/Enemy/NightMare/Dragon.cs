using UnityEngine;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    private int HP = 100;
    public Slider healthBar;
    
    public Animator animator;
    
    void Update(){
    	healthBar.value = HP;
    }
    

    public void Scream()
    {
        FindObjectOfType<AudioManager>().Play("DragonScream");
    }

    public void Attack()
    {
        FindObjectOfType<AudioManager>().Play("DragonAttack");
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if(HP <= 0)
        {
            animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false;
            FindObjectOfType<AudioManager>().Play("DragonDeath");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("DragonDamage");
            animator.SetTrigger("damage");
        }
    }

}
