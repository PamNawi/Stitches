using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum TilePos { d1, up, d2, left, center, right, d3, down, d4, enumEnd };

public class TileManager : MonoBehaviour {

    GameObject[] gObjectsNodes;
    public List<Collider2D> colliders = new List<Collider2D>();
    Vector2 tilePositionGrid;
    Node[] nodes;
    const int nNodes = 5;

    public bool walkable = true;
    public bool placeable = true;

    public List<GameObject> buildings;

    // Use this for initialization
    void Awake () {
        buildings = new List<GameObject>();
        nodes = new Node[nNodes];
        gObjectsNodes = new GameObject[nNodes];
        for (int i = 0; i < nNodes; i++)
        {
            gObjectsNodes[i] = transform.GetChild(i).gameObject;
            nodes[i] = transform.GetChild(i).GetComponent<Node>();
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setNodesPositions()
    {
        
        GridPosition gPosition;
        gPosition.x = (int)this.tilePositionGrid.x;
        gPosition.y = (int)this.tilePositionGrid.y;
        for (int i = 0; i < nNodes; i++)
        {
            gPosition.idnode = i;
            nodes[i].setGridPosition(gPosition);
        }
    }

    public void setTilePositionGrid(int i, int j)
    {
        this.setTilePositionGrid(new Vector2((int)i, (int)j));
    }

    public void setTilePositionGrid(Vector2 vec)
    {
        tilePositionGrid = vec;
        setNodesPositions();
    }

    public Node[] getNodes()
    {
        return nodes;
    }

    #region Create Edges

    public void createInternEdges()
    {
        //Node 1
        nodes[0].addNeighbor(nodes[1]);
        nodes[0].addNeighbor(nodes[2]);
        nodes[0].addNeighbor(nodes[nNodes-1]);

        //Node 2
        nodes[1].addNeighbor(nodes[0]);
        nodes[1].addNeighbor(nodes[3]);
        nodes[1].addNeighbor(nodes[nNodes - 1]);

        //Node 3
        nodes[2].addNeighbor(nodes[0]);
        nodes[2].addNeighbor(nodes[3]);
        nodes[2].addNeighbor(nodes[nNodes - 1]);

        //Node 4
        nodes[3].addNeighbor(nodes[1]);
        nodes[3].addNeighbor(nodes[2]);
        nodes[3].addNeighbor(nodes[nNodes - 1]);

        //Node 5
        nodes[4].addNeighbor(nodes[0]);
        nodes[4].addNeighbor(nodes[1]);
        nodes[4].addNeighbor(nodes[2]);
        nodes[4].addNeighbor(nodes[3]);
    }

    public void createExternEdges(TileManager d1, TileManager up, TileManager d2, TileManager left, TileManager right, TileManager d3, TileManager down, TileManager d4)
    {
        if (up != null)
        {
            //Node 1
            createEdgeBetweenTiles(1, up, 3);
            createEdgeBetweenTiles(1, up, 4);
            createEdgeBetweenTiles(1, up, 5);

            //Node 2
            createEdgeBetweenTiles(2, up, 3);
            createEdgeBetweenTiles(2, up, 4);
            createEdgeBetweenTiles(2, up, 5);

            //Node 5
            createEdgeBetweenTiles(5, up, 5);
            createEdgeBetweenTiles(5, up, 3);
            createEdgeBetweenTiles(5, up, 4);
        }

        if (left != null)
        {
            //Node 1
            createEdgeBetweenTiles(1, left, 2);
            createEdgeBetweenTiles(1, left, 4);
            createEdgeBetweenTiles(1, left, 5);

            //Node 3
            createEdgeBetweenTiles(3, left, 2);
            createEdgeBetweenTiles(3, left, 4);
            createEdgeBetweenTiles(3, left, 5);

            //Node 5
            createEdgeBetweenTiles(5, left, 2);
            createEdgeBetweenTiles(5, left, 4);
            createEdgeBetweenTiles(5, left, 5);
        }

        if (right != null)
        {
            //Node 2
            createEdgeBetweenTiles(2, right, 1);
            createEdgeBetweenTiles(2, right, 5);
            createEdgeBetweenTiles(2, right, 3);

            //Node 4
            createEdgeBetweenTiles(4, right, 1);
            createEdgeBetweenTiles(4, right, 3);
            createEdgeBetweenTiles(4, right, 5);

            //Node 5
            createEdgeBetweenTiles(5, right, 1);
            createEdgeBetweenTiles(5, right, 5);
            createEdgeBetweenTiles(5, right, 3);
        }

        if (down != null)
        {
            //Node 3
            createEdgeBetweenTiles(3, down, 1);
            createEdgeBetweenTiles(3, down, 5);
            createEdgeBetweenTiles(3, down, 2);

            //Node 4
            createEdgeBetweenTiles(4, down, 1);
            createEdgeBetweenTiles(4, down, 2);
            createEdgeBetweenTiles(4, down, 5);

            //Node 5
            createEdgeBetweenTiles(5, down, 1);
            createEdgeBetweenTiles(5, down, 5);
            createEdgeBetweenTiles(5, down, 2);
        }

        if (d1 != null)
            createEdgeBetweenTiles(1, d1, 4);


        if(d2 != null)
            createEdgeBetweenTiles(2, d2, 3);

        if(d3 != null)
            createEdgeBetweenTiles(3, d3, 2);

        if(d4!= null)
            createEdgeBetweenTiles(4, d4, 1);
    }

    public void createEdgeBetweenTiles(int idNode, TileManager tile, int idNodeOtherTile)
    {
        nodes[idNode - 1].addNeighbor(tile.nodes[idNodeOtherTile-1]);
        tile.nodes[idNodeOtherTile - 1].addNeighbor(nodes[idNode - 1]);
    }

    #endregion

    #region Destroy Edges
    public void DestroyEdge(int idNode, TileManager tile, int idNodeOtherTile) {
        nodes[idNode - 1].removeNeighbor(tile.nodes[idNodeOtherTile - 1]);
        tile.nodes[idNodeOtherTile - 1].removeNeighbor(nodes[idNode - 1]);
    }

    public void removeNullEdges()
    {
        foreach(Node n in nodes)
        {
            n.removeNullNeighbors();
        }
    }

    #endregion

    #region Obstruction manager

    public void unsew(GameObject gObject)
    {
        CentralNode centralNode = transform.GetComponentInChildren<CentralNode>();
        if (gObject.tag == "Building")
        {
            buildings.Remove(gObject);
            colliders.Remove(gObject.GetComponent<Collider2D>());
            centralNode.unsewBuilding();
        }
        //foreach(Node n in nodes)
        //{
        //    n.unsew(gObject);
        //}
    }

    public void sew(GameObject gObject)
    {
        CentralNode centralNode = transform.GetComponentInChildren<CentralNode>();
        if (gObject.tag == "Building")
        {

            buildings.Add(gObject);
            colliders.Add(gObject.GetComponent<Collider2D>());
            centralNode.sewBuilding();
        }
        //foreach (Node n in nodes)
        //{
        //    n.sew(gObject);
        //}
    }
    #endregion

}
