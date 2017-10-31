using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerBehaviour : MonoBehaviour
{
    private CharacterController controller;
    public EnergyBar lifeBar;
    public Image breathFB;
    public LanternFunctions lantern;

    [Header("Direction")]
    public Vector3 moveDirection;

	[Header("Speed")]
	bool moveFast;
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
    public float hitDamage;
    
    [Header("Stats Player")]
    [SerializeField]
    private bool death;

    [Header("Energy Player")]
    public float maxEnergy;
    public float energy;
    [SerializeField]
    public bool stamina;
    public float maxStamina;
    [SerializeField]
    float staminaCount;
    float breath;
    float lanternEnergy;

    // Use this for initialization
    void Start ()
    {
        controller = GetComponent<CharacterController>();
        speed = walk;
        death = false;
        staminaCount = maxStamina;
		stamina = false;
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

        // CONTROL ESTAMINA AND FEEDBACK BREATH
        breathFB.color = new Vector4(255.0F, 255.0f, 255.0F, 0.1f * (Time.deltaTime + breath));

		if (moveDirection != new Vector3(0, moveDirection.y, 0)) // SI LA DIRECCIÓN DEL JUGADOR ES IGUAL A 0 ( NO SE ESTÁ MOVIENDO ), STAMINA = FALSE.
		{
			if (moveFast) 
			{
				stamina = true;
				Debug.Log ("IS RUNNING");
			}
		}

        if (stamina == true)
        {
            staminaCount -= Time.deltaTime;
            if (staminaCount <= 0)
            {
                staminaCount = 0;
                Walk();
                Debug.Log("I'M TIRED");
            }

            breath += Time.deltaTime;
            if (breath >= 4) breath = 4;
        }
        else if (stamina == false)
        {
            staminaCount += (Time.deltaTime/4);
            if (staminaCount >= maxStamina) staminaCount = maxStamina;

            breath -= Time.deltaTime;
            if (breath <= 0) breath = 0;
        }

        // CONTROL ENERGY PLAYER
        if (energy <= 0)
        {
            energy = 0;
            death = true;
            Debug.Log("NO ENERGY...YOU WILL DIE");
        }

        // CONTROL LANTERN
        if(lantern.switchOn)
        {
            lanternEnergy += Time.deltaTime/100;
            Lantern();
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
		moveFast = true;

        if (staminaCount >= 0) speed += run;
    }

    public void Walk()
    {
		moveFast = false;
		stamina = false;

        speed = walk;
    }

    public void SlowStep()
    {
        speed = walk - 1;
    }

    public void ReceivedDamage()
    {
        lifeBar.ReceivedDamage(hitDamage);
        Debug.Log("OUCH!");
    }

    public void Lantern()
    {
        lifeBar.ReceivedDamage(lanternEnergy);
        Debug.Log("LANTERN CONSUME ENERGY!");
    }
}
