using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightEnd : MonoBehaviour {

    public Light myLight;

    public bool changeIntesity = false;
    public float intesitySpeed = 1.0f;
    public float maxIntesity = 10.0f;

    float startTime;

    void Start()
    {
        myLight = GetComponent<Light>();
        startTime = Time.time;
    }

    void Update()
    {
        if(changeIntesity)
        {
            myLight.intensity = Mathf.PingPong(Time.time * intesitySpeed, maxIntesity);
        }
    }
}
