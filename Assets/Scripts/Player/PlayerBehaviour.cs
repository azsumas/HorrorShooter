using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerBehaviour : MonoBehaviour
{
    public enum State { Default, GOD }
    public State state = State.Default;

    public Camera cameraAim;
    CharacterController characterCollider;
    private CharacterController controller;
    public EnergyBar lifeBar;
    public LanternFunctions lantern;
    public Light lanternLight;
    public GameObject managerScene;
    private LevelManager script;
    private LaserGun laser;
    public Text packEnergyCount;

    [Header("Direction")]
    public Vector3 moveDirection;

    [Header("Speed")]
    bool moveFast;
    public float speed;
    public float run;
    public float walk;
    public float slowStep;
    public Vector2 axis;
    Vector3 desiredDirection;
    float altura;

    [Header("Stats Jumps")]
    public float forceToGround = Physics.gravity.y;
    public float jumpSpeed;
    public float gravitymagnitude = 1;

    [Header("Stats")]
    public bool jump;
    public bool isGrounded;
    public bool stealthy = false;

    [Header("Stats Player")]
    public bool death = false;
    public int deadCondition;

    [Header("Energy Player")]
    private DataLogic data;
    public float hitYourself;
    public float maxEnergy;
    public float energy;
    public float maxStamina;
    bool stamina;
    float staminaCount;
    float breath;
    float lanternEnergy;
    float maxLightIntensity;
    float runEnergy;
    public int energyPackCount;

    [Header("Canvas")]
    public Image breathFB;
    public Image aimPoint;
    public Image radar;

    [Header("Animations")]
    public Animator hitAnim;
    public Animator stealthyhitAnim;
    public float fieldOfViewAim;

    [Header("Easing Aim")]
    public GameObject gun;
    public bool aimEasing;
    public float iniPosX;
    public float iniPosY;
    public float finalPosX;
    public float finalPosY;
    public float currentTime;
    public float timeDuration;

    [Header("Sounds")]
    private AudioPlayer audioPlayer;

    // Use this for initialization
    private void Awake()
    {
        this.gameObject.SetActive(true);
    }
    void Start()
    {
        Cursor.visible = false;
        characterCollider = gameObject.GetComponent<CharacterController>();
        controller = GetComponent<CharacterController>();
        speed = walk;
        staminaCount = maxStamina;
        stamina = false;
        death = false;
        maxLightIntensity = lanternLight.intensity;
        PlayerPrefs.SetInt("Death", 0);
        managerScene = GameObject.FindWithTag("Manager");
        script = managerScene.GetComponent<LevelManager>();
        data = managerScene.GetComponent<DataLogic>();
        laser = this.gameObject.GetComponent<LaserGun>();
        gun.transform.localPosition = new Vector3(iniPosX, iniPosY, transform.position.z);
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
        audioPlayer.PlayMusic(1, 0.1f, true);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Default:
                DefaultUpdate();
                break;
            case State.GOD:
                GODUpdate();
                break;
            default:
                break;
        }
    }
    void GODUpdate()
    {
        if(Input.GetKey(KeyCode.Z)) altura = -1f;
        else if(Input.GetKey(KeyCode.Q)) altura = 1f;
        else altura = 0f;
        transform.Translate(speed * Input.GetAxis("Horizontal") * Time.deltaTime, altura * Time.deltaTime, speed * Input.GetAxis("Vertical") * Time.deltaTime);
        energy = maxEnergy;
        if(Input.GetKey(KeyCode.A)) laser.ammo = laser.maxAmmo;
    }

    void DefaultUpdate()
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

        if(moveDirection != new Vector3(0, moveDirection.y, 0)) // SI LA DIRECCIÓN DEL JUGADOR ES IGUAL A 0 ( NO SE ESTÁ MOVIENDO ), STAMINA = FALSE.
        {
            //audioPlayer.PlayMusic(0);
            if(moveFast)
            {
                //Debug.Log("MoveFast funciona?");
                stamina = true;
                //Debug.Log("IS RUNNING");
            }
        }

        if(stamina == true)
        {
            staminaCount -= Time.deltaTime;
            if(staminaCount <= 0)
            {
                staminaCount = 0;
                Walk();
                //Debug.Log("I'M TIRED");
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
        if(energy <= 0) energy = 0;

        if(energy == 0)
        {
            death = true;
            speed = 0;
            //this.gameObject.SetActive(false);
            PlayerPrefs.SetInt("Death", 1);
            script.LoadNext();
        }

        // CONTROL LANTERN AND ENERGY
        if(lantern.switchOn)
        {
            lanternEnergy = Time.deltaTime / 4;
            Lantern();

            if(energy <= 25)
            {
                lanternLight.intensity = energy / 25;
                //Debug.Log("Low Battery");
            }
            else lanternLight.intensity = maxLightIntensity;
        }
        if(moveFast == true)
        {
            runEnergy = Time.deltaTime;
            RunEnergy();
        }
        packEnergyCount.text = energyPackCount + ("");

        //STEALTHY ANIMATION
        if(stealthy == true)
        {
            if(characterCollider.height >= 1.0f) { characterCollider.height -= Time.deltaTime * 10; }
        }
        else if(stealthy == false)
        {
            if(characterCollider.height <= 1.8f) { characterCollider.height += Time.deltaTime * 10; }
        }

        //AIM ANIMATION
        if(aimEasing == true)
        {
            if(currentTime <= timeDuration) //Hacer el easing durante el tiempo
            {
                //Calcular el valor del easing en currentTime
                float valueX = Easing.SineEaseOut(currentTime, iniPosX, finalPosX - iniPosX, timeDuration);
                float valueY = Easing.SineEaseOut(currentTime, iniPosY, finalPosY - iniPosY, timeDuration);

                currentTime += Time.deltaTime;
                // Asignar el valor calculado a la posicion que queremos modificar. Los demás ejes, no los modificamos.
                gun.transform.localPosition = new Vector3(valueX, valueY, 0);

                // Ha terminado el easing justo cuando se cumpla esa condicion
                if(currentTime >= timeDuration)
                {
                    // Nos aseguramos de que acabe en la posición final.
                    gun.transform.localPosition = new Vector3(finalPosX, finalPosY, 0);
                }
            }
        }
        else if(aimEasing == false)
        {
            currentTime -= Time.deltaTime;
            if(currentTime <= 0) { currentTime = 0; }

            float valueX = Easing.SineEaseOut(currentTime, gun.transform.position.z, iniPosX - gun.transform.position.z, timeDuration);
            float valueY = Easing.SineEaseOut(currentTime, gun.transform.position.y, iniPosY - gun.transform.position.y, timeDuration);

            // Asignar el valor calculado a la posicion que queremos modificar. Los demás ejes, no los modificamos.
            gun.transform.localPosition = new Vector3(valueX, valueY, 0);

            // Ha terminado el easing justo cuando se cumpla esa condicion
            if(currentTime <= 0)
            {
                // Nos aseguramos de que acabe en la posición final.
                gun.transform.localPosition = new Vector3(iniPosX, iniPosY, 0);
            }
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
        if(controller.isGrounded)
        {
            jump = true;
            moveDirection.y = jumpSpeed;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if(controller.isGrounded)
            {
                isGrounded = true;
                jump = false;
            }
        }
    }

    public void Run()
    {
        moveFast = true;
        speed += run;
        //Debug.Log("RUN");
    }

    public void RunEnergy()
    {
        lifeBar.ReceivedDamage(runEnergy);
    }

    public void Walk()
    {
        //characterCollider.height = 1.8f;
        stealthy = false;
        moveFast = false;
        stamina = false;
        speed = walk;
    }

    public void SlowStep()
    {
        //characterCollider.height = 1.0f;
        stealthy = true;
        speed -= slowStep;
    }

    public void Lantern()
    {
        lifeBar.ReceivedDamage(lanternEnergy);
        data.SetEnergy(energy);
        //Debug.Log("LANTERN CONSUME ENERGY!");
    }

    public void ReceivedDamage(int hit)
    {
        lifeBar.ReceivedDamage(hit);
        data.SetEnergy(energy);
        if(stealthy == true)
        {
            //Debug.Log("ENTRAAAAAA");
            stealthyhitAnim.SetTrigger("StealthyHit");
        }
        hitAnim.SetTrigger("Hit");
        //Debug.Log("OUCH!");
    }

    public void PackEnergy(int energyPack)
    {
        /*if (energy < maxEnergy)
        {
            energy += recoveryEnergy;
        }
        if (energy >= maxEnergy) energy = maxEnergy;*/
        energyPackCount += energyPack;
        data.SetEnergy(energy);
    }

    public void RecoveryEnergy()
    {
        if(energyPackCount >= 1 && energy != maxEnergy)
        {
            energy = maxEnergy;
            energyPackCount -= 1;
        }
        lifeBar.UpdateEnergyUI();
        data.SetEnergy(energy);
    }


    public void AimPlayer()
    {
        cameraAim.fieldOfView -= 2;
        if(cameraAim.fieldOfView <= fieldOfViewAim) cameraAim.fieldOfView = fieldOfViewAim;
        //aimPoint.enabled = false;
        radar.color = new Vector4(255.0F, 255.0f, 255.0F, 0.0f);
        aimEasing = true;
    }

    public void NoAimPlayer()
    {
        //currentTime = 0;
        cameraAim.fieldOfView += 2;
        if(cameraAim.fieldOfView >= 60) cameraAim.fieldOfView = 60;
        //aimPoint.enabled = true;
        radar.color = new Vector4(255.0F, 255.0f, 255.0F, 1.0f);
        aimEasing = false;
    }

    public void SetGodMode()
    {
        if(state == State.Default)
        {
            controller.enabled = false;
            state = State.GOD;
        }

        else
        {
            controller.enabled = true;
            state = State.Default;
        }
    }



    /*void Aim()
    {
        if(aiming) return;
        aiming = true;

        StopCoroutine(AimAnim());

        iniValue = gun.localPos
        finalValue = 1
        currentTime = 0:
        timeDuration = 1

        StartCoroutine(AimAnim());
    }
    void NoAim()
    {
        if(!aiming) return;
        aiming = false;

        StopCoroutine(AimAnim());

        iniValue = gun.localPos;
        finalValue = 0
        currentTime = 0:
        timeDuration = 1

        StartCoroutine(AimAnim());
    }

    IEnumerator AimAnim()
    {
        while(true)
        {
            if(currentTime <= timeDuration)
            {
                //Vector3 v = Easing.nasda(currentTime, iniValue, finalValue - iniValue, timeDurati
                //gun.pos = v;

                currentTime += Time.deltaTime;
            }
            else
            {
                //gun.pos = final;
                currentTime = 0;
                break;
            }
            yield return null;
        }
    }*/
}
