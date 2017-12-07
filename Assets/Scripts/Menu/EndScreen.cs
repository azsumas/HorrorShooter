using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public PlayerBehaviour player;
    public GameObject dieText;
    public GameObject winText;

    // Use this for initialization
    void Start ()
    {
        player = GetComponent<PlayerBehaviour>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(player.death == true) dieText.SetActive(true);
        if (player.death == false) winText.SetActive(true);
	}
}
