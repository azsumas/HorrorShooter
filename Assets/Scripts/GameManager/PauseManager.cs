using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private bool paused;
    public GameObject pauseCanvas;
    public GameObject hud;

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
            hud.SetActive(false);
            pauseCanvas.SetActive(true);
            Cursor.visible = true;
        }

        else if (!paused)
        {
            hud.SetActive(true);
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
            Cursor.visible = false;
        }
    }

    public void PauseGame()
    {
        paused = !paused;
    }
}
