using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class PlayMedium : MonoBehaviour
    {
        public void PlayGamemode()
        {
            SceneManager.LoadScene("Medium-Map");
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
