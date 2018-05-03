using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Scene state")]
    public int backScene;
    public int currentScene;
    public int nextScene;
    public int managerScene = 0;
    public int titleScene = 2;
    public int sceneCountInBuildSettings;
    [Header("Load parameters")]
    AsyncOperation loadAsync = null;
    //AsyncOperation unloadAsync = null;
    int loadingSceneIndex;
    bool isLoading = false;
    [Header("UI")]
    public Image blackScreen;
    public Image loadImg;
    float fadeTime = 2.0f;
    float rotationImg = 0;
    bool rotation = false;
    
    private void Start()
    {
        blackScreen.color = Color.black;
        FadeIn();

        if(SceneManager.sceneCount >= 2) SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        else if(SceneManager.sceneCount == 1) StartLoad(1);

        UpdateSceneState();
    }
    void UpdateSceneState()
    {
        sceneCountInBuildSettings = SceneManager.sceneCountInBuildSettings;

        currentScene = SceneManager.GetActiveScene().buildIndex;

        if(currentScene - 1 <= managerScene) backScene = sceneCountInBuildSettings - 1;
        else backScene = currentScene - 1;

        if(currentScene + 1 >= sceneCountInBuildSettings) nextScene = managerScene + 1;
        else nextScene = currentScene + 1;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.AltGr))
        {
            if(Input.GetKeyDown(KeyCode.N)) LoadNext();
            if(Input.GetKeyDown(KeyCode.B)) StartLoad(backScene);
            if(Input.GetKeyDown(KeyCode.M)) StartLoad(titleScene);
            if(Input.GetKeyDown(KeyCode.R)) StartLoad(currentScene);
        }
        if(rotation == true)
        {
            rotationImg -=2;
            loadImg.transform.rotation = Quaternion.Euler(0, 0, rotationImg);
        }
    }

    public void LoadNext() { StartLoad(nextScene); }
    public void LoadBack() { StartLoad(backScene); }
    public void LoadMenu() { StartLoad(titleScene); }
    public void LoadKinematic() { StartLoad(3); }
    public void LoadGamePlay() { StartLoad(4); }
    public void LoadSceneIndex(int index) { StartLoad(index); }
    public void ExitGame()
    {
        FadeOutExit();
    }

    void StartLoad(int index)
    {
        TextData.ResetUIText();

        Cursor.visible = false;

        if (isLoading)
        {
            Debug.LogError("No se puede cargar mas de una escena al mismo tiempo");
            return;
        }

        isLoading = true;
        loadingSceneIndex = index;

        FadeOut();
    }

    void LoadLevel()
    {
        //if (currentScene != managerScene) unloadAsync = SceneManager.UnloadSceneAsync(currentScene);
        loadAsync = SceneManager.LoadSceneAsync(loadingSceneIndex, LoadSceneMode.Single);

        StartCoroutine(Loading());
    }

    IEnumerator WaitForFade()
    {
        yield return new WaitForSecondsRealtime(fadeTime);
        LoadLevel();
    }

    IEnumerator WaitForExit()
    {
        yield return new WaitForSecondsRealtime(fadeTime);
        QuitGame();
    }
    IEnumerator Loading()
    {
        while(true)
        {
            if(loadAsync.isDone )//&& (unloadAsync == null || unloadAsync.isDone))
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(loadingSceneIndex));
                UpdateSceneState();

                loadAsync = null;
                //unloadAsync = null;

                FadeIn();

                Time.timeScale = 1;

                isLoading = false;
                break;
            }

            yield return null;
        }
    }

    void QuitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit();
        return;
    }

    void FadeIn()
    {
        rotation = false;
        blackScreen.CrossFadeAlpha(0, fadeTime, true);
        loadImg.CrossFadeAlpha(0, fadeTime/4, true);
        loadImg.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    void FadeOut()
    {
        rotation = true;
        blackScreen.CrossFadeAlpha(1, fadeTime, true);
        loadImg.CrossFadeAlpha(1, fadeTime*4, true);
        StartCoroutine(WaitForFade());
    }

    void FadeOutExit()
    {
        blackScreen.CrossFadeAlpha(1, fadeTime, true);
        Debug.Log("ToExit");
        StartCoroutine(WaitForExit());
    }
}
