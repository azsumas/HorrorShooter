using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionEasing : MonoBehaviour
{
    public float iniPos;
    public float finalPos;
    public float currentTime;
    public float duration;

    // Update is called once per frame
    public void Start()
    {
        transform.position = new Vector3(transform.position.x, iniPos, transform.position.z);
    }

    void Update ()
    {
        if(currentTime >= 0)
        {
            float value = Easing.BounceEaseOut(currentTime, iniPos, finalPos - iniPos, duration);
            transform.localPosition = new Vector3(transform.position.x, value, transform.position.z);
        }

        currentTime += Time.deltaTime;

        if(currentTime >= duration)
        {
            transform.localPosition = new Vector3(transform.position.x, finalPos, transform.position.z);
        }
    }
}


