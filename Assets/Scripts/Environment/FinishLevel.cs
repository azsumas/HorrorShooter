using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    public GameObject managerScene;
    private LevelManager script;

    public Light myLight;

    public bool changeIntesity = false;
    public float intesitySpeed = 1.0f;
    public float maxIntesity = 10.0f;

    float startTime;

    private void Start()
    {
        managerScene = GameObject.FindWithTag("Manager");
        script = managerScene.GetComponent<LevelManager>();

        myLight = GetComponent<Light>();
        startTime = Time.time;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            changeIntesity = true;
            script.LoadNext();
        }

        if(changeIntesity)
        {
            myLight.intensity = Mathf.PingPong(Time.time * intesitySpeed, maxIntesity);
        }
    }
}
