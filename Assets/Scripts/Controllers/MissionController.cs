using UnityEngine;
using System.Collections.Generic;

public class MissionController : MonoBehaviour {
    public GameObject attackMission, defenseMission, exploreMission;
    GameObject selectedMission = null;
    SmartObjectManager soManager;

    public LayerMask enemyLayerMask = 1 << 8;
    public LayerMask buildingLayerMask = 1 << 8;
    public LayerMask tileLayerMask = 1 << 8;

    // Use this for initialization
    void Start () {
        soManager = transform.parent.GetComponentInChildren<SmartObjectManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            bool attack, defense, explore;
            attack = Input.GetButton("Attack");
            defense = Input.GetButton("Defense");
            explore = Input.GetButton("Exploration");

            if (attack)
                createAttackMission();
            else if (defense)
                createDefenseMission();
            else if (explore)
                createExplorationMission();
        }

        if (this.selectedMission)
        {
            //bool raiseValue = Input.GetButton("RaiseValue");
            //if (raiseValue)
                //this.raiseMissionValue(100);
        }
    }
    public void createAttackMission()
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), enemyLayerMask);
        GameObject enemy, newMission;
        AttackMission attackMission; 
        if (hitCollider != null && hitCollider.tag == "Enemy")
        {
            enemy = hitCollider.gameObject;
            if (enemy.GetComponentInChildren<AttackMission>() == null)
            {
                //Create a new Attack mission
                newMission = Instantiate(this.attackMission, enemy.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject;
                attackMission = newMission.GetComponent<AttackMission>();
                //newMission.transform.parent = enemy.transform;
                //attackMission.target = enemy;
                //selectedMission = newMission;
                soManager.addSmartObject(attackMission);
            }
        }
    }
    public void createDefenseMission()
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), buildingLayerMask);
        GameObject building, newMission;
        if (hitCollider != null && hitCollider.tag == "Building")
        {
            building = hitCollider.gameObject;
            if (building.GetComponentInChildren<DefenseMission>() == null)
            {
                newMission = Instantiate(defenseMission, building.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject;
                newMission.transform.parent = building.transform;
                //newMission.GetComponent<DefenseMission>().target = building;
                //newMission.GetComponent<DefenseMission>().raiseValue(100);

                DefenseMission dm = newMission.GetComponent<DefenseMission>();
                selectedMission = newMission;
                soManager.addSmartObject(dm);
            }
        }
    }
    public void createExplorationMission()
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        GameObject newMission;
        ExplorationMission explorationMission;
        Vector2 clickedPosition;
        if (hitCollider != null && (hitCollider.tag == "Tile" || hitCollider.tag == "Node"))
        {

            clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newMission = Instantiate(exploreMission, clickedPosition, Quaternion.identity) as GameObject;
            explorationMission = newMission.GetComponent<ExplorationMission>();
            selectedMission = newMission;
            soManager.addSmartObject(explorationMission);
        }
    }

}
