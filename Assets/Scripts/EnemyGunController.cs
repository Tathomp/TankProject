using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunController : MonoBehaviour
{

    public bool isFiring;
    public BulletController projectile;
    public float muzzleVelocity;
    public float timeBetweenShots;
    private float shotCounter;
    public Transform firePoint;
    public EnemyGunController enemyGunController;


    // Use this for initialization
    void Start()
    {
        enemyGunController = GetComponent<EnemyGunController>();
    }

    // Update is called once per frame
    void Update()
    {
        // don't process ai if the game is paused
        if(GameState.GameIsPaused == true)
        {
            return;
        }

        if (isFiring)
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
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
