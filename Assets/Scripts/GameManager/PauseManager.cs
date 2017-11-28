using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool paused;

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
            Time.timeScale = 0;
        }
        if(!paused)
        {
            Time.timeScale = 1;
        }
    }

    public void PauseGame()
    {
        paused = !paused;
    }
}
