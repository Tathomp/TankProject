using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletController : MonoBehaviour {
    

    [Tooltip("Set the speed of the bullet.")]
    public float velocity = 1;

    [Tooltip("Set how long the bullet lasts after firing.")]
    public float lifespan = 5;

    public float lifeLeft;

    public ParticleSystem explosion;

    
    // Use this for initialization
    void Start () {
        lifeLeft = lifespan;
        explosion = GetComponent<ParticleSystem>();
	}

    void OnCollisionEnter(Collision col)
    {
        Logger hitLogger;
        hitLogger = new Logger(Debug.unityLogger.logHandler);

        if (col.gameObject.tag.Equals("Player") || col.gameObject.tag.Equals("Enemy") || col.gameObject.tag.Equals("Level") )
        {
            explosion.Play();

           
            hitLogger.Log(" hit object type : " + col.gameObject.tag);

            Destroy(this);
            Destroy(this.gameObject);
            Destroy(this.transform);

        }
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.up * velocity * Time.deltaTime);

        lifeLeft -= Time.deltaTime;

        if(lifeLeft <= 0)
        {
            Destroy(this.gameObject);
            Destroy(this);

        }
	}
}
