using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    public GameObject managerScene;
    private LevelManager script;
    public float timeCounter;
    public float maxTimeCounter;
    bool loadNext = false;

    // Use this for initialization
    void Start ()
    {
        managerScene = GameObject.FindWithTag("Manager");
        script = managerScene.GetComponent<LevelManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		timeCounter += Time.deltaTime;

        if (timeCounter >= maxTimeCounter) ChangeScene();
	}

    void ChangeScene()
    {
        if (loadNext == true) return;
        script.LoadMenu();
        loadNext = true;
    }
}
