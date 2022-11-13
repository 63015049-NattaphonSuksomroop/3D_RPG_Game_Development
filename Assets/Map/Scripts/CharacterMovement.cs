using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Animator animator; 
    CharacterController characterController; //เก็บค่า component

    public float speed = 6.0f; 
    public float rotationSpeed = 25; //การหมุน
    public float jumpSpeed = 7.5f; 
    public float graviy = 20.0f; //แรงโน้มถ่วง
    Vector3 inputVec; //แกนที่จะวิ่ง จะรับค่า x,y,z เคลื่อนที่ movement
    Vector3 targetDirection; //ทิศทางที่จะไปจริงๆ 

    private Vector3 moveDirection = Vector3.zero; //

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = -(Input.GetAxisRaw("Vertical"));
        float z = Input.GetAxisRaw("Horizontal");
        inputVec = new Vector3(x, 0, z);

        animator.SetFloat("Input X", z);
        animator.SetFloat("Input Z", -(x));

        if (x != 0 || z != 0)
        {
            animator.SetBool("Moving", true);
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Running", false);
        }

        //Jump
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;
        }
        characterController.Move(moveDirection * Time.deltaTime);
        UpdateMovement();

    }
    void UpdateMovement()
    {
        Vector3 motion = inputVec;
        motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? .7f : 1;
        RotateTowardMovementDirection();
        getCameraRealtive();
    }
    void RotateTowardMovementDirection()
    {
        if(inputVec != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationSpeed);
        }
    }
    void getCameraRealtive()
    {
        Transform cameraTransform = Camera.main.transform;
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        targetDirection = (h * right) + (v * forward);
    }
}
