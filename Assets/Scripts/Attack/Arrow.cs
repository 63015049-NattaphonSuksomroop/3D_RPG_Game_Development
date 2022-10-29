using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector3 m_target;
    public float speed;
    public float delay = 1f;
    public float startTime;


    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;
        if (m_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_target, step);
        }
        if(Time.time - startTime >= delay)
        {
            Destroy(gameObject);
        }
    }
    public void setTarget(Vector3 target)
    {
        m_target = target;
    }
}
