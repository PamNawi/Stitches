using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InspectorController : MonoBehaviour {
    public GameObject agentTemplate;
    public GameObject buildingTemplate;
    public GameObject missionTemplate;

    public GameObject unravelButton;

    public GameObject selectedGameObject;
    public Text inspectorText;

    [HideInInspector] public enum InspectorTemplate { None, Agent, Building, Mission };

    private InspectorTemplate showingTemplate;
    // Use this for initialization
    void Start () {
        showingTemplate = InspectorTemplate.None;
        showNothing();
    }
    // Update is called once per frame
    void Update()
    {
        GameObject selected = null;
        Collider2D[] hitColliderList;
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            hitColliderList = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (hitColliderList.Length > 0)
            {
                foreach (Collider2D hitCollider in hitColliderList)
                {
                    selected = hitCollider.gameObject as GameObject;
                    selectedGameObject = selected;
                    switch (hitCollider.tag)
                    {
                        case "Mission":
                            showInfoMission(selected);
                            return;
                        case "Building":
                            showInfoBuilding(selected);
                            return;
                        case "Hero":
                            showInfoAgent(selected);
                            return;
                        case "Tile":
                            showAnythingElse(selected);
                            return;
                        case "UI":
                            return;
                        case "Nature":
                            showInfoNature(selected);
                            return;
                        default:
                            continue;
                    }
                }
            }
            selectedGameObject = null;
            showNothing();
        }
    }
    public void showInfoAgent(GameObject agent) {
        showingTemplate = InspectorTemplate.Agent;

        agentTemplate.SetActive(true);
        StatusInspector si = GetComponentInChildren<StatusInspector>();

        missionTemplate.SetActive(false);
        buildingTemplate.SetActive(false);
        inspectorText.text = "";
        si.showHeroState(agent);
        unravelButton.SetActive(false);
    }

    public void showInfoBuilding(GameObject building){
        showingTemplate = InspectorTemplate.Building;

        buildingTemplate.SetActive(true);
        missionTemplate.SetActive(false);
        agentTemplate.SetActive(false);

        if (willShowUnravelButton(building))
            unravelButton.SetActive(true);
        else
            unravelButton.SetActive(false);

        inspectorText.text = "";
        BuildingStatusInspector bi = GetComponentInChildren<BuildingStatusInspector>();
        bi.showBuilding(building);

    }
    public void showInfoNature(GameObject natureObject)
    {
        showingTemplate = InspectorTemplate.Building;

        buildingTemplate.SetActive(true);
        missionTemplate.SetActive(false);
        agentTemplate.SetActive(false);

        if (willShowUnravelButton(natureObject))
            unravelButton.SetActive(true);
        else
            unravelButton.SetActive(false);

        inspectorText.text = "";
        BuildingStatusInspector bi = GetComponentInChildren<BuildingStatusInspector>();
        bi.showNature(natureObject);
    }

    public void showInfoMission(GameObject mission)
    {
        showingTemplate = InspectorTemplate.Mission;

        missionTemplate.SetActive(true);
        MissionStatusInspector mi = missionTemplate.GetComponent<MissionStatusInspector>();

        agentTemplate.SetActive(false);
        buildingTemplate.SetActive(false);

        mi.bm = mission.GetComponent<BasicMission>();
        if (willShowUnravelButton(mission))
            unravelButton.SetActive(true);
        else
            unravelButton.SetActive(false);

        inspectorText.text = "";
    }

    public void showNothing()
    {
        showingTemplate = InspectorTemplate.None;

        missionTemplate.SetActive(false);
        agentTemplate.SetActive(false);
        buildingTemplate.SetActive(false);
        unravelButton.SetActive(false);

        inspectorText.text = "";
    }

    public void showAnythingElse(GameObject gObject)
    {
        showingTemplate = InspectorTemplate.None;

        missionTemplate.SetActive(false);
        agentTemplate.SetActive(false);
        buildingTemplate.SetActive(false);

        if (willShowUnravelButton(gObject))
            unravelButton.SetActive(true);
        else
            unravelButton.SetActive(false);

        inspectorText.text = gObject.tag;
    }

    public bool willShowUnravelButton(GameObject selected)
    {
        if(selected.tag == "Tile")
            return transform.parent.GetComponent<CanvasController>().mapController.canUnravelTile(selectedGameObject);
        else if (selected.tag == "Building" || selected.tag == "Nature")
            return selected.GetComponent<PlaceableBuilding>().unravel;
        else
            return false;
    }

    public void Unravel()
    {
        CanvasController cvController = transform.parent.GetComponent<CanvasController>();
        cvController.mapController.Unravel(selectedGameObject);
        cvController.Unravel(selectedGameObject);
        showNothing();
    }
}
