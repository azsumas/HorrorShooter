using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public Text pauseText;
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
            pauseText.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else if(!paused)
        {
            pauseText.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void PauseGame()
    {
        paused = !paused;
    }
}
