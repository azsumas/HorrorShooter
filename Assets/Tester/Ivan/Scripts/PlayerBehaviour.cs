﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerBehaviour : MonoBehaviour
{
    private CharacterController controller;
    public EnergyBar lifeBar;
    public Image breathFB;

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
    [SerializeField]
    public bool stamina;
    public float maxStamina;
    [SerializeField]
    private float staminaCount;
    public float breath;

    // Use this for initialization
    void Start ()
    {
        controller = GetComponent<CharacterController>();
        speed = walk;
        death = false;
        staminaCount = maxStamina;
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
        if (staminaCount >= 0) speed += run;
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