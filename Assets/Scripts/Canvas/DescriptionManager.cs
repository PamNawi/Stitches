using UnityEngine;
using UnityEngine.UI;

public class DescriptionManager : MonoBehaviour {
    // Use this for initialization
    public Text Title;
    public Text Description;
    Animator anim;
    void Awake () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void showDescription(GameObject gObject)
    {
        //Set title, description and recipe
        PlaceableBuilding placeableBuilding = gObject.GetComponent<PlaceableBuilding>();
        TileManager tileManager = gameObject.GetComponent<TileManager>();
        if(placeableBuilding != null)
        {
            Title.text = placeableBuilding.name;
            Description.text = placeableBuilding.buildingDescription;
        }
        else
        {
            Title.text = gObject.name;
            Description.text = "";
        }
        //Active the animation
        anim.SetInteger("Showing", 1);
    }

    public void hide()
    {
        anim.SetInteger("Showing", 0);
    }
}
