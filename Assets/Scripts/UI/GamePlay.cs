using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class GamePlay : MonoBehaviour
    {
        public void PlayGamemode()
        {
            SceneManager.LoadScene("Easy-Map");
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                Debug.Log("ออกจากเกม");
            }
        }
    }
}
