using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class PlayBoss : MonoBehaviour
    {
        public void PlayGamemode()
        {
            SceneManager.LoadScene("Boss-Map");
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                Debug.Log("Quit");

            }
        }
    }
}
