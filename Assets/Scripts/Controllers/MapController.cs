using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapController : MonoBehaviour {

    [HideInInspector] public TerrainData terrainData;
    [HideInInspector] public Atractiviness atractiviness;
    GraphManager graphManager;
    SmartObjectManager smartObjectsManager;
    public EconomyController economyController;
    public LayerMask enemyLayerMask = 1 << 8;
    public LayerMask buildingLayerMask = 1 << 8;
    public LayerMask tileLayerMask = 1 << 8;

    void Awake () {
        terrainData = gameObject.GetComponent<TerrainData>();
        graphManager = GetComponentInChildren<GraphManager>();

        smartObjectsManager = transform.parent.GetComponentInChildren<SmartObjectManager>();
        atractiviness = new Atractiviness();
	}

    public void nextRound(uint round) { }

    public void createNewObject(GameObject gObject, Vector3 position)
    {
        if (isValidPositionToCreate(gObject, position))
        {
            position.z = 0;
            economyController.knitThis(gObject);
            switch (gObject.tag)
            {
                case "Building":
                    gObject.GetComponent<PlaceableBuilding>().sew();
                    atractiviness.calculateAtractiviness();
                    smartObjectsManager.addSmartObject(gObject);
                    break;
                case "Tile":
                    graphManager.addNewTile(gObject.GetComponent<TileManager>());
                    break;
            }
        }
    }

    public bool isValidPositionToCreate(GameObject building, Vector3 position)
    {
        if (building.tag != "Tile")
        {
            PlaceableBuilding placeableBuilding = building.GetComponent<PlaceableBuilding>();
            if (!placeableBuilding)
                return false;
            //Just verify if there's is a tile on the ground...
            if ((placeableBuilding.colliders.Count <= 0 && placeableBuilding.tiles.Count > 0))//placeableBuilding.tile))
            {
                //If is over only a placeable tiles
                foreach(GameObject tile in placeableBuilding.tiles)
                {
                    TileManager tManager = tile.GetComponent<TileManager>();
                    if (!tManager.placeable)
                        return false;
                }
                Bounds bounds = placeableBuilding.gameObject.GetComponent<SpriteRenderer>().bounds;
                List<Vector3> corners = new List<Vector3>();
                corners.Add(new Vector3(bounds.min.x, bounds.max.y, 0.0f)); //Top Left
                corners.Add(new Vector3(bounds.max.x, bounds.max.y, 0.0f)); //Top Right
                corners.Add(new Vector3(bounds.min.x, bounds.min.y, 0.0f)); //Bottom Left
                corners.Add(new Vector3(bounds.max.x, bounds.min.y, 0.0f)); //Bottom Right
                foreach (Vector3 corner in corners)
                {
                    RaycastHit2D hit = Physics2D.Raycast(corner, Vector2.zero, Mathf.Infinity, tileLayerMask);
                    if (!hit)
                        return false;
                }
                return true;
            }
        }
        else
        {
            //Verify if don't have a tile on the ground...
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward , Mathf.Infinity, tileLayerMask);
            TileManager otherTile = graphManager.getTile(position);
            if (hit && hit.collider.tag == "Tile" || otherTile)
            {
                return false;
            }
            //Verify if will be neighbours
            //Local para otimização ???? Ainda preferiria otimizar na IA dos agentes.
            TileManager d1, up, d2, left, right, d3, down, d4;
            d1 = graphManager.getNeighborTile(position, TilePos.d1);
            up = graphManager.getNeighborTile(position, TilePos.up);
            d2 = graphManager.getNeighborTile(position, TilePos.d2);
            left = graphManager.getNeighborTile(position, TilePos.left);
            right = graphManager.getNeighborTile(position, TilePos.right);
            d3 = graphManager.getNeighborTile(position, TilePos.d3);
            down = graphManager.getNeighborTile(position, TilePos.down);
            d4 = graphManager.getNeighborTile(position, TilePos.d4);

            if (d1 || up || d2 || left || right || d3 || down || d4)
                return true;
        }
        return false;
    }

    public bool canUnravelTile(GameObject gObject)
    {
        TileManager tileManager = gObject.GetComponent<TileManager>();
        //Verify if is supporting some building
        if(tileManager.colliders.Count > 0)
        {
            return false;
        }
        //Verify if is the last tile?
        return true;
    }

    public void Unravel(GameObject gObject)
    {
        if (gObject.tag == "Tile")
                graphManager.removeTile(gObject);
        else if(gameObject.tag == "Building")
        {
            gObject.GetComponent<PlaceableBuilding>().unsew();
            smartObjectsManager.removeSmartObject(gObject.GetComponent<SmartObject>());
        }
        economyController.unravelThis(gObject);
        atractiviness.calculateAtractiviness();
        Destroy(gObject);
    }

    #region Log
    #endregion
}
