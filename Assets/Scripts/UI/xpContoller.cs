using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class xpContoller : GameManager
    {

        GameManager manager;

        // Update is called once per frame
        void Update()
        {

            //ExperienceText.text = CurrentXp + "/" + TargetXp.ToString();
            //int i = score;
            //if (Input.GetKeyDown(KeyCode.Space))
            //if (i >= 1)

            ExperienceController();
            ExpLv();
        }

        public void ExperienceController()
        {
            LevelText.text = "Lv. " + Level.ToString();
            XpProgresBar.fillAmount = (CurrentXp / TargetXp);

            if(CurrentXp >= TargetXp)
            {
                CurrentXp = CurrentXp - TargetXp;
                Level++;
                TargetXp += 2000;
                Debug.Log("Level UP"+ Level );
            }
            ExperienceText.text = CurrentXp + "/" + TargetXp;
        }

    }
}
