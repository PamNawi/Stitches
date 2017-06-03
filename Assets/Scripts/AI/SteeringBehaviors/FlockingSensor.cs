using UnityEngine;
using System.Collections.Generic;

public class FlockingSensor : MonoBehaviour {
    List<GameObject> heroes = new List<GameObject>();
    List<SteeringBehavior> heroesST = new List<SteeringBehavior>();
   	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.transform == transform.parent)
            return;
        if(c.tag == "Hero")
        {
            Debug.Log("Tem outro cara aqui!");
            heroes.Add(c.gameObject);
            heroesST.Add(c.GetComponent<SteeringBehavior>());
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.transform == transform.parent)
            return;
        if (c.tag == "Hero")
        {
            heroes.Remove(c.gameObject);
            heroesST.Remove(c.GetComponent<SteeringBehavior>());
        }
    }
    
    public List<GameObject> getAllHeroesGameObjects()
    {
        return heroes;
    }
    public List<SteeringBehavior> getAllHeroesMovement() {
        return heroesST;
    }
}
