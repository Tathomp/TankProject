using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainerManager : MonoBehaviour {

	// Use this for initialization
	void Awake () {


        Transform levelSpawn = GameObject.FindGameObjectWithTag("LevelSpawn").transform;

        int level = Random.Range(1, 3);

        if (levelSpawn.transform.childCount > 0)
            GameObject.Destroy(levelSpawn.GetChild(0).gameObject);

        GameObject go = Instantiate<GameObject>(
            Resources.Load<GameObject>("Levels/Level" + level),
            levelSpawn
            );

        go.transform.parent = levelSpawn;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
