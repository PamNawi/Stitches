using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEditor;


[Serializable]
public class StatusSymbols:SerializableDictionary<StatusEnum, GameObject>
{
    public StatusSymbols()
    {
        for(StatusEnum s = 0; s != StatusEnum.endEnum; s++)
        {
            this.Add(s, null);
        }
    }
}
[CustomPropertyDrawer(typeof(StatusSymbols))]
public class StatusSymbolsDrawer : DictionaryDrawer<StatusEnum, GameObject> { }

public class BuildingStatusInspector : MonoBehaviour {

    public Text buildingName;
    public GameObject building;
    PlaceableBuilding placeable;
    SmartObject bSmartObject;

    public GameObject visitorsInfo;
    public GameObject attractionInfo;

    public HeroGameObjectDictionary heroSymbols;
    public StatusSymbols statusSymbols;

    public float maxStatusEffect = 10.0f;

    public void showBuilding(GameObject building)
    {
        buildingName.text = "";
        placeable = building.GetComponent<PlaceableBuilding>();
        bSmartObject = building.GetComponent<SmartObject>();
        if(bSmartObject != null)
        {

            visitorsInfo.SetActive(true);
            if (bSmartObject.soName != "")
                buildingName.text = bSmartObject.soName;
            updateStatusArrowsIndicators();
            updateInsideVisitors();
            
        }
        else
            visitorsInfo.SetActive(false);

        if(placeable != null)
        {
            attractionInfo.SetActive(true);
            foreach(HeroClass hc in placeable.heroAtractiviness.Keys)
            {
                if(placeable.heroAtractiviness[hc] != 0)
                    heroSymbols[hc].SetActive(true);
                else
                    heroSymbols[hc].SetActive(false); 
            }
            if (buildingName.text == "" && placeable.buildingName != "")
                buildingName.text = placeable.buildingName;
        }
        else
            attractionInfo.SetActive(false);
    }

    public void updateStatusArrowsIndicators()
    {
        //RawImage[] arrows;
        ////Update status arrows indicators
        //for (StatusEnum s = 0; s != StatusEnum.endEnum; s++)
        //{
            //Get all textures
            //Ignores the first on the list because is the status symbol icon
            //arrows = statusSymbols[s].GetComponentsInChildren<RawImage>();
            //if (smartObject.statusEffect[s] == 0)
            //{
            //    arrows[1].transform.localScale = new Vector3(0, 0, 0);
            //    arrows[2].transform.localScale = new Vector3(0, 0, 0);
            //}
            //else if (visitable.statusEffect[s] > 0)
            //{
            //    arrows[2].transform.localScale = new Vector3(0, 0, 0);
            //    if (visitable.statusEffect[s] <= 1 / 3 * maxStatusEffect)
            //    {
            //        arrows[1].uvRect = new Rect(0, 0, 1, 1);
            //        arrows[1].transform.localScale = new Vector3(1, 1, 1);
            //    }
            //    else if (visitable.statusEffect[s] <= 2 / 3 * maxStatusEffect)
            //    {
            //        arrows[1].uvRect = new Rect(0, 0, 2, 1);
            //        arrows[1].transform.localScale = new Vector3(2, 1, 1);
            //    }
            //    else
            //    {
            //        arrows[1].uvRect = new Rect(0, 0, 3, 1);
            //        arrows[1].transform.localScale = new Vector3(3, 1, 1);
            //    }
            //}
            //else
            //{
            //    arrows[1].transform.localScale = new Vector3(0, 0, 0);
            //    if (visitable.statusEffect[s] >= -1 / 3 * maxStatusEffect)
            //    {
            //        arrows[2].uvRect = new Rect(0, 0, 1, 1);
            //        arrows[2].transform.localScale = new Vector3(1, 1, 1);
            //    }
            //    else if (visitable.statusEffect[s] >= -2 / 3 * maxStatusEffect)
            //    {
            //        arrows[2].uvRect = new Rect(0, 0, 2, 1);
            //        arrows[2].transform.localScale = new Vector3(2, 1, 1);
            //    }
            //    else
            //    {
            //        arrows[2].uvRect = new Rect(0, 0, 3, 1);
            //        arrows[2].transform.localScale = new Vector3(3, 1, 1);
            //    }
            //}

        //}
    }

    public void updateInsideVisitors()
    {
        GameObject visitorsInfo = transform.GetChild(0).gameObject;
    }

    public void showNature(GameObject building)
    {
        buildingName.text = building.name;
        placeable = building.GetComponent<PlaceableBuilding>();
        visitorsInfo.SetActive(false);
        attractionInfo.SetActive(false);
    }
}
