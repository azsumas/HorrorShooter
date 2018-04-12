using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematic : MonoBehaviour
{
    [Header("ChangeScene")]
    public float timeToChangeScreen;
    public bool loadNext = false;
    [Header("EasingAtributes")]
    public float currentTime;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey) ChangeScene();

        if (currentTime >= timeToChangeScreen) ChangeScene();

        currentTime += Time.deltaTime;
    }

    void ChangeScene()
    {
        if (loadNext == true) return;
        GameManager.instance.LoadGame();
        loadNext = true;
    }
}
