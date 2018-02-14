using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsFunctions : MonoBehaviour
{

    private void Start()
    {
        Cursor.visible = true;
    }

    public void Update()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void NewGame()
    {
        Cursor.visible = false;
        GameManager.instance.NewGame();
    }

    public void TryAgain()
    {
        GameManager.instance.NewGame();
    }

    public void MainGame()
    {
        GameManager.instance.LoadMenu();
    }

    public void SpanishLanguageButton()
    {
        GameManager.instance.SpanishLanguage();
    }

    public void EnglishhLanguageButton()
    {
        GameManager.instance.EnglishLanguage();
    }

    public void SaveGameButton()
    {
        GameManager.instance.SaveGame();
    }

    public void LoadGameButton()
    {
        GameManager.instance.LoadGame();
    }

    public void QuitGame()
    {
        Cursor.visible = false;
        GameManager.instance.ExitGame();
    }
}
