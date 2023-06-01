using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGameDevelopmentKMITL
{
    public class Enemycontroller : MonoBehaviour
    {
        public float lookRadius = 10f;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, lookRadius);
        }
    }
}
