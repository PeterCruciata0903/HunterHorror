using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    //CharacterMovement Variables
    public CharacterController controller;
    public GameObject player;
    public PlayerAudio playerAudio;
    

    [Header("Walking")]
    public float walkSpeed = 8f;
    public float gravity = -9.8f;

    [Header("Jumping")]
    public float jumpVelocity = 4f;
    public float jumpCooldown = 0.25f; //After how much time can we jump again?
    public float airMultiplier = 0.7f; //how much can we move while airborn compared to walking speed
    bool readyToJump;

    [Header("Sprinting")]
    public float sprintSpeed = 15f;
    public float endurance = 10f; // Value changes depending on how long the character is sprinting
    public float enduranceMax = 10f; // constant value

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    public bool isGrounded;
    public LayerMask whatIsGround;

    //True velocity
    public Vector3 velocity;

    //Wasd values
    float AD;
    float WS;

    //True Speed
    public float playerSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        readyToJump = true;
        playerSpeed = walkSpeed;
        player = gameObject;
    }

    public float Speed
    {
        get { return playerSpeed; }
        set { playerSpeed = value; }
    }

    void Update()
    {
        
        //For ground check
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, controller.height * 0.5f + 0.2f, whatIsGround);

        if (isGrounded && velocity.y<0)
        {
            velocity.y = 0.0f;
        }

        WASDInput();

        Vector3 move = transform.right * AD + transform.forward * WS;

        //Modify speed depending on controls and position
        PlayerSprint();
        PlayerWalk();
        airMovement();

        controller.Move(move*playerSpeed*Time.deltaTime);

        if (move != Vector3.zero)
        {
            //gameObject.transform.forward = move;
        }
        Jump(); //increases vertical velocity instantly
        Gravity(); //applies vertical acceleration
        
    }
    private void WASDInput()
    {
        AD = Input.GetAxis("Horizontal");
        WS = Input.GetAxis("Vertical");
    }

    private void Jump()
    {
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;

            velocity.y += Mathf.Sqrt(jumpVelocity * -5f * gravity);

            PlayerAudio.Instance.playerJump();

            //How to activate a function on a cooldown
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Gravity()
    {

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void PlayerWalk() //sets speed if can walk
    {
        if (!Input.GetKey(sprintKey) && isGrounded)
        {
            accelerateToSpeed(walkSpeed, 30);
            return;
        }
        if(playerSpeed < walkSpeed + 1)
            PlayerAudio.Instance.playerWalkingNoise();
    }
    private void PlayerSprint() //modifies speed if can sprint
    {
        if(playerSpeed > walkSpeed+1)
            PlayerAudio.Instance.playerSprintingNoise();

        if (Input.GetKey(sprintKey) && endurance > 0 && isGrounded)
        {
            accelerateToSpeed(sprintSpeed, 20);
            endurance-=Time.deltaTime;
            return;
        }
        if (endurance < enduranceMax)
        {
            endurance += Time.deltaTime; //with more endurance, recovery is faster
            PlayerWalk();
            WaitSeconds(1);
            return;
        }

    }

    private void airMovement() //Changes the speed if airborn
    {

        if (!isGrounded)
        {
            accelerateToSpeed(airMultiplier*sprintSpeed, 0.5f);
        }
    }

    private void accelerateToSpeed(float finalSpeed, float accelerationFactor)
    {

        if (Mathf.Abs(finalSpeed - playerSpeed) < 0.3) // if difference between final speed is negligible, return
            return;
        if(finalSpeed > playerSpeed)
        {
            playerSpeed += Time.deltaTime * accelerationFactor;
            //Singleton.Instance.AudioManager.playerSprintingNoise(player);
        }
        if (finalSpeed < playerSpeed)
        {
            playerSpeed -= Time.deltaTime*accelerationFactor*1.5f;
            //Singleton.Instance.AudioManager.playerStoppingNoise(player);
        }
            
    }

    IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
