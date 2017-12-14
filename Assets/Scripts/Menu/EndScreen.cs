using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public GameObject dieText;
    public GameObject winText;
    public int dead;

    // Use this for initialization
    void Start ()
    {
        dead = PlayerPrefs.GetInt("Death");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(dead == 1) dieText.SetActive(true);
        else winText.SetActive(true);
	}
}
