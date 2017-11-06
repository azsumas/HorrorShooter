using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManeger : MonoBehaviour
{
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

		if (Input.GetButtonDown("Jump"))
        {
            player.Jump();
        }

		if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.Run();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftControl))
        {
            player.Walk();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            player.SlowStep();
        }

        /*if (Input.GetKeyUp(KeyCode.Q))
        {
            player.ReceivedDamage();
        }*/

		if (Input.GetKeyUp(KeyCode.T))
		{
			lantern.SwitchOn ();
		}

        //mouse
        mouseAxis.x = Input.GetAxis("Mouse X") * sensitivity;
        mouseAxis.y = Input.GetAxis("Mouse Y") * sensitivity;
        cameraBehaviour.SetRotationX(mouseAxis.y);
        cameraBehaviour.SetRotationY(mouseAxis.x);

        if (Input.GetButtonDown("Cancel")) mouse.Show();
        if (Input.GetMouseButtonDown(0)) mouse.Hide();
    }
}
