using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class GameManager : MonoBehaviour
    {
        
        public int score = 0;
        public Text scoretext;

        [SerializeReference] public TextMeshProUGUI LevelText;
        [SerializeReference] public TextMeshProUGUI ExperienceText;
        [SerializeReference] public int Level;
        [SerializeReference] public float CurrentXp;
        [SerializeReference] public float TargetXp;
        [SerializeReference] public Image XpProgresBar;

        public GameManager()
        {
            CurrentXp = 0;
        }

        void Update()
        {
          /*  ExperienceController();
            ExpLv();*/
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
            if (score >= PlayerPrefs.GetInt(""))
            {

            }
            Debug.Log("Score = " + score);

        }

        public void show()
        {
            Debug.Log("KILL");
        }
        public void ExpLv()
        {
            /*
            CurrentXp += 2000;
            ExperienceText.text = CurrentXp + "/" + TargetXp;
            */
/*            if (Input.GetKeyDown("k"))
            {
                getExp();
            }
            */
        }
        public float getExp()
        {
            
            CurrentXp += 2000;
            return CurrentXp;
        }
    }

}
