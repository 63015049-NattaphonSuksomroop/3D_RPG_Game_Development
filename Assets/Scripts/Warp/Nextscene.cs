using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class Nextscene : MonoBehaviour
    {
        private int next;
        // Start is called before the first frame update
        void Start()
        {
            next = SceneManager.GetActiveScene().buildIndex + 1;
        }

        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                SceneManager.LoadScene(next);
            }
        }
    }
}
