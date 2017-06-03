using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroController : MonoBehaviour {

    public HeroGameObjectDictionary heros;

    List<SpawnManager> spawners;
    MapController mapController;

    //Summon Stats
    public uint summonRound;
    public float atrativinessMultiply = 2.0f, minimumConstant = 1.0f , a = 1.0f, b = 1.0f, c = 0.0f;

    // Use this for initialization
    void Awake () {
        mapController = transform.parent.GetComponentInChildren<MapController>();
        summonRound = 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void nextRound(uint round)
    {
        
        Atractiviness mapAtractiviness = mapController.atractiviness;
        float totalAtractiviness = mapAtractiviness.totalAtractiviness;

        //Functions result
        //Linear function determines the "max" on the time
        //Exponential function determines the  probability to summon a hero
        float constantValue = atrativinessMultiply * totalAtractiviness + minimumConstant;
        float exponentialValue = a * summonRound * summonRound + b * summonRound * totalAtractiviness + c;

        float rndValue = Random.Range(0, constantValue);
        summonRound++;
        if (rndValue <= exponentialValue)
        {
            //Call a Hero
            callNewHero();
            summonRound = 0;
        }
    }

    public void callNewHero()
    {
        //Choose a new hero based on the constructed buildings
        HeroClass heroClass = mapController.atractiviness.chooseHeroClass();

        //GameObject spawner = GameObject.FindGameObjectWithTag("Spawner");
        //Choose a random Spawner on the screen and then call him to spawn the hero
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        GameObject spawner = spawners[Random.Range(0,spawners.Length -1)];
        GameObject hero = spawner.GetComponent<SpawnManager>().INeedAHero(heros[heroClass]);

        hero.GetComponent<EmotionalState>().randomizeStatus();
    }

    public void sendHeroAway(BasicHero hero)
    {
        Destroy(hero);
    }
}
