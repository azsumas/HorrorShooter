using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour {


    public Light Light;
    public float minTimeOn;
    public float maxTimeOn;
    public float minTimeOff;
    public float maxTimeOff;
    public float minLight;
    public float maxLight;

    public float waitTimeOn;
    public float waitTimeOff;

    void Start()
    {
        StartCoroutine("Flicker");
    }

    IEnumerator Flicker()
    {
        if(Light.enabled == true)
        {
            waitTimeOn = Random.Range(minTimeOn, maxTimeOff);
            yield return new WaitForSeconds(waitTimeOn);
            Light.enabled = false;
            StartCoroutine("Flicker");
        }
        else
        {
            waitTimeOff = Random.Range(minTimeOff, maxTimeOff);
            yield return new WaitForSeconds(waitTimeOff);
            Light.intensity = Random.Range(minLight, maxLight);
            Light.enabled = true;
            StartCoroutine("Flicker");
        }
    }
}
