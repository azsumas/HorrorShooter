using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePad : MonoBehaviour
{
    private bool gamePad = false;
    public GameObject text;

    // Update is called once per frame
    void Update ()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            print(names[x].Length);
            if (names[x].Length == 0)
            {
                print("NOTHING IS CONNECTED");
                KeyBoardController();
            }
            if (names[x].Length == 19)
            {
                print("PS4 CONTROLLER IS CONNECTED");
                PlayStationController();
            }
            if (names[x].Length == 33 || names[x].Length == 20)
            {
                print("XBOX ONE CONTROLLER IS CONNECTED");
                XboxController();
            }
        }
    }

    public void KeyBoardController()
    {
        if (gamePad == false) return;
        text.SetActive(true);
        gamePad = false;
    }

    public void PlayStationController()
    {
        if (gamePad == true) return;
        Debug.Log("PS4 GAMEPAD MAN!");
        gamePad = true;
    }

    public void XboxController()
    {
        if (gamePad == true) return;
        text.SetActive(false);
        gamePad = true;
    }
}
