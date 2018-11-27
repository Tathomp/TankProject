﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : Tank {
    
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public float maxFiringRange;
    public EnemyGunController enemyGun;

    private Transform player;
    private Vector3 playerTarget;


	// Use this for initialization
	void Start () {
        player =  GameObject.FindGameObjectsWithTag("Player")[0].transform;
        playerTarget = player.position;
        CurrentHealth = MaxHealth;

        GameState.LevelStarted += InitializeEnemy;
    }

    // Update is called once per frame
    void Update () {
		
        if(Vector3.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        } else if (Vector3.Distance(transform.position, player.position) < stoppingDistance && Vector3.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        } else if (Vector3.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }


        enemyGun.isFiring = true;

        //if (Vector3.Distance(transform.position, player.position) < maxFiringRange)
        //{
        //    enemyGun.isFiring = true; 
        //}
        //else
        //{
        //    enemyGun.isFiring = false;
        //}


    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("EnemyProjectile"))
        {
            Logger hitLogger;
            hitLogger = new Logger(Debug.unityLogger.logHandler);
            hitLogger.Log(" enemy hit, score : " + ScoreManager.score);
            CurrentHealth -= 1;
            ScoreManager.score += 1;
            if (CurrentHealth <= 0)
            {
                Destroy(gameObject);
                Destroy(this.gameObject);
                Destroy(this.transform);
                Destroy(this);
                
            }
        }
    }

    public void InitializeEnemy()
    {
        UpgradeDatabase db = Resources.Load<UpgradeDatabase>("UpgradeDatabase");

        switch (GameState.CurrentDifficulty)
        {
            case Difficulty.Easy:
                {
                    ArmorUpgrade = db.UpgradeList[0];
                    TrackUpgrade = db.UpgradeList[3];
                    break;
                }
        }

        ApplyUpgrades();
    }
}
