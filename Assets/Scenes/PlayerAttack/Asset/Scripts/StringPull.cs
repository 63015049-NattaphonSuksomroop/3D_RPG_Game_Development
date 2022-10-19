using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringPull : MonoBehaviour
{
    public GameObject BowString;
    public Transform StringIdlePos;
    public Transform StringPullPos;

    void stringPull()
    {
        BowString.transform.position = StringPullPos.position;
        BowString.transform.SetParent(StringPullPos);
    }

    void stringNotPull()
    {
        BowString.transform.position = StringIdlePos.position;
        BowString.transform.SetParent(StringIdlePos);
    }



}
