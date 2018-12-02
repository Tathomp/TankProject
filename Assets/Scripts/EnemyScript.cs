using System.Collections;
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

        if (player != null)
        {                       
            if (Vector3.Distance(transform.position, player.position) > stoppingDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            else if (Vector3.Distance(transform.position, player.position) < stoppingDistance && Vector3.Distance(transform.position, player.position) > retreatDistance)
            {
                transform.position = this.transform.position;
            }
            else if (Vector3.Distance(transform.position, player.position) < retreatDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            }


            if (Vector3.Distance(transform.position, player.position) < maxFiringRange)
            {
                enemyGun.isFiring = true;
            }
            else
            {
                enemyGun.isFiring = false;
            }
        }

    }

    void OnCollisionEnter(Collision col)
    {
        ScoreManager SM = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreManager>();
        if (col.gameObject.tag.Equals("PlayerProjectile"))
        {
            //Logger hitLogger;
            //hitLogger = new Logger(Debug.unityLogger.logHandler);
            //hitLogger.Log(" enemy hit, score : " + SM.GetScore());
            CurrentHealth -= 1;
            SM.IncreaseScore(100);
            if (CurrentHealth <= 0)
            {
                SM.IncreaseScore(1000);
                /*
                Destroy(gameObject);
                Destroy(this.gameObject);
                Destroy(this.transform);
                Destroy(this);
                */

                gameObject.SetActive(false);

                EndGameManager.HasPlayerWon();
            }
        }
    }

    public void InitializeEnemy()
    {
        UpgradeDatabase db = Resources.Load<UpgradeDatabase>("UpgradeDatabase");
        ResetTank();

        switch (GameState.GetCurrentDifficultyStr())
        {
            /// I added the other difficulty levels here - Clayin
            /// todo: These values should be different for each difficulty
            /// todo: There should be changes to things like starting heath pool and score multiplier too
            case "Easy":
                ArmorUpgrade = db.UpgradeList[0];
                TrackUpgrade = db.UpgradeList[3];
                break;
            case "Medium":
                ArmorUpgrade = db.UpgradeList[0];
                TrackUpgrade = db.UpgradeList[3];
                break;
            case "Hard":
                ArmorUpgrade = db.UpgradeList[0];
                TrackUpgrade = db.UpgradeList[3];
                break;
            case "Impossible":
                ArmorUpgrade = db.UpgradeList[0];
                TrackUpgrade = db.UpgradeList[3];
                break;
        }

        ApplyUpgrades();
    }
}
