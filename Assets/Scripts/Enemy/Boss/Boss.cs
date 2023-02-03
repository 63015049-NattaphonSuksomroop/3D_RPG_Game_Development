using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class Boss : Enemy
    {
        public Enemy Boss_Mutant;
        Animator anim;
        // Start is called before the first frame update
        void Start()
        {
            Mutant();
        }
        public void Mutant()
        {
            Debug.Log(Boss_Mutant + "Boss_Mutant Start Running");
        }
    }
}
