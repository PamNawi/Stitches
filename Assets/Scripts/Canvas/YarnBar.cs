using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class YarnBar : MonoBehaviour {

    public Text[] yarnQuantity;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void updateYarnQuantity(YarnStash stash)
    {
        int i = 0;
        for (YarnColors color = YarnColors.Black; color != YarnColors.enumEnd; color++, i++)
        {
            yarnQuantity[i].text = " : x " + stash.stash[color] + "\n";
        }
    }
}
