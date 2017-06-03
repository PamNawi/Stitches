using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameObject INeedAHero(GameObject hero)
    {
        Vector3 summonPosition = transform.parent.position;
        summonPosition.z = 0.0f;
        return Instantiate(hero, summonPosition, Quaternion.identity) as GameObject;
    }
}
