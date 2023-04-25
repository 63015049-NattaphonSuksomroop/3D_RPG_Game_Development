using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class IsAlive : MonoBehaviour
    {
        
        public bool isAlivee = true;
        public Slider slider;
        public AudioSource audioSource;
        public AudioClip hit, die;
        public Text HpText;
        public Animator _animator;
        // Start is called before the first frame update
        void Start()
        {
            isAlivee = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
