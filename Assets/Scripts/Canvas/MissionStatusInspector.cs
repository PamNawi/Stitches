using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MissionStatusInspector : MonoBehaviour {

    public Text missionType;
    public Text interested;

    public BasicMission bm;

	// Use this for initialization

	// Update is called once per frame
	void Update () {
        //interested.text = "Interested: x" + bm.interactingAgents.Keys.Count;
	}
    
}
