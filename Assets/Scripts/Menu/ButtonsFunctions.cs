using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsFunctions : MonoBehaviour
{
    public GameObject managerScene;
    private LevelManager script;

    private void Start()
    {
        Cursor.visible = true;
        managerScene = GameObject.FindWithTag("Manager");
        script = managerScene.GetComponent<LevelManager>();
    }

    public void Update()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void NewGame()
    {
        Cursor.visible = false;
        script.LoadNext();
    }

    public void TryAgain()
    {
        script.LoadGamePlay();
    }

    public void MainGame()
    {
        script.LoadMenu();
    }

    public void QuitGame()
    {
        Cursor.visible = false;
        script.ExitGame();
    }
}
