using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YourName : MonoBehaviour
{
    public Text yourName;
    public MainMenu nameMenu;

    public void Welcome()
    {
        yourName.text = "Welcome, " + nameMenu.userName + ".";
    }
}
