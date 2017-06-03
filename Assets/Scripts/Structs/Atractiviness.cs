using UnityEngine;
using System.Collections.Generic;

public class Atractiviness {
    public HeroGameObjectDictionary heroTypes;
    public HeroFloatDictionary heroAtractiviness;
    public float totalAtractiviness = 0.0f;

    public Atractiviness()
    { 
        heroAtractiviness = new HeroFloatDictionary();
        totalAtractiviness = 0.0f;
    }

    public void calculateAtractiviness() {
        foreach(HeroClass hc in heroAtractiviness.Keys)
        {
            heroAtractiviness[hc] = 0.0f;
        }
        //Get all Buildings
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        //Sum the attractiviness based on the buildings list
        foreach(GameObject b in buildings)
        {
            if (!b)
                continue;
            PlaceableBuilding pb = b.GetComponent<PlaceableBuilding>();
            if (pb)
            {
                heroAtractiviness = heroAtractiviness + pb.heroAtractiviness;
            }
        }
        totalAtractiviness = 0.0f;
        foreach (HeroClass hc in heroAtractiviness.Keys)
        {
            totalAtractiviness += heroAtractiviness[hc];
        }
    }

    public HeroClass chooseHeroClass()
    {
        HeroClass hero = 0;
        float rndClass = (int) Random.Range(0, totalAtractiviness);
        float sum = 0;
        foreach (HeroClass hc in heroAtractiviness.Keys)
        {
            sum += heroAtractiviness[hc];
            if (sum > rndClass)
                return hc;
        }
        return hero;
    }
}
