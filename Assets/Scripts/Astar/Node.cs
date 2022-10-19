using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public int iGridX;//ตำแหน่ง X ใน Node Array
    public int iGridY;//ตำแหน่ง Y ใน Node Array

    public bool bIsWall;//บอกโหนดนี้ถูกขัดขวางไหม
    public Vector3 vPosition;//คำแหน่ง Node

    public Node ParentNode;//A* algoritm,จะเก็บโหนดที่มาจากก่อนหน้านี้และตามเส้นทางที่สั้นที่สุดหรือ shortest path.

    public int igCost;//ค่า cost
    public int ihCost;//ระยะทางจากEnemy ถึง Player หรือจาก node ไป goal

    public int FCost { get { return igCost + ihCost; } }

    public Node(bool a_bIsWall, Vector3 a_vPos, int a_igridX, int a_igridY)
    {
        bIsWall = a_bIsWall;
        vPosition = a_vPos;
        iGridX = a_igridX;
        iGridY = a_igridY;
    }

}
