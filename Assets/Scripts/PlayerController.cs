using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Tank {

    public float moveSpeed;
    public int playerHealth;

    private Rigidbody playerRigidbody;

    private Vector3 moveInput;
    private Vector3 moveVelocity;

    private Camera playerCamera;

    private Object tm;

    [Tooltip("Attach the Barrel GameObject here")]
    public GunController playerGun;


    // Use this for initialization
    void Start () {
       // tm = GetComponentInChildren<RFX4_TransformMotion>(true);  this component is from the effects package
                                                                //  there were too many files in the effects folder
                                                                //  to make a commit; if we add effects, we can
                                                                //  cherrypick what we need from the package.
        playerRigidbody = GetComponent<Rigidbody>();
        playerCamera = FindObjectOfType<Camera>();

        //Quick test call to make sure the effects work
        Upgrades[0].AddEffect();
    }



    // Update is called once per frame
    void Update () {


        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput * moveSpeed;  // we could add a term here for moveMultiplier
                                               // with high values for buffs and maybe
                                               // lower values to drop speed as a penalty for something

       

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

        if(playerHealth <= 0)
        {
            playerRigidbody.AddExplosionForce(10000.0f, playerRigidbody.position, 10.0f);
        }
        

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("EnemyProjectile"))
        {
            playerHealth -= 1;

            if(playerHealth <= 0)
            {
                Destroy(playerRigidbody);
                Destroy(this);
            }
        }
    }

    void FixedUpdate() {
        playerRigidbody.velocity = moveVelocity;
        
    }
}
