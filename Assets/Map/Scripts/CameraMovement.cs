using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const float SMOOTH_TIME = 0.3f;
    public bool LockX, LockY, LockZ;
    public float offSetZ = -1f;
    public bool useSmoothing = true;
    public Transform target;
    //Player
    private Transform thisTransform;
    //Camera
    private Vector3 velocity; //การเคลื่อนที่ของกล้อง 

    void Awake()
    {
        thisTransform = transform;
        velocity = new Vector3(0.5f, 0.5f, 0.5f);
    }

    void Update()
    {
        var newPos = Vector3.zero;
        if (useSmoothing)
        {
            //smooth
            newPos.x = Mathf.SmoothDamp(thisTransform.position.x, target.position.x, ref velocity.x, SMOOTH_TIME);
            newPos.y = Mathf.SmoothDamp(thisTransform.position.y, target.position.y, ref velocity.y, SMOOTH_TIME);
            newPos.z = Mathf.SmoothDamp(thisTransform.position.z, target.position.z + offSetZ, ref velocity.z, SMOOTH_TIME);
        }
        else
        {
            newPos.x = target.position.x;
            newPos.x = target.position.y;
            newPos.x = target.position.z ;
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
        transform.position = Vector3.Slerp(thisTransform.position, newPos, Time.time);
    }
}
