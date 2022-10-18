using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    Transform target;
    NavMeshAgent nav;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    //void Update()
    //{
        //nav.destination = target.position;
    //}
    public LayerMask hitLayers;
    void Update()
    {
        {
            nav.destination = target.position;
        }
        /*if (Input.GetMouseButtonDown(0))//If the player has left clicked
        {
            Vector3 mouse = Input.mousePosition;//Get the mouse Position
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);//Cast a ray to get where the mouse is pointing at
            RaycastHit hit;//Stores the position where the ray hit.
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))//If the raycast doesnt hit a wall
            {
                this.transform.position = hit.point;//Move the target to the mouse position
            }
        }*/
    }
}
