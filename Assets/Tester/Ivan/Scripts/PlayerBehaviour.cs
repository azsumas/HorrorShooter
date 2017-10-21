using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private CharacterController controller;
    public EnergyBar lifeBar;
	[Header("Direction")]
    public Vector3 moveDirection;
	[Header("Speed")]
    float speed;
	public float run;
    public float walk;
    public float slowStep;
    public Vector2 axis;
	Vector3 desiredDirection;
	[Header("Stats Jumps")]
    public float forceToGround = Physics.gravity.y;
    public float jumpSpeed;
    public float gravitymagnitude = 1;
    [Header("Stats")]
    public bool jump;
    public bool isGrounded;
    public float hitYourself;
    [Header("Stats Player")]
    [SerializeField]
    private bool death;
    [Header("Energy Player")]
    public float maxEnergy;
    public float energy;

    // Use this for initialization
    void Start ()
    {
        controller = GetComponent<CharacterController>();
        speed = walk;
        death = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Reset states
        if (!controller.isGrounded) isGrounded = false;

        //logic
        if (isGrounded && !jump)
        {
            moveDirection.y = forceToGround;
        }
        else
        {
            moveDirection.y += forceToGround * Time.deltaTime;
        }
        moveDirection.y += Physics.gravity.y * Time.deltaTime;

        desiredDirection = transform.forward * axis.y + transform.right * axis.x;
        moveDirection.x = desiredDirection.x * speed;
        moveDirection.z = desiredDirection.z * speed;

        controller.Move(moveDirection * Time.deltaTime);

        if (energy <= 0)
        {
            energy = 0;
            death = true;
            Debug.Log("NO ENERGY...YOU WILL DIE");
        }
    }

    public void SetHorizontalAxis(float x)
    {
        axis.x = x;
    }

    public void SetVerticalAxis(float y)
    {
        axis.y = y;
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            //sound.Play(0);
            jump = true;
            moveDirection.y = jumpSpeed;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (controller.isGrounded)
            {
                isGrounded = true;
                jump = false;
            }
        }
    }

    public void Run()
    {
        speed += run;
    }

    public void Walk()
    {
        speed = walk;
    }

    public void SlowStep()
    {
        speed = walk - 1;
    }

    public void ReceivedDamage()
    {
        lifeBar.ReceivedDamage(hitYourself);
        Debug.Log("OUCH!");
    }
}
