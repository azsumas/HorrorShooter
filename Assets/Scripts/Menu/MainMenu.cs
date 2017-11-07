using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public InputField userNameInput;
    public string userName;
    public YourName setName;

    public void SaveUsername(string newName)
    {
        userName = newName;
        setName.Welcome();
    }
}