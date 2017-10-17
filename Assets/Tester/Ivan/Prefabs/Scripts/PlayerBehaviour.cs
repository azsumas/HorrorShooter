using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
    {
    private CharacterController controller;
    public Vector3 moveDirection;

    public float speed;
    //public float speedWalk;
    public Vector2 axis;
    public float forceToGround = Physics.gravity.y;
    public float jumpSpeed;
    public float gravitymagnitude = 1;
    [Header("Stats")]
    public int life = 10;
    public bool dead = false;
    public bool jump;
    public bool isGrounded;
    [Header("UI")]

    private Vector3 desiredDirection;
    [Header("Sounds")]
    public PlaySound sound;
    public float stepTime = 0.4f;
    public float timeCounter = 0.0f;
    public float timeCounterDead = 0.0f;
    [Header ("Animation")]
    [SerializeField] public Animator anim;

    // Use this for initialization
    void Start ()
    {
        controller = GetComponent<CharacterController>();
        //forceToGround = Physics.gravity.y;
        //speed = speedWalk;
        lifeBar.Init(life);
	}
	
	// Update is called once per frame
	void Update ()
    {
        lifeBar.UpdateBar(life);

        if (dead == true)
        {
            anim.enabled = true;
            timeCounterDead += Time.deltaTime;
            GetComponent<deadEffect>().dead = true;
            if (timeCounterDead >= 2.0f)
            {
                Application.LoadLevel(Application.loadedLevel + 1);
            }
        }
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

        if(isGrounded && (moveDirection.x != 0 || moveDirection.z != 0))
        {
            PlayFootsteps();
        }

        if(!dead)
        {
            controller.Move(moveDirection * Time.deltaTime);
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
            sound.Play(0);
            jump = true;
            moveDirection.y = jumpSpeed;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (!isGrounded)
            {
                sound.Play(1);
            }
            if (controller.isGrounded)
            {
                isGrounded = true;
                jump = false;
            }
        }
    }

    public void Run()
    {
        stepTime = 0.2f;
        speed += 2;
    }

    public void Walk()
    {
        stepTime = 0.4f;
        speed -= 2;
    }

    void PlayFootsteps()
    {
        if(dead == false && (timeCounter >= stepTime))
        {
            timeCounter = 0;
            sound.Play(2, Random.Range(0.4f, 0.6f), Random.Range(0.89f, 1.03f), 1);
        }
        timeCounter += Time.deltaTime;
    }

    public void SetDamage(int damage)
    {
        life -= damage;
        sound.Play(3);

        if (life <= 0)
        {
            dead = true;
            life = 0;
            sound.Play(6);
        }

        lifeBar.UpdateBar(life);
    }
    public void Recovery(int healthRecovery)
    {
        if (life < 100)
        {
            life += healthRecovery;
        }
        if (life >= 100) life = 100;

        if(healthRecovery >= 50) sound.Play(4);
        else sound.Play(5);

        //healedSource.Play();
        lifeBar.UpdateBar(life);
    }
}
