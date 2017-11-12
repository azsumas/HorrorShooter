using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeScreenMain : MonoBehaviour
{
    public GameObject title;
    public GameObject main;

    // Use this for initialization
    void Start ()
    {
        title.SetActive(true);
        main.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.anyKey)
        {
           title.SetActive(false);
            main.SetActive(true);
        }
	}
}
