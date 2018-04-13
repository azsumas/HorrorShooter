using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(AudioSource))]

public class Kinematic : MonoBehaviour
{
    [Header("ChangeScene")]
    public float timeToChangeScreen;
    public bool loadNext = false;
    public float currentTime;
    [Header("Resources")]
    public MovieTexture kinSpa;
    public MovieTexture kinEng;
    private AudioSource audioMov;

    private void Start()
    {
        if (Language.language == Language.Lang.esES)
        {
            GetComponent<RawImage>().texture = kinSpa as MovieTexture;
            audioMov = GetComponent<AudioSource>();
            audioMov.clip = kinSpa.audioClip;
            kinSpa.Play();
            audioMov.Play();
        }
        if (Language.language == Language.Lang.enUS)
        {
            GetComponent<RawImage>().texture = kinEng as MovieTexture;
            audioMov = GetComponent<AudioSource>();
            audioMov.clip = kinEng.audioClip;
            kinEng.Play();
            audioMov.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            if (Language.language == Language.Lang.esES)
            {
                kinSpa.Stop();
                audioMov.Stop();
            }
            if (Language.language == Language.Lang.enUS)
            {
                kinEng.Stop();
                audioMov.Stop();
            }
            ChangeScene();
        }

        if (currentTime >= timeToChangeScreen) ChangeScene();

        currentTime += Time.deltaTime;
    }

    void ChangeScene()
    {
        if (loadNext == true) return;
        GameManager.instance.LoadGame();
        loadNext = true;
    }
}
