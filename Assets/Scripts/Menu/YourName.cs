using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YourName : MonoBehaviour
{
    public Text name;
    public MainMenu nameMenu;

    public void Welcome()
    {
        name.text = "Welcome, " + nameMenu.userName + ".";
    }
}
