using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class GameManager : MonoBehaviour
    {
        public int score = 0;
        public Text scoretext;
        void Update()
        {
            scoretext.text = "Score = " + score.ToString();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                Debug.Log("ออกจากเกม");
            }
        }
        public void killEnemy()
        {
            score += 1;
            Debug.Log("Score = " + score);
    }
    }
