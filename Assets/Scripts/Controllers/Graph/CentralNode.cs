using UnityEngine;
using System.Collections.Generic;


public class CentralNode : Node
{
    public bool activeSpawner = true;
    const int maxConections = 15;

    public GameObject Spawner;

    public new void Awake()
    {
        base.Awake();

        Spawner = transform.GetChild(0).gameObject;
        if (activeSpawner)
        {
            Spawner.SetActive(true);
        }
        else
        {
            Spawner.SetActive(false);
        }
    }

    public override void addNeighbor(Node neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
            GameObject Spawner = transform.GetChild(0).gameObject;
            if (Neighbors.Count < maxConections)
                //If is not fully conected is a border and then have to active the Spawner;
                Spawner.SetActive(true);
            else
                Spawner.SetActive(false);
        }
    }

    public override bool removeNeighbor(Node neighbor)
    {
        GameObject Spawner = transform.GetChild(0).gameObject;
        bool b = Neighbors.Remove(neighbor);
        if (activeSpawner) { 
            if (Neighbors.Count < maxConections)
                //If is not fully conected is a border and then have to active the Spawner;
                Spawner.SetActive(true);
            else
                Spawner.SetActive(false);
        }
        return b;
    }

    public void unsewBuilding()
    {
        GameObject Spawner = transform.GetChild(0).gameObject;
        if (activeSpawner) {
            if ( Neighbors.Count < maxConections)
                //If is not fully conected is a border and then have to active the Spawner;
                Spawner.SetActive(true);
            else
                Spawner.SetActive(false);
        }
    }

    public void sewBuilding()
    {
        GameObject Spawner = transform.GetChild(0).gameObject;
        if(activeSpawner)
            Spawner.SetActive(false);
    }

}