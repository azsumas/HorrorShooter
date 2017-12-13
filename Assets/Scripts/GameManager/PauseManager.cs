using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject mainPause;
    private bool paused;
    public Transform player;

    public bool Pause
    {
        get { return paused; }
    }
    private static PauseManager instance;

    public static PauseManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<PauseManager>();
            }
            return PauseManager.instance;
        }
    }

    void Update()
    {
        if(paused)
        {
            mainPause.gameObject.SetActive(true);
            Time.timeScale = 0;
            player.GetComponent<CharacterController>().enabled = false;
        }
        else if(!paused)
        {
            mainPause.gameObject.SetActive(false);
            Time.timeScale = 1;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }

    public void PauseGame()
    {
        paused = !paused;
    }
}
