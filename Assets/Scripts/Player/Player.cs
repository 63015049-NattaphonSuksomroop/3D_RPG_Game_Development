using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class Player : MonoBehaviour
    {
        public GameObject Woman;
        public GameObject Man;

        private void Start()
        {
            Woman_Archer_With_Bow_Arrow();
            Man_Paladin();
        }
        public void Woman_Archer_With_Bow_Arrow()
        {
            Debug.Log(Woman + "Start Running");
        }
        public void Man_Paladin()
        {
            Debug.Log(Man + "Start Running");
        }
    }
}
