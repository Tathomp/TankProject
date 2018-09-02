using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public bool isFiring;
    public BulletController projectile;
    public float muzzleVelocity;
    public float timeBetweenShots;
    private float shotCounter;
    public Transform firePoint;
    public GunController playerGun; // we could just use the "this" 
                                    // keyword instead of having an 
                                    // object hold a reference to itself


    // Use this for initialization
    void Start () {
        playerGun = GetComponent<GunController>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            playerGun.isFiring = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerGun.isFiring = false;
        }


        if (isFiring)
        {
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                BulletController newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation) as BulletController;
                newProjectile.velocity = muzzleVelocity;
            }
           
        }
        else
        {
            shotCounter = 0;
        }
	}
}
