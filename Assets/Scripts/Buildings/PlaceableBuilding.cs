using UnityEngine;
using System.Collections.Generic;

public class PlaceableBuilding : MonoBehaviour {

    [HideInInspector] public List<Collider2D> colliders = new List<Collider2D>();
    public List<GameObject> tiles = new List<GameObject>();
    public string buildingName;
    public string buildingDescription;
    public HeroFloatDictionary heroAtractiviness;
    public bool unravel = true;
    public bool sewed = false;
    // Use this for initialization
    void Start () {
        //heroAtractiviness = new HeroAtractiviness();
	}
	
	// Update is called once per frame
	void Update () {
	}


    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Building")
            colliders.Add(c);
        else if (c.tag == "Tile")
            tiles.Add(c.transform.gameObject);
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.tag == "Building")
            colliders.Remove(c);
        if (c.tag == "Tile")
            tiles.Remove(c.transform.gameObject);
    }

    public void unsew()
    {
        sewed = false;
        foreach (GameObject tile in tiles)
            tile.GetComponent<TileManager>().unsew(this.gameObject);
    }

    public void sew()
    {
        sewed = true;
        foreach (GameObject tile in tiles)
            tile.GetComponent<TileManager>().sew(this.gameObject);
    }
}
