using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Tank
{

    //--------------------------------------------------------------------------------
    //                  MOVEMENT EXPERIMENT
    //--------------------------------------------------------------------------------


    public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    //public float m_Speed;                 // How fast the tank moves forward and back.
    //public float m_TurnSpeed;            // How fast the tank turns in degrees per second.
    ////public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    ////public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
    ////public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
    ////public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.


    //private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    //private string m_TurnAxisName;              // The name of the input axis for turning.
    //private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    //private float m_MovementInputValue;         // The current value of the movement input.
    //private float m_TurnInputValue;             // The current value of the turn input.
    //private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.


    //all left wheels
    public GameObject[] LeftWheels;
    //all right wheels
    public GameObject[] RightWheels;

    public GameObject LeftTrack;

    public GameObject RightTrack;

    public float wheelsSpeed = 2f;
    public float tracksSpeed = 2f;
    public float forwardSpeed = 10f;
    public float rotateSpeed = 1f;




    //--------------------------------------------------------------------------------
    //                  original code

    //public float moveSpeed; **

    private Rigidbody playerRigidbody;
    public Logger hitLogger;

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

    //private void Awake()
    //{
    //    m_Rigidbody = GetComponent<Rigidbody>();
    //}


    //private void OnEnable()
    //{
    //    // When the tank is turned on, make sure it's not kinematic.
    //    m_Rigidbody.isKinematic = false;

    //    // Also reset the input values.
    //    m_MovementInputValue = 0f;
    //    m_TurnInputValue = 0f;
    //}


    //private void OnDisable()
    //{
    //    // When the tank is turned off, set it to kinematic so it stops moving.
    //    m_Rigidbody.isKinematic = true;
    //}




    // Use this for initialization
    void Start()
    {
       
        playerRigidbody = GetComponent<Rigidbody>();
        playerCamera = FindObjectOfType<Camera>();


        CurrentHealth = MaxHealth;

        InitializePlayer();
        GameState.LevelStarted += InitializePlayer;

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

        // check to make sure it's appropriate to check for input
        if (GameState.GameIsPaused == true)
        {
            return;
        }
        
        //m_MovementInputValue = Input.GetAxisRaw("Vertical");
        //m_TurnInputValue = Input.GetAxisRaw("Horizontal");

        //Keyboard moves =======================================//
        //Forward Move
        if (Input.GetKey(KeyCode.W))
        {
            //Left wheels rotate
            foreach (GameObject wheelL in LeftWheels)
            {
                wheelL.transform.Rotate(new Vector3(wheelsSpeed, 0f, 0f));
            }
            //Right wheels rotate
            foreach (GameObject wheelR in RightWheels)
            {
                wheelR.transform.Rotate(new Vector3(-wheelsSpeed, 0f, 0f));
            }
            //left track texture offset
            LeftTrack.transform.GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, Time.deltaTime * tracksSpeed);
            //right track texture offset
            RightTrack.transform.GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, Time.deltaTime * tracksSpeed);

            //Move Tank

            transform.Translate(new Vector3(0f, 0f, forwardSpeed));

        }
        //Back Move
        if (Input.GetKey(KeyCode.S))
        {
            //Left wheels rotate
            foreach (GameObject wheelL in LeftWheels)
            {
                wheelL.transform.Rotate(new Vector3(-wheelsSpeed, 0f, 0f));
            }
            //Right wheels rotate
            foreach (GameObject wheelR in RightWheels)
            {
                wheelR.transform.Rotate(new Vector3(wheelsSpeed, 0f, 0f));
            }
            //left track texture offset
            LeftTrack.transform.GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, Time.deltaTime * -tracksSpeed);
            //right track texture offset
            RightTrack.transform.GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, Time.deltaTime * -tracksSpeed);
            //Move Tank
            transform.Translate(new Vector3(0f, 0f, -forwardSpeed));
        }
        //On Left
        if (Input.GetKey(KeyCode.A))
        {
            //Left wheels rotate
            foreach (GameObject wheelL in LeftWheels)
            {
                wheelL.transform.Rotate(new Vector3(wheelsSpeed, 0f, 0f));
            }
            //Right wheels rotate
            foreach (GameObject wheelR in RightWheels)
            {
                wheelR.transform.Rotate(new Vector3(wheelsSpeed, 0f, 0f));
            }
            //left track texture offset
            LeftTrack.transform.GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, Time.deltaTime * tracksSpeed);
            //right track texture offset
            RightTrack.transform.GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, Time.deltaTime * -tracksSpeed);
            //Rotate Tank
            transform.Rotate(new Vector3(0f, -rotateSpeed, 0f));
        }
        //On Right
        if (Input.GetKey(KeyCode.D))
        {
            //Left wheels rotate
            foreach (GameObject wheelL in LeftWheels)
            {
                wheelL.transform.Rotate(new Vector3(-wheelsSpeed, 0f, 0f));
            }
            //Right wheels rotate
            foreach (GameObject wheelR in RightWheels)
            {
                wheelR.transform.Rotate(new Vector3(-wheelsSpeed, 0f, 0f));
            }
            //left track texture offset
            LeftTrack.transform.GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, Time.deltaTime * -tracksSpeed);
            //right track texture offset
            RightTrack.transform.GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, Time.deltaTime * tracksSpeed);
            //Rotate Tank
            transform.Rotate(new Vector3(0f, rotateSpeed, 0f));
        }
        //=======================================//





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
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerGun.isFiring = false;
        }

        if (CurrentHealth <= 0)
        {
            playerRigidbody.AddExplosionForce(10000.0f, playerRigidbody.position, 10.0f);

            //Destroy(gameObject);
        }
        


    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("EnemyProjectile"))
        {
            CurrentHealth -= 3;
            //hitLogger.Log("player hit normal proj");
            if (CurrentHealth <= 0)
            {
               
                // Destroy(gameObject);
                // Destroy(playerRigidbody);
                // Destroy(this);

                // trigger game over : 
                //      - submit score to db
                //      - bring up high score canvas
                //      - have options for user to restart or try another level
            }
        }

        if (col.gameObject.tag.Equals("GiantEnemyProjectile"))
        {
            CurrentHealth -= 15;
            //hitLogger.Log("player hit giant proj");
            if (CurrentHealth <= 0)
            {
               
                //Destroy(gameObject);
                //Destroy(playerRigidbody);
                //Destroy(this);

                // trigger game over : 
                //      - submit score to db
                //      - bring up high score canvas
                //      - have options for user to restart or try another level
            }
        }

        if(CurrentHealth <= 0)
        {
            GameState.EndLevel(false);
        }
    }

    //void FixedUpdate()
    //{
    //    //playerRigidbody.velocity = moveVelocity;

    //    Move();
    //    Turn();

    //}



    //private void Move()
    //{
    //    // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
    //    Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

    //    // Apply this movement to the rigidbody's position.
    //    m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    //}


    //private void Turn()
    //{
    //    // Determine the number of degrees to be turned based on the input, speed and time between frames.
    //    float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

    //    // Make this into a rotation in the y axis.
    //    Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

    //    // Apply this rotation to the rigidbody's rotation.
    //    m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    //}

    public void InitializePlayer()
    {
        SelectUpgradeManager.DistributeUpGrades();

        PlayerState ps = PlayerState.GetCurrentPlayerState();

        transform.position = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;

        ResetTank();



        GunUpgrade = ps.GetGunUpgrade();
        TrackUpgrade = ps.GetTreadUpgrade();
        ArmorUpgrade = ps.GetArmorUpgrade();



        ApplyUpgrades();

        CurrentHealth = MaxHealth;


    }

    private void OnDestroy()
    {
        Debug.Log(gameObject.name + "Destoryed");
    }

}