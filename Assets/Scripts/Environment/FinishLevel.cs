using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    public GameObject managerScene;
    private LevelManager script;

    private void Start()
    {
        managerScene = GameObject.FindWithTag("Manager");
        script = managerScene.GetComponent<LevelManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        script.LoadNext();
	}
}
