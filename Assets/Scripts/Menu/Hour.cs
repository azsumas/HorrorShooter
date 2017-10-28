using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hour : MonoBehaviour
{
    public Text time;
    string hour;

    void FixedUpdate()
    {
        time.text = System.DateTime.Now.ToString("HH:mm:ss dd/MM/" + "2271");
    }
}
