using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretController : MonoBehaviour
{

    [Tooltip("The rate of speed at which the turret will turn.")]
    public float rotationSpeed = 1;
    

    private SphereCollider enemyTurretRigidbody;

    private Transform player;
    private EnemyGunController enemyGun;

    private Vector3 playerTarget;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        playerTarget = new Vector3();
    }

    // Fixed Update is used to code that uses the physics engine
    void FixedUpdate()
    {
        if(playerTarget != null && GameState.GameIsPaused == false)
        {
            playerTarget = player.position;

            RotateTowardsPlayer();
        }


    }


    void RotateTowardsPlayer()
    {
        

        Quaternion newRotationTarget = Quaternion.LookRotation(playerTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotationTarget, rotationSpeed * Time.deltaTime);

        //We create a plane to use as a target for the raycast
        //Plane tempPlane = new Plane(Vector3.up, playerTarget);
        //Ray ray = Camera.main.ScreenPointToRay(playerTarget);

        //float hit;

        // Since we're doing this every frame we want to make sure we actually hit the plane before doing anything
        // Then we create a quaternion to use for our new rotation
        //if (tempPlane.Raycast(ray, out hit))
        //{
        //    Vector3 target = ray.GetPoint(hit);

        //    Quaternion newRotationTarget = Quaternion.LookRotation(target - transform.position);

        //    transform.rotation = Quaternion.Slerp(transform.rotation, newRotationTarget, rotationSpeed * Time.deltaTime);
        //}
    }
}
