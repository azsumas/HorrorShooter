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
        managerScene = GameObject.FindWithTag("Manager");
        script = managerScene.GetComponent<LevelManager>();
    }

    public void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void NewGame()
    {
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
        script.ExitGame();
    }
}
