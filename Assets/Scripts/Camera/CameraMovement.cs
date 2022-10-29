using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private const float SMOOTH_TIME = 0.3f; //เคลื่อนที่ตามตัวละครที่วิ่ง lock พิกัด camera
    public bool LockX, LockY, LockZ;
    public float offsetZ = -2f;
    public bool useSmoothing = true;
    public Transform target; //Playertarget
    private Transform thisTransform; //Cameratarget
    private Vector3 velocity;
    void Awake()
    {
        thisTransform = transform;
        velocity = new Vector3(0.5f, 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        var newPos = Vector3.zero;
        if (useSmoothing)
        {
            newPos.x = Mathf.SmoothDamp(thisTransform.position.x, target.position.x, ref velocity.x, SMOOTH_TIME); //รับค่า3ค่า position ตัวแปร camera player smooth_Time
            newPos.y = Mathf.SmoothDamp(thisTransform.position.y, target.position.y, ref velocity.y, SMOOTH_TIME);
            newPos.z = Mathf.SmoothDamp(thisTransform.position.z, target.position.z + offsetZ, ref velocity.z, SMOOTH_TIME);
        }
        else
        {
            newPos.x = thisTransform.position.x;
            newPos.y = thisTransform.position.y;
            newPos.z = thisTransform.position.z + offsetZ;
        }
        if (LockX)
        {
            newPos.x = thisTransform.position.x;
        }
        if (LockY)
        {
            newPos.y = thisTransform.position.y;
        }
        if (LockZ)
        {
            newPos.z = thisTransform.position.z;
        }
        thisTransform.position = Vector3.Slerp(thisTransform.position,newPos, Time.time);
    }
}
