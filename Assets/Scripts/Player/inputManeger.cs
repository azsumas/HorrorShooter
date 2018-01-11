using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManeger : MonoBehaviour
{
    public OpenDoorButton door;
    public LaserGun gun;
    private Vector2 inputAxis;
    private PlayerBehaviour player;
	public LanternFunctions lantern;
    private Vector2 mouseAxis;
    public float sensitivity = 3;
    private CameraBehaviour cameraBehaviour;
    private MouseCursor mouse;
    public PauseManager manager;
    bool stealthy;

    [Header("SoundEffects")]
    public AudioSource breathSound;
    public AudioSource reload;
	// Use this for initialization
	void Start ()
    {
        player = GetComponent<PlayerBehaviour>();
        cameraBehaviour = GetComponentInChildren<CameraBehaviour>();
        mouse = GetComponent<MouseCursor>();
        stealthy = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetButtonDown("Pause")) manager.PauseGame();
        if (Input.GetKeyDown(KeyCode.F10)) player.SetGodMode();

        if(player.death == false)
        {

            if(!PauseManager.Instance.Pause )
            {
                inputAxis.x = Input.GetAxis("Horizontal");
                player.SetHorizontalAxis(inputAxis.x);
                inputAxis.y = Input.GetAxis("Vertical");
                player.SetVerticalAxis(inputAxis.y);

                //if(Input.GetKeyDown(KeyCode.Q)) player.ReceivedDamage(10);
                if(Input.GetKeyDown(KeyCode.Q)) player.RecoveryEnergy();

                if (Input.GetKeyDown(KeyCode.R))
                {
                    reload.Play();
                    gun.Reload();
                }

                if(Input.GetButtonDown("Jump"))
                {
                    player.Jump();
                }

                if(Input.GetButtonDown("Run"))
                {
                    breathSound.Play();
                    player.Run();
                }

                if(Input.GetButtonUp("Run") || Input.GetButtonUp("Walk"))
                {
                    breathSound.Stop();
                    player.Walk();
                }

                if(Input.GetButton("Aim"))
                {
                    player.AimPlayer();
                }
                else player.NoAimPlayer();

                if(Input.GetButtonDown("Walk"))
                {
                    stealthy = true;
                    if(stealthy == true)
                    {
                        player.SlowStep();
                    }
                }
                else
                {
                    stealthy = false;
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

                if(Input.GetButtonDown("ActiveLantern"))
                {
                    lantern.SwitchOn();
                }

                if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

                //mouse
                mouseAxis.x = Input.GetAxis("Mouse X") * sensitivity;
                mouseAxis.y = Input.GetAxis("Mouse Y") * sensitivity;
                cameraBehaviour.SetRotationX(mouseAxis.y);
                cameraBehaviour.SetRotationY(mouseAxis.x);

                if(Input.GetButtonDown("Cancel")) mouse.Show();
                if(Input.GetMouseButtonDown(0)) mouse.Hide();
            }
        }
    }
}
