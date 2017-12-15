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
        if(dead == 1) DeadText();
        else WinText(); 
    }

    void DeadText()
    {
        dieText.SetActive(true);
    }

    void WinText()
    {
        winText.SetActive(true);
    }
}
