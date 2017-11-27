using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManeger : MonoBehaviour
{
    public OpenDoorButton door;
    private Vector2 inputAxis;
    private PlayerBehaviour player;
	public LanternFunctions lantern;
    private Vector2 mouseAxis;
    public float sensitivity = 3;
    private CameraBehaviour cameraBehaviour;
    private MouseCursor mouse;


	// Use this for initialization
	void Start ()
    {
        player = GetComponent<PlayerBehaviour>();
        cameraBehaviour = GetComponentInChildren<CameraBehaviour>();
        mouse = GetComponent<MouseCursor>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        inputAxis.x = Input.GetAxis("Horizontal");
        player.SetHorizontalAxis(inputAxis.x);
        inputAxis.y = Input.GetAxis("Vertical");
        player.SetVerticalAxis(inputAxis.y);

        if (Input.GetKeyDown(KeyCode.Q)) player.ReceivedDamage(5);

        //if(Input.GetButtonDown("Pause")

        if (Input.GetButtonDown("Jump"))
        {
            player.Jump();
        }

		if (Input.GetButtonDown("Run"))
        {
            player.Run();
        }

        if (Input.GetButtonUp("Run") || Input.GetButtonUp("Walk"))
        {
            player.Walk();
        }

        if (Input.GetButtonDown("Walk"))
        {
            player.SlowStep();
        }

        /*if (Input.GetButtonDown("ActiveDoor"))
        {
            Debug.Log("YES");
            door.opening = true;
        }
        else if (Input.GetButtonUp("ActiveDoor"))
        {
            Debug.Log("NO");
            door.opening = false;
        }*/

        if (Input.GetButtonDown("ActiveLantern"))
		{
			lantern.SwitchOn ();
		}

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        //mouse
        mouseAxis.x = Input.GetAxis("Mouse X") * sensitivity;
        mouseAxis.y = Input.GetAxis("Mouse Y") * sensitivity;
        cameraBehaviour.SetRotationX(mouseAxis.y);
        cameraBehaviour.SetRotationY(mouseAxis.x);

        if (Input.GetButtonDown("Cancel")) mouse.Show();
        if (Input.GetMouseButtonDown(0)) mouse.Hide();
    }
}
