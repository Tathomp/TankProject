using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Tank
{

    //--------------------------------------------------------------------------------
    //                  MOVEMENT EXPERIMENT
    //--------------------------------------------------------------------------------

    public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    public float m_Speed = 12f;                 // How fast the tank moves forward and back.
    public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
    //public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    //public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
    //public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
    //public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.


    private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    private string m_TurnAxisName;              // The name of the input axis for turning.
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    //private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.







    //--------------------------------------------------------------------------------
    //                  original code

    //public float moveSpeed; **

    private Rigidbody playerRigidbody;

    //private Vector3 moveInput;    **
    //private Vector3 moveVelocity; **

    //private Vector3 throttleInput; **
    //private Vector3 turnInput; **

    private Camera playerCamera;

    [Tooltip("Attach the Barrel GameObject here")]
    public GunController playerGun;

    //--------------------------------------------------------------------------------
    //                  MOVEMENT EXPERIMENT
    //--------------------------------------------------------------------------------

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        // Also reset the input values.
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
    }




    // Use this for initialization
    void Start()
    {
       

        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        playerRigidbody = GetComponent<Rigidbody>();
        playerCamera = FindObjectOfType<Camera>();


        CurrentHealth = MaxHealth;

    }



    // Update is called once per frame
    void Update()
    {


        //--------------------------------------------------------------------------------
        //                  original code

        //throttleInput = new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
        //turnInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);

        //moveVelocity = throttleInput * moveSpeed;

        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //{
        //    playerRigidbody.MoveRotation(Vector3.up * Time.deltaTime);
        //}

        //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //{
        //    playerRigidbody.transform.Rotate(Vector3.down * Time.deltaTime);
        //}




        //--------------------------------------------------------------------------------
        //                  MOVEMENT EXPERIMENT
        //--------------------------------------------------------------------------------


        m_MovementInputValue = Input.GetAxisRaw("Vertical");
        m_TurnInputValue = Input.GetAxisRaw("Horizontal");





        //moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        //moveVelocity = moveInput * moveSpeed;  // we could add a term here for moveMultiplier
        //                                       // with high values for buffs and maybe
        //                                       // lower values to drop speed as a penalty for something



        /*
        Ray cameraRay = playerCamera.ScreenPointToRay(Input.mousePosition);
        Plane levelPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        // if the camera ray shoots out and hits any other object in the world
        // this returns true
        if (levelPlane.Raycast(cameraRay, out rayLength))
        {
            // create point on the plane for the camera to point at
            Vector3 playerView = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, playerView, Color.cyan);

            playerRigidbody.transform.LookAt(new Vector3(playerView.x, transform.position.y, playerView.z));
        }
        */

        if (Input.GetMouseButtonDown(0))
        {
            playerGun.isFiring = true;

            // trying to get an effect to fire


            //Ray cameraRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            //Plane levelPlane = new Plane(Vector3.up, Vector3.zero);
            //float rayLength;
            //if (levelPlane.Raycast(cameraRay, out rayLength))
            //{
            //    Vector3 playerView = cameraRay.GetPoint(rayLength);
            //    Quaternion fireDirection = new Quaternion();
            //    fireDirection.SetLookRotation(playerView);
            //    Instantiate(tm, playerRigidbody.position, fireDirection);
            //}



        }

        if (Input.GetMouseButtonUp(0))
        {
            playerGun.isFiring = false;
        }

        if (CurrentHealth <= 0)
        {
            playerRigidbody.AddExplosionForce(10000.0f, playerRigidbody.position, 10.0f);
        }


    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("EnemyProjectile"))
        {
            CurrentHealth -= 1;

            if (CurrentHealth <= 0)
            {
                Destroy(playerRigidbody);
                Destroy(this);
            }
        }
    }

    void FixedUpdate()
    {
        //playerRigidbody.velocity = moveVelocity;

        Move();
        Turn();

    }



    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }

}