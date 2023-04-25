using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemy : MonoBehaviour
{
    public GameObject theEnemy;
    public int xPos;
    public int zPos;
    public int enemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop());
        StartCoroutine(EnemyDrop1());
    }
    IEnumerator EnemyDrop()
    {
        while (enemyCount < 5)
        {
            xPos = Random.Range(1, 15);
            zPos = Random.Range(1, 33);
            Instantiate(theEnemy, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }
    IEnumerator EnemyDrop1()
    {
        while (enemyCount < 5)
        {
            xPos = Random.Range(1, 150);
            zPos = Random.Range(1, 136);
            Instantiate(theEnemy, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }
}
