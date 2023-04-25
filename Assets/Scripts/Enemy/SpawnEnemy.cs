using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class SpawnEnemy : MonoBehaviour
    {
        
        public GameObject enemy;
        public float spawnTime = 0f;
        public float delay = 5f;
        public GameObject[] spawnPoint;

        // Update is called once per frame
        void Update()
        {
            spawnTime += Time.deltaTime;
            if (spawnTime >= delay)
            {
                int index = Random.Range(0, spawnPoint.Length);
                GameObject enemClone = Instantiate(enemy) as GameObject;
                Instantiate(enemy, spawnPoint[index].transform.position, spawnPoint[index].transform.rotation);
                spawnTime -= delay;
            }
        }
    }
}
