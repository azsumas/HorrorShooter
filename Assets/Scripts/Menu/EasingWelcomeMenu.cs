using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasingWelcomeMenu : MonoBehaviour
{
    public float currentTime;
    public float duration;
    public Text welcome;
    public float alpha;

    // Update is called once per frame
    public void Start()
    {
        alpha = 0;
    }

    void Update()
    {
        if (currentTime >= 0)
        {
            welcome.color = new Vector4(255f, 255f, 255f, alpha);
        }

        currentTime += Time.deltaTime;
        alpha += Time.deltaTime / 6;

        if (currentTime >= duration)
        {
            welcome.color = new Vector4(255f, 255f, 255f, alpha);
        }
    }
}