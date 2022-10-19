using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    public LayerMask hitLayers;
    void Update()
    {
        //Scripts ไว้เช็คจุด Target Player หรือ ทดสอบ เป็น Enemyก็ได้
        if (Input.GetMouseButtonDown(0))//ถ้า Player คลิกขวา
        {
            Vector3 mouse = Input.mousePosition;//จะรับตำแหน่งเมาส์
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);//Cast ray เพื่อรับจุดตำแหน่งของเมาส์ที่เปลี่ยน
            RaycastHit hit;//เก็บตำแหน่งที่ Raycast กระทบ
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))//ถ้า raycast ไม่ชนกำแพง wall
            {
                this.transform.position = hit.point;//ย้ายเป้าหมาย Player ไปที่ตำแหน่งเมาส์
            }
        }
    }
}

