using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleAnimation : MonoBehaviour
{
    [Header("ChangeScene")]
    public float timeToChangeScreen;
    public bool loadNext = false;
    [Header("EasingAtributes")]
    public Vector2 iniPos;
    public Vector2 finalPos;
    public float currentTime;
    public float duration;
    float alpha;
    public Image logo;
    public AudioSource logoSound;
    bool startMusic = false;

    public void Start()
    {
        transform.localScale = new Vector3(iniPos.x, iniPos.y, transform.localScale.z);
        alpha = 0;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.anyKey) ChangeScene();

        if (loadNext == true) logoSound.volume -= Time.deltaTime/4;

        if (currentTime >= timeToChangeScreen) ChangeScene();

        if (currentTime >= 0)
        {
            PlayMusicLogo();
            Vector2 value = new Vector2(Easing.QuintEaseIn(currentTime, iniPos.x, finalPos.x - iniPos.x, duration), Easing.QuintEaseIn(currentTime, iniPos.y, finalPos.y - iniPos.y, duration));
            transform.localScale = new Vector3(value.x, value.y, transform.localScale.z);
            logo.color = new Vector4(255f, 255f, 255f, alpha);
        }

        currentTime += Time.deltaTime;
        alpha += Time.deltaTime / 6;

        if (currentTime >= duration)
        {
            transform.localScale = new Vector3(finalPos.x, finalPos.y, transform.localScale.z);
            logo.color = new Vector4(255f, 255f, 255f, 1);
        }
    }

    void PlayMusicLogo()
    {
        if (startMusic == true) return;
        logoSound.Play();
        startMusic = true;
    }

    void ChangeScene()
    {
        if (loadNext == true) return;
        GameManager.instance.LoadMenu();
        loadNext = true;
    }
}
