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
    public GunController playerGun;
    public ParticleSystem muzzleFlash;

    // Use this for initialization
    void Start () {
        playerGun = GetComponent<GunController>();
        
    }
	
	// Update is called once per frame
	void Update ()
    {
               
        if (isFiring)
        {
           // muzzleFlash. = playerGun.transform;
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                muzzleFlash.Play();
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
