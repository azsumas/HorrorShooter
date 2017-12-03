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
    public Light lanternLight;

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
    public bool stealthy = false;
    
    [Header("Stats Player")]
    [SerializeField]
    bool death;

    [Header("Energy Player")]
    public float hitYourself;
    public float maxEnergy;
    public float energy;
    public float maxStamina;
    bool stamina;
    float staminaCount;
    float breath;
    float lanternEnergy;
    float maxLightIntensity;

    [Header("Animations")]
    public Animator stealthyAnim;

    // Use this for initialization
    void Start ()
    {
        controller = GetComponent<CharacterController>();
        speed = walk;
        death = false;
        staminaCount = maxStamina;
		stamina = false;
        maxLightIntensity = lanternLight.intensity;
    }

	// Update is called once per frame
	void Update ()
    {
        //Reset states
        if(!controller.isGrounded) isGrounded = false;

        //logic
        if(isGrounded && !jump)
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
        breathFB.color = new Vector4(255.0F, 255.0f, 255.0F, 0.1f * (breath));

        if (moveDirection != new Vector3(0, moveDirection.y, 0)) // SI LA DIRECCIÓN DEL JUGADOR ES IGUAL A 0 ( NO SE ESTÁ MOVIENDO ), STAMINA = FALSE.
        {
            if(moveFast)
            {
                stamina = true;
                Debug.Log("IS RUNNING");
            }
        }

        if(stamina == true)
        {
            staminaCount -= Time.deltaTime;
            if(staminaCount <= 0)
            {
                staminaCount = 0;
                Walk();
                Debug.Log("I'M TIRED");
            }

            breath += Time.deltaTime;
            if(breath >= 2) breath = 2;
        }
        else if(stamina == false)
        {
            staminaCount += (Time.deltaTime / 4);
            if(staminaCount >= maxStamina) staminaCount = maxStamina;

            breath -= Time.deltaTime;
            if(breath <= 0) breath = 0;
        }

        // CONTROL ENERGY PLAYER
        if(energy <= 0)
        {
            energy = 0;
            death = true;
        }

        // CONTROL LANTERN AND ENERGY
        if(lantern.switchOn)
        {
            lanternEnergy = Time.deltaTime / 2;
            Lantern();

            if(energy <= 25)
            {
                lanternLight.intensity = energy / 5;
                Debug.Log("Low Battery");
            }
            else lanternLight.intensity = maxLightIntensity;
        }
        if (stealthy == true) stealthyAnim.SetBool("Stealthy", true);
        else if (stealthy == false)
        {
            stealthyAnim.enabled = true;
            stealthyAnim.SetBool("Stealthy", false);
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
        stealthy = false;
        moveFast = false;
		stamina = false;
        speed = walk;
    }

    public void SlowStep()
    {
        stealthy = true;
        speed = walk - 1;
    }

    public void Lantern()
    {
        lifeBar.ReceivedDamage(lanternEnergy);
        Debug.Log("LANTERN CONSUME ENERGY!");
    }

    public void ReceivedDamage(int hit)
    {
        lifeBar.ReceivedDamage(hit);
        Debug.Log("OUCH!");
    }

    public void RecoveryEnergy(int recoveryEnergy)
    {
        if (energy < maxEnergy)
        {
            energy += recoveryEnergy;
        }
        if (energy >= maxEnergy) energy = maxEnergy;

        lifeBar.UpdateEnergyUI();
    }

    void PauseAnimationEvent()
    {
        stealthyAnim.enabled = false;
    }
}
