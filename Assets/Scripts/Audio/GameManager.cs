using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    LevelManager levelManager;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        levelManager = GetComponent<LevelManager>();
        AudioManager.Initialize();
    }

    public void LoadMenu()
    {
        levelManager.LoadMenu();
    }
}
