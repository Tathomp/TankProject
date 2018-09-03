using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {

    [Tooltip("The rate of speed at which the turret will turn.")]
    public float rotSpeed = 1;


    private SphereCollider playerTurretRigidbody;

    private Vector3 moveInput;
    private Vector3 moveVelocity;

    private GunController playerGun;


    // Use this for initialization
    void Start () {

       
    }

    // Fixed Update is used to code that uses the physics engine
    void FixedUpdate()
    {
        RotateTowardsMouse(); 

    }

    
    void RotateTowardsMouse()
    {
        //We create a plane to use as a target for the raycast
        Plane tempPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float hit;

        // Since we're doing this every frame we want to make sure we actually hit the plane before doing anything
        // Then we create a quaternion to use for our new rotation
        if (tempPlane.Raycast(ray, out hit))
        {
            Vector3 target = ray.GetPoint(hit);

            Quaternion newRotationTarget = Quaternion.LookRotation(target - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, newRotationTarget, rotSpeed * Time.deltaTime);
        }
    }
}
