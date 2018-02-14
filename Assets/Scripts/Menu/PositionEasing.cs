using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionEasing : MonoBehaviour
{
    public Vector2 iniPos;
    public Vector2 finalPos;
    public float currentTime;
    public float duration;

    // Update is called once per frame
    public void Start()
    {
        transform.localPosition = new Vector3(iniPos.x, iniPos.y, transform.position.z);
    }

    void Update ()
    {
        if(currentTime >= 0)
        {
            Vector2 value = new Vector2(Easing.BounceEaseOut(currentTime, iniPos.x, finalPos.x - iniPos.x, duration), Easing.BounceEaseOut(currentTime, iniPos.y, finalPos.y - iniPos.y, duration));
            transform.localPosition = new Vector3(value.x, value.y, transform.position.z);
        }

        currentTime += Time.deltaTime;

        if(currentTime >= duration)
        {
            transform.localPosition = new Vector3(finalPos.x, finalPos.y, transform.position.z);
        }
    }
}


