using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class GamePlay : MonoBehaviour
    {
        //public Text highscoretxt;


        public void Start()
        {
            /*
            int highscore = PlayerPrefs.GetInt("Hightscore");
            highscoretxt.text = "Hightscore = " + highscore.ToString();
            */
        }
        public void PlayGamemode()
        {
            SceneManager.LoadScene("UI_Mode");
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
