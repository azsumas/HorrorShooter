using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    LevelManager levelManager;
    public DataLogic dataLogic;

    void Awake()
    {
        Debug.Log("Game manager awake");
        if(instance == null) instance = this;
        else if(instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        levelManager = GetComponent<LevelManager>();

        DontDestroyOnLoad(this.gameObject);

        Debug.Log("Game manager start");
        AudioManager.Initialize();
        Language.Initialize();
        dataLogic = new DataLogic();
        dataLogic.InitData(); 
    }

    public void LoadMenu()
    {
        levelManager.LoadMenu();
    }

    public void NewGame()
    {
        levelManager.LoadKinematic();
        dataLogic.NewGameState();
    }

    public void ExitGame()
    {
        levelManager.ExitGame();
    }

    //Datalogic functions
    public void SaveGame()
    {
        dataLogic.SaveState();
    }

    public void LoadGame()
    {
        levelManager.LoadGamePlay();
        dataLogic.LoadState();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            SaveGame();
        }
    }

    public void SpanishLanguage()
    {
        Language.SetLanguage(Language.Lang.esES);
        Debug.Log("1");
    }

    public void EnglishLanguage()
    {
        Language.SetLanguage(Language.Lang.enUS);
        Debug.Log("2");
    }
}
