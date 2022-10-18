using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 0.7f;
    public float delay = 1f;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position += speed * this.gameObject.transform.forward;
        if (Time.time - startTime >= delay)
        {
            Destroy(gameObject);
        }
    }
}
