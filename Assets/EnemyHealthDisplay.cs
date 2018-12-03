using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthDisplay : MonoBehaviour {

    public Transform maxTransform, current;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Vector3 v3 = Camera.main.transform.position - this.transform.position;
        //transform.rotation = Quaternion.LookRotation(v3) * Quaternion.Euler(0, -90, 0);

        transform.LookAt(Camera.main.transform);
    }

    public void UpdateHealthVisual(int max, int crt)
    {
        float percent = (float)crt / (float)max;
        current.localScale = new Vector2(maxTransform.localScale.x * percent, current.localScale.y);

    }
}
