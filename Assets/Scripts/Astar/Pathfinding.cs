using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    Grid GridReference;//Grid Class
    public Transform StartPosition;//ตำแหน่งเริ่มต้นเพื่อค้นหาเส้นทางจาก Enemy
    public Transform TargetPosition;//ตำแหน่งเริ่มต้นเพื่อค้นหาเส้นทางสู่ Player

    private void Awake()//เมื่อโปรแกรมเริ่ม
    {
        GridReference = GetComponent<Grid>();//GetComponent Grid
    }

    private void Update()//ทุกเฟรม
    {
        FindPath(StartPosition.position, TargetPosition.position);//ค้นหา path ไป goal
    }

    void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = GridReference.NodeFromWorldPoint(a_StartPos);//รับโหนด Node ที่ใกล้กับตำแหน่งเริ่มต้นมากที่สุด
        Node TargetNode = GridReference.NodeFromWorldPoint(a_TargetPos);//รับโหนด Node ที่ใกล้กับตำแหน่งเป้าหมายมากที่สุด

        List<Node> OpenList = new List<Node>();//เปิด List nodes ที่เก็บใน grid
        HashSet<Node> ClosedList = new HashSet<Node>();//closed list

        OpenList.Add(StartNode);//เพิ่มโหนดเริ่มต้นในรายการที่เปิดอยู่เพื่อเริ่มโปรแกรม

        while (OpenList.Count > 0)//ในขณะที่ OpenList
        {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)//Loop
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ihCost < CurrentNode.ihCost)//ถ้าค่า f ของวัตถุนั้นน้อยกว่าหรือเท่ากับต้นทุน f ของโหนดปัจจุบัน
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);//ลบออกจาก OpenList
            ClosedList.Add(CurrentNode);//เพิ่ม closed list

            if (CurrentNode == TargetNode)//ถ้าโหนด Node ปัจจุบันเหมือนกับโหนดเป้าหมาย Target
            {
                GetFinalPath(StartNode, TargetNode);//คำนวณเส้นทางสุดท้าย
            }

            foreach (Node NeighborNode in GridReference.GetNeighboringNodes(CurrentNode))//Loop
            {
                if (!NeighborNode.bIsWall || ClosedList.Contains(NeighborNode))//ถ้าเป็นกำแพง wall หรือ check แล้ว
                {
                    continue;//Skip it
                }
                int MoveCost = CurrentNode.igCost + GetManhattenDistance(CurrentNode, NeighborNode);//จะรับค่า F

                if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode))//ถ้าค่า f มากกว่าค่า g ใน open list
                {
                    NeighborNode.igCost = MoveCost;//Set ค่า g cost เป็น f cost
                    NeighborNode.ihCost = GetManhattenDistance(NeighborNode, TargetNode);//Set ค่า h cost
                    NeighborNode.ParentNode = CurrentNode;//retracing steps ย้อนขั้นตอน

                    if(!OpenList.Contains(NeighborNode))//ถ้าไม่มีใน openlist
                    {
                        OpenList.Add(NeighborNode);//จะเพิ่ม list
                    }
                }
            }

        }
    }



    void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();//List จะ hold เส้นทางตามลำดับ
        Node CurrentNode = a_EndNode;//โหนดสำหรับเก็บโหนดปัจจุบันที่กำลังตรวจสอบchecked

        while (CurrentNode != a_StartingNode)//loop
        {
            FinalPath.Add(CurrentNode);//เพิ่มโหนดนั้นไปยังเส้นทางสุดท้าย final path
            CurrentNode = CurrentNode.ParentNode;//ย้ายไปยังโหนดหลัก parent node
        }

        FinalPath.Reverse();//ย้อนกลับเส้นทางเพื่อรับลำดับที่ถูกต้อง Reverse path

        GridReference.FinalPath = FinalPath;//กำหนดเส้นทางสุดท้าย

    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.iGridX - a_nodeB.iGridX);//x1-x2
        int iy = Mathf.Abs(a_nodeA.iGridY - a_nodeB.iGridY);//y1-y2

        return ix + iy;//Return sum
    }
}
