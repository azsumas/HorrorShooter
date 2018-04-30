using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject textInitialization;
    public GameObject player;
    private inputManeger controlPlayer;
    public GameObject ammoText;
    public GameObject packEneText;
    public GameObject energyBar;

    public float timeCounter;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindWithTag("Player");
        controlPlayer = player.GetComponentInChildren<inputManeger>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        timeCounter += Time.deltaTime;
        if(timeCounter >= 7.8f)
        controlPlayer.enabled = true;
	}
}
