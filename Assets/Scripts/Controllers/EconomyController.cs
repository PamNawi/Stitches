using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;


public class EconomyController : MonoBehaviour {

    YarnStash stash;
    public GameObject yarnBar;
    // Use this for initialization
    void Awake () {
        stash = transform.GetComponent<YarnStash>();
    }
	
	// Update is called once per frame
	void Update () {
        YarnBar yb = yarnBar.GetComponent<YarnBar>();
        yb.updateYarnQuantity(stash);
	}

    public bool canKnitThis(GameObject building)
    {
        YarnStash buildingCost = building.GetComponent<YarnStash>();
        for (YarnColors color = YarnColors.Black; color != YarnColors.enumEnd; color++)
        {
            if (stash.stash[color] - buildingCost.stash[color] < 0)
                return false;
        }
        return true;
    }

    public void knitThis(GameObject gObject) {
        YarnStash yarnCost = gObject.GetComponent<YarnStash>();
        stash.stash -= yarnCost.stash;
    }

    public void unravelThis(GameObject gObject)
    {
        YarnStash yarnCost = gObject.GetComponent<YarnStash>();
        stash.stash += yarnCost.stash;
    }

    public void nextRound(uint round) {
        //Update all statistics
        if (round % 2 == 0)
        {
        }
    }

    public List<string> getReports()
    {
        List<string> reports = new List<string>();
        return reports;
    }

    public YarnStash getStash()
    {
        return stash;
    }

}
