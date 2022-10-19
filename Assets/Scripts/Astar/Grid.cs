using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public Transform StartPosition;//เริ่ม pathfinding A*
    public LayerMask WallMask;//ค้นหาสิ่งกีดขวางของเส้นทาง WallMask ด้วย LayerMask
    public Vector2 vGridWorldSize;//vector2 เพื่อเก็บความกว้างและความสูงของกราฟในหน่วยโลก
    public float fNodeRadius;//เก็บข้อมูลว่าแต่ละ each square บนกราฟจะมีขนาดเท่าไหร่
    public float fDistanceBetweenNodes;//ระยะทางที่ square ระหว่าง Nodes

    Node[,] NodeArray;//อาร์เรย์ของโหนดที่อัลกอริทึม A* ใช้
    public List<Node> FinalPath;//เส้นทางที่เสร็จสมบูรณ์ที่เส้นสีแดงจะถูกลากไปตาม Node FinalPath


    float fNodeDiameter;//รัศมีสองเท่า
    int iGridSizeX, iGridSizeY;//ขนาดของ Grid ในอาเรย์


    private void Start()//Running เมื่อโปรแกรมเริ่มทำงาน
    {
        fNodeDiameter = fNodeRadius * 2;//เพิ่มรัศมีเป็นสองเท่าเพื่อให้ได้เส้นผ่านศูนย์กลาง
        iGridSizeX = Mathf.RoundToInt(vGridWorldSize.x / fNodeDiameter);//แบ่งพิกัดโลกของ Grid ด้วยเส้นผ่านศูนย์กลางเพื่อให้ได้ขนาดของกราฟในหน่วยอาร์เรย์ x
        iGridSizeY = Mathf.RoundToInt(vGridWorldSize.y / fNodeDiameter);//แบ่งพิกัดโลกของ Grid ด้วยเส้นผ่านศูนย์กลางเพื่อให้ได้ขนาดของกราฟในหน่วยอาร์เรย์ y
        CreateGrid();//Draw the grid
    }

    void CreateGrid()
    {
        NodeArray = new Node[iGridSizeX, iGridSizeY];//สร้างอาร์เรย์ของโหนด
        Vector3 bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.forward * vGridWorldSize.y / 2;//รับตำแหน่งจริง grid.
        for (int x = 0; x < iGridSizeX; x++)//วนรอบอาร์เรย์ของโหนด
        {
            for (int y = 0; y < iGridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);//รับพิกัด graph
                bool Wall = true;//ทำให้โหนดเป็นกำแพงจริง

                if (Physics.CheckSphere(worldPoint, fNodeRadius, WallMask)) //ถ้าโหนดไม่ถูกกีดขวาง ตรวจสอบการชนกันกับโหนดปัจจุบันและทุกสิ่งในโลกที่ตำแหน่ง หากชนกับวัตถุด้วย WallMask
                {
                    Wall = false;//Object wall คำสั่ง if จะคืนค่าเป็นเท็จ
                }

                NodeArray[x, y] = new Node(Wall, worldPoint, x, y);//สร้างโหนดใหม่ในอาร์เรย์
            }
        }
    }

    //ฟังก์ชันที่รับโหนดข้างเคียงของโหนดที่กำหนด
    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();//สร้าง new list node
        int icheckX;//ตัวแปรเพื่อตรวจสอบว่า XPosition อยู่ภายในช่วงของโหนดอาร์เรย์หรือไม่
        int icheckY;//ตัวแปรเพื่อตรวจสอบว่า YPosition อยู่ภายในช่วงของโหนดอาร์เรย์หรือไม่

        //ตรวจสอบด้านขวาของโหนดปัจจุบัน
        icheckX = a_NeighborNode.iGridX + 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < iGridSizeX)//ถ้า XPosition อยู่ในช่วงของอาร์เรย์
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)//ถ้า YPosition อยู่ในช่วงของอาร์เรย์
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);//เพิ่ม grid ไปยัง list
            }
        }
        //ตรวจสอบด้านซ้ายของโหนดปัจจุบัน
        icheckX = a_NeighborNode.iGridX - 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < iGridSizeX)//ถ้า XPosition อยู่ในช่วงของอาร์เรย์
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)//ถ้า YPosition อยู่ในช่วงของอาร์เรย์
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);//เพิ่ม grid ไปยัง list
            }
        }
        //ตรวจสอบด้านบนของโหนดปัจจุบัน
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY + 1;
        if (icheckX >= 0 && icheckX < iGridSizeX)//ถ้า XPosition อยู่ในช่วงของอาร์เรย์
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)//ถ้า YPosition อยู่ในช่วงของอาร์เรย์
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);//เพิ่ม grid ไปยัง list
            }
        }
        //ตรวจสอบด้านล่างของโหนดปัจจุบัน
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY - 1;
        if (icheckX >= 0 && icheckX < iGridSizeX)//หาก XPosition อยู่ในช่วงของอาร์เรย์
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)//ถ้า YPosition อยู่ในช่วงของอาร์เรย์
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);//เพิ่ม grid ไปยัง list
            }
        }

        return NeighborList;//Return list.
    }

    //รับโหนดที่ใกล้เคียงที่สุดกับตำแหน่งโลกที่กำหนด
    public Node NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float ixPos = ((a_vWorldPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x);
        float iyPos = ((a_vWorldPos.z + vGridWorldSize.y / 2) / vGridWorldSize.y);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);

        return NodeArray[ix, iy];
    }


    //ฟังก์ชัน wireframe
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireCube(transform.position, new Vector3(vGridWorldSize.x, 1, vGridWorldSize.y));//วาดเส้นที่มีขนาดที่กำหนดจากตัวตรวจสอบ Unity

        if (NodeArray != null)//ถ้า grid ไม่ empty
        {
            foreach (Node n in NodeArray)//วนซ้ำทุกโหนดใน grid
            {
                if (n.bIsWall)//ถ้าโหนดปัจจุบันเป็น wall
                {
                    Gizmos.color = Color.white;//กำหนดสีของโหนดเป็นสีขาว
                }
                else
                {
                    Gizmos.color = Color.yellow;//กำหนดสีของโหนดเป็นสีเหลือง
                }


                if (FinalPath != null)//ถ้าเส้นทางสุดท้ายไม่ว่างเปล่า
                {
                    if (FinalPath.Contains(n))//หากโหนดปัจจุบันอยู่ในเส้นทางสุดท้าย final path
                    {
                        Gizmos.color = Color.red;//กำหนดสีของโหนดนั้นเป็นสีแดง
                    }

                }


                Gizmos.DrawCube(n.vPosition, Vector3.one * (fNodeDiameter - fDistanceBetweenNodes));//วาดโหนดที่ตำแหน่งของโหนด Node
            }
        }
    }
}
