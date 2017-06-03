using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public struct GridPosition
{
    public int x, y, idnode;
}

public class Node : MonoBehaviour
{
    public List<Node> Neighbors;
    GridPosition gridPosition;
    public float weight;
    public bool iswalkable = true;
    public bool active;

    public void Awake()
    {
        Neighbors = new List<Node>();
        gridPosition.x = -1;
        gridPosition.y = -1;
        gridPosition.idnode = -1;
        weight = 1;

        active = iswalkable;
    }
    #region get/set data
    public void setGridPosition(GridPosition gPosition)
    {
        gridPosition = gPosition;
    }

    public GridPosition getGridPosition()
    {
        return gridPosition;
    }

    public virtual void addNeighbor(Node neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
        }
    }

    public virtual bool removeNeighbor(Node neighbor)
    {
        bool b = Neighbors.Remove(neighbor);
        return b;
    }

    public bool isNeighbor(Node neighbor)
    {
        return Neighbors.Contains(neighbor);
    }

    public float getWeight()
    {
        return weight;
    }

    public List<Node> getNeighbors()
    {
        //removeNullNeighbors();
        return Neighbors;
    }

    public void removeNullNeighbors()
    {
        Neighbors.RemoveAll(null);
    }
    #endregion

    void OnDestroy()
    {
        //Debug.Log("Im dying:" + gridPosition.idnode.ToString() + " > " + gridPosition.x.ToString() + "," + gridPosition.y.ToString());
        foreach (Node neig in Neighbors)
        {
            neig.removeNeighbor(this);
        }
    }

    #region Obstruction manager
    public virtual void OnTriggerEnter2D(Collider2D c)
    {
        sew(c.gameObject);
    }

    public virtual void OnTriggerExit2D(Collider2D c)
    {
        this.unsew(c.gameObject);

    }
    public virtual void unsew(GameObject gObject)
    {
        if (weight != 1 && iswalkable)
        {
            switch (gObject.tag)
            {
                case "Building":
                    weight = 1;
                    active = true;
                    break;
                default:
                    break;
            }
        }
    }

    public virtual void sew(GameObject gObject)
    {
        switch (gObject.tag){
                case "Building":
                    weight = 99999;
                    active = false;
                    break;
                default:
                    break;
        }
    }

    #endregion
}