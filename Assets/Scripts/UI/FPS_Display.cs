using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS_Display : MonoBehaviour

{
    //FPS_Display
    public TextMeshProUGUI FpsText;
    public float pollingTime = 1f;
    private float time;
    private int frameCount;

    // Update is called once per frame
    void Update()
    {
        //FPS_Display
        time += Time.deltaTime;

        frameCount++;

        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            FpsText.text = frameRate.ToString() + " FPS";

            time -= pollingTime;
            frameCount = 0;
            Debug.Log(frameRate + " Fps");
        }
    }
}