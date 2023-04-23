using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class xpContoller : Enemy
    {
        [SerializeReference] public TextMeshProUGUI LevelText;
        [SerializeReference] public TextMeshProUGUI ExperienceText;
        [SerializeReference] private int Level;
        [SerializeReference] public float CurrentXp;
        [SerializeReference] public float TargetXp;
        [SerializeReference] private Image XpProgresBar;
        
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CurrentXp += 12;
            }
            ExperienceText.text = CurrentXp + "/" + TargetXp;
            ExperienceController();
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
            }
        }
    }
}
