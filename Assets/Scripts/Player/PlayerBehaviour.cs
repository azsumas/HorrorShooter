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
    public LaserGun laser;
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
    Collider col;

    [Header("Canvas")]
    public Image breathFB;
    public Image aimPoint;
    public Image radar;
    public GameObject lowEne;
    public Text rechargeText;

    [Header("Animations")]
    public Animator hitAnim;
    public Animator stealthyhitAnim;
    public float fieldOfViewAim;

    [Header("Easing Aim")]
    public GameObject gun;
    public bool aiming;
    public float iniPosX;
    public float iniPosY;
    public float finalPosX;
    public float finalPosY;
    public float currentTime;
    public float timeDuration;

    [Header("Sounds")]
    private AudioPlayer audioPlayer;
    bool playWarning = false;
    bool playSound = false;

    public float stepTime = 0.6f;
    public float timeCounter = 0.0f;

    public float volumeMusic = 0.5f;

    IEnumerator aimCoroutine;

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
        gun.transform.localPosition = gun.transform.localPosition;
        audioPlayer = managerScene.GetComponentInChildren<AudioPlayer>();
        audioPlayer.PlayMusic(1, volumeMusic, true);
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
            if(moveFast)
            {
                stamina = true;
            }
        }

        if(stamina == true)
        {
            staminaCount -= Time.deltaTime;
            if(staminaCount <= 0)
            {
                staminaCount = 0;
                Walk();
            }

            breath += Time.deltaTime;
            if(breath >= 0.5f) breath = 0.5f;
        }
        else if(stamina == false)
        {
            staminaCount += (Time.deltaTime / 4);
            if(staminaCount >= maxStamina) staminaCount = maxStamina;

            breath -= Time.deltaTime/2;
            if(breath <= 0) breath = 0;
        }

        // CONTROL ENERGY PLAYER
        if(energy <= 0) energy = 0;

        if (energy <= 25)
        {
            lowEne.SetActive(true);
            WarningSound();
        }
        else
        {
            lowEne.SetActive(false);
            playWarning = false;
        }

        if(energy == 0)
        {
            death = true;
            speed = 0;
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
            }
            else lanternLight.intensity = maxLightIntensity;
        }
        if(moveFast == true)
        {
            runEnergy = Time.deltaTime;
            RunEnergy();
        }

        packEnergyCount.text = energyPackCount + ("");
        if (energyPackCount <= 0) packEnergyCount.color = new Vector4(220, 0, 0, 1);
        else packEnergyCount.color = new Vector4(224, 245, 236, 1);

        if (energyPackCount >= 1 && energy <= 25)
        {
            rechargeText.enabled = true;
            if (energy <= 20) PlaySound();
        }
        else
        {
            rechargeText.enabled = false;
            playSound = false;
        }

        if (energy <= 25) lifeBar.LowEnergy();
        else lifeBar.HighEnergy();

        //STEALTHY ANIMATION
        if(stealthy == true)
        {
            if(characterCollider.height >= 1.0f) { characterCollider.height -= Time.deltaTime * 10; }
        }
        else if(stealthy == false)
        {
            if(characterCollider.height <= 1.8f) { characterCollider.height += Time.deltaTime * 10; }
        }

        if (isGrounded && (moveDirection.x != 0 || moveDirection.z != 0))
        {
            PlayFootsteps();
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
            audioPlayer.PlaySFX(16);
            jump = true;
            moveDirection.y = jumpSpeed;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (!isGrounded)
            {
                audioPlayer.PlaySFX(17);
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
        if (moveFast == true) return;
        moveFast = true;
        speed += run;
        stepTime = 0.4f;
    }

    public void RunEnergy()
    {
        lifeBar.ReceivedDamage(runEnergy);
    }

    public void Walk()
    {
        stealthy = false;
        moveFast = false;
        stamina = false;
        speed = walk;
        stepTime = 0.6f;
    }

    public void SlowStep()
    {
        stealthy = true;
        speed -= slowStep;
        stepTime = 0.8f;
    }

    void PlayFootsteps()
    {
        if (death == false && (timeCounter >= stepTime))
        {
            timeCounter = 0;
            audioPlayer.PlaySFX(18, Random.Range(0.8f, 1.0f), Random.Range(0.9f, 1.1f), false, false, "SFX" );
        }
        timeCounter += Time.deltaTime;
    }

    public void Lantern()
    {
        lifeBar.ReceivedDamage(lanternEnergy);
        GameManager.instance.dataLogic.SetEnergy(energy);
    }

    public void ReceivedDamage(int hit)
    {
        lifeBar.ReceivedDamage(hit);
        GameManager.instance.dataLogic.SetEnergy(energy);
        if (stealthy == true)
        {
            stealthyhitAnim.SetTrigger("StealthyHit");
        }
        hitAnim.SetTrigger("Hit");
    }

    public void PackEnergy(int energyPack)
    {
        energyPackCount += energyPack;
        GameManager.instance.dataLogic.SetEnergy(energy);
    }

    public void RecoveryEnergy()
    {
        if(energyPackCount >= 1 && energy != maxEnergy)
        {
            audioPlayer.PlaySFX(10);
            energy = maxEnergy;
            energyPackCount -= 1;
        }
        lifeBar.UpdateEnergyUI();
        GameManager.instance.dataLogic.SetEnergy(energy);
    }


    public void AimPlayer()
    {
        cameraAim.fieldOfView -= 5;
        if(cameraAim.fieldOfView <= fieldOfViewAim) cameraAim.fieldOfView = fieldOfViewAim;

        radar.color = new Vector4(255.0F, 255.0f, 255.0F, 0.0f);

        if (aiming) return;

        aiming = true;
        currentTime = 0;

        if (aimCoroutine != null)
        {
            StopCoroutine(aimCoroutine);
            aimCoroutine = null;
        }
        aimCoroutine = AimAnim();

        iniPosX = gun.transform.localPosition.x;
        iniPosY = gun.transform.localPosition.y;
        finalPosX = -0.331f;
        finalPosY = 0.155f;
        currentTime = 0 + currentTime;
        timeDuration = 0 + timeDuration;

        StartCoroutine(aimCoroutine);
    }

    public void NoAimPlayer()
    {
        cameraAim.fieldOfView += 5;
        if(cameraAim.fieldOfView >= 60) cameraAim.fieldOfView = 60;

        radar.color = new Vector4(255.0F, 255.0f, 255.0F, 1.0f);

        if (!aiming) return;

        aiming = false;
        currentTime = 0;

        if (aimCoroutine != null)
        {
            StopCoroutine(aimCoroutine);
            aimCoroutine = null;
        }
        aimCoroutine = AimAnim();

        iniPosX = gun.transform.localPosition.x;
        iniPosY = gun.transform.localPosition.y;
        finalPosX = 0;
        finalPosY = 0;
        currentTime = 0 + currentTime;
        timeDuration = 0 + timeDuration;

        StartCoroutine(aimCoroutine);
    }

    public void WarningSound()
    {
        if (playWarning == true) return;
        audioPlayer.PlaySFX(15);
        playWarning = true;
    }

    public void PlaySound()
    {
        if (playSound == true) return;
        audioPlayer.PlaySFX(23);
        playSound = true;
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

    IEnumerator AimAnim()
    {
        while(true)
        {
            if(currentTime <= timeDuration)
            {
                float x = Easing.SineEaseOut(currentTime, iniPosX, finalPosX - iniPosX, timeDuration);
                float y = Easing.SineEaseOut(currentTime, iniPosY, finalPosY - iniPosY, timeDuration);
                gun.transform.localPosition = new Vector3(x, y, 0); ;

                currentTime += Time.deltaTime;
            }
            else
            {
                gun.transform.localPosition = new Vector3(finalPosX,finalPosY , 0);
                currentTime = 0;
                aimCoroutine = null;
                break;
            }
            yield return null;
        }
    }
}
