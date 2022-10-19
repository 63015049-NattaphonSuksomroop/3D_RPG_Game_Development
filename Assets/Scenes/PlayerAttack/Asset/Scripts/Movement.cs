using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public CharacterController controller;

    Animator animator;

    [Header("Arrow")]
    public GameObject HandArrow;

    void Start()
    {
        animator = GetComponent<Animator>();
        HandArrow.gameObject.SetActive(false);
    }
    void HandArrowActive()
    {
        HandArrow.gameObject.SetActive(true);
    }
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        animator.SetFloat("Strafe", x);
        animator.SetFloat("Forward", y);

        if(Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }
        if(Input.GetButton("Fire1"))
        {
            animator.SetBool("aim", true);
        }    
        if(Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("aim", false);
            animator.SetBool("shoot", true);
        }
        else
        {
            animator.SetBool("shoot", false);
        }
    }
}
