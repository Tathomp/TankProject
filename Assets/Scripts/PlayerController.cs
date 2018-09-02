using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    private Rigidbody playerRigidbody;

    private Vector3 moveInput;
    private Vector3 moveVelocity;

    private Camera playerCamera;


    public GunController playerGun;


    // Use this for initialization
    void Start () {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCamera = FindObjectOfType<Camera>();

        // There isn't a gun controller component on the player the inspector so this line was 
        // nulling out the reference that was set in the editor
        //
        //  playerGun = GetComponent<GunController>();
    }



    // Update is called once per frame
    void Update () {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput * moveSpeed;  // we could add a term here for moveMultiplier
                                               // with high values for buffs and maybe
                                               // lower values to drop speed as a penalty for something


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

        if (Input.GetMouseButtonDown(0))
        {
            playerGun.isFiring = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerGun.isFiring = false;
        }

    }

    void FixedUpdate() {
        playerRigidbody.velocity = moveVelocity;
        
    }
}
