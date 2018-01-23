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

    public void PauseGame()
    {
        paused = !paused;
        if (paused) PauseOn();
        else if (!paused) PauseOff();
    }

    public void PauseOn()
    {
        Time.timeScale = 0;
        hud.SetActive(false);
        pauseCanvas.SetActive(true);
        Cursor.visible = true;
    }

    public void PauseOff()
    {
        hud.SetActive(true);
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
    }
}
