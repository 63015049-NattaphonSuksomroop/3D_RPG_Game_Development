using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class Prevscene : MonoBehaviour
    {
        private int prev;
        // Start is called before the first frame update
        void Start()
        {
            prev = SceneManager.GetActiveScene().buildIndex - 1;
        }

        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                SceneManager.LoadScene(prev);
            }
        }
    }
}
