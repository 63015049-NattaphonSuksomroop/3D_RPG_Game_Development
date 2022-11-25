using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGameDevelopment.KMITL.CE.ProjectFourth
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField]
        private float rotationSpeed;

        [SerializeField]
        private float JumpHeight;

        [SerializeField]
        private float gravityMultiplier;

        [SerializeField]
        private float JumpHorizontalSpeed;

        [SerializeField]
        private float JunpButtonGracePeriodSpeed;

        [SerializeField]
        private float cameraTransform;

        // Start is called before the first frame update
        Animator animator;
        CharacterController characterController;
        public float speed = 3.0f;
        public float roatationSpeed = 25;
        public float jumpSpeed = 5f;
        public float gravity = 20.0f;
        Vector3 inputVec;
        Vector3 targetDiriction;
        private Vector3 moveDirection = Vector3.zero;
        private float ySpeed;

        public GameObject Arrow;
        //RaycastHit hit;
        //float range = 1000f;
        public Transform ArrowSpawPosition;
        [Header("Arrow")]
        public GameObject HandArrow;

        public Transform MainCameraTransform { get; private set; }

        private void Awake()
        {
            MainCameraTransform = Camera.main.transform;
        }
        void Start()
        {
            Time.timeScale = 1;
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            //shoot();
            HandArrowActive();
            HandArrow.gameObject.SetActive(false);
            Debug.Log(animator + "Start Running");
        }
        /*
        void shoot()
        {
            HandArrow.gameObject.SetActive(true);

            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            if (Physics.Raycast(ray, out hit, range))
            {
                GameObject ArrowInstantiate = GameObject.Instantiate(Arrow, ArrowSpawPosition.transform.position, ArrowSpawPosition.transform.rotation) as GameObject;
                ArrowInstantiate.GetComponent<Arrow>().setTarget(hit.point);
            }
        }
        */
        void HandArrowActive()
        {
            HandArrow.gameObject.SetActive(true);
        }
        // Update is called once per frame
        public LayerMask hitLayers;
        void Update()
        {
            //Check A* กับ Enemy to Player โดยค้นหา Shotest Path จาก Pathfinding.cs
            if (Input.GetMouseButtonDown(0))//คลิกตัวผู้เล่นเเละเคลื่อยย้ายจากจุดเพื่อทดสอบตำแหน่ง Enemy to Player
            {
                Vector3 mouse = Input.mousePosition;//รับตำแหน่งเมาส์
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);//ScreenPointToRayเพื่อให้ได้ตำแหน่งที่เมาส์ชี้มา
                RaycastHit hit;//เก็บตำแหน่ง ray hit.
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))//ถ้า raycast ไม่ชนกำแพง
                {
                    this.transform.position = hit.point;//ย้ายเป้าหมายไปที่ตำแหน่งเมาส์
                }
            }
            float x = -(Input.GetAxisRaw("Vertical"));
            float y = Input.GetAxisRaw("Jump");
            float z = Input.GetAxisRaw("Horizontal");
            inputVec = new Vector3(x, y, z);

            animator.SetFloat("Input X", z);
            animator.SetFloat("Input Y", 0);
            animator.SetFloat("Input Z", -(x));

            ySpeed += Physics.gravity.y * Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("Running", true);
            }
            else
            {
                animator.SetBool("Running", false);
            }
            //Jump
            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpSpeed;
            }
            //Aim
            if (Input.GetButton("Fire1"))
            {
                animator.SetBool("Aim", true);
            }
            if (Input.GetButtonUp("Fire1"))
            {
                animator.SetBool("Aim", false);
                animator.SetBool("Shoot", true);
            }
            else
            {
                animator.SetBool("Shoot", false);
            }
            if (x != 0 || z != 0) //X ,Y or Z != 0
            {
                animator.SetBool("Moving", true);
                animator.SetBool("Running", true);
            }
            else
            {
                animator.SetBool("Moving", false);
                animator.SetBool("Running", false);
            }

            //Input=Horizontal,Jump,Vertical
            if (characterController.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical")); // 
                moveDirection *= speed;
            }
            characterController.Move(moveDirection * Time.deltaTime);
            UpdateMovement();
            Debug.Log(animator + "Update Running");
            Debug.Log(moveDirection + "moveDirection");
            Debug.Log(speed + "speed");
            Debug.Log(ySpeed + "ySpeed");
            Debug.Log(jumpSpeed + "jumpSpeed");

        }

        void UpdateMovement()
        {
            Vector3 motion = inputVec;
            motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? .7f : 1;
            RotateTowardMovementDirection();
            getCameraRealtive();
            Debug.Log(inputVec + "UpdateMovement");
        }

        void RotateTowardMovementDirection()
        {
            if (inputVec != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDiriction), Time.deltaTime * roatationSpeed);
            }
        }
        void getCameraRealtive()
        {
            Transform cameraTransform = Camera.main.transform; //เคลื่อนที่ไปทางไหนทิศทางจะตามด้วย
            Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;

            Vector3 right = new Vector3(forward.z, 0, -forward.x);      //เปลี่ยนตำแหน่งมุมมองใหม่เมื่อตัวละครหันกลับมา
            float v = Input.GetAxisRaw("Vertical");
            //float j = Input.GetAxisRaw("Jump");
            float h = Input.GetAxisRaw("Horizontal");
            targetDiriction = (h * right) + (v * forward);
        }
    }
}
