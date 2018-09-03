using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    [Tooltip("Set the speed of the bullet.")]
    public float velocity = 1;

    [Tooltip("Set how long the bullet lasts after firing.")]
    public float lifespan = 5;

    public float lifeLeft;
    // Use this for initialization
    void Start () {
        lifeLeft = lifespan;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.up * velocity * Time.deltaTime);

        lifeLeft -= Time.deltaTime;

        if(lifeLeft <= 0)
        {
            Destroy(this);
        }
	}
}
