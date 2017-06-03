using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

public enum Priority { super, monsterRequest, heroRequest,other, enumEnd };

public class Route
{
    public Vector3 startPoint, endPoint;
    public Node startNode, endNode;
    public List<Node> route;
    public int routeIndex = 0;
    public bool active; // If is active, FollowPath can use this route
    public Priority priority = Priority.enumEnd;

    public void nextNode()
    {
        routeIndex++;
        if(hasPathEnd())
        {
            active = false;
        }
    }

    public bool hasPath()
    {
        return active;
    }

    public bool hasPathEnd()
    {
        if (routeIndex >= route.Count)
            return true;
        return false;
    }
}

[Serializable]
public class PriorityDictionary: SerializableDictionary<Priority, List<Route>>
{
    public PriorityDictionary()
    {
        for(Priority p = 0; p != Priority.enumEnd; p++)
        {
            Add(p, new List<Route>());
        }
    }

}

[CustomPropertyDrawer(typeof(PriorityDictionary))]
public class PriorityDictionaryDrawer : DictionaryDrawer<Priority, List<Route>> { }

public class PathFindingManager : MonoBehaviour {

    PriorityDictionary routes;
    public int N;
    public int Z;
    GraphManager graphManager;
    public List<Route> activeRoutes = new List<Route>();
	// Use this for initialization
	void Start () {
        routes = new PriorityDictionary();
        graphManager = FindObjectOfType<GraphManager>();
    }
	
	// Update is called once per frame
	void Update () {
        int i = 0;
        Route r;
        for (Priority p = 0; p != Priority.enumEnd;)
        {
            if (routes[p].Count <= 0)
            {
                p++;
                continue;
            }
            //Get the first, calculate the route and active
            r = routes[p][0];

            r.startNode = graphManager.foundNearestNode(r.startPoint);
            r.endNode = graphManager.foundNearestNode(r.endPoint);
            r.route = graphManager.AStar(r.startNode, r.endNode);
            
            //Remove from the dictionary and put on the list of actives;
            routes[p].RemoveAt(0);
            activeRoutes.Add(r);

            r.active = true;
            i++;
            if (i >= N)
                break;
        }
	}

    public Route newRequest(Vector3 startPoint, Vector3 endPoint, Priority p)
    {
        Route r = new Route();
        r.active = false;
        r.startPoint    = startPoint;
        r.endPoint      = endPoint;
        r.priority = p;
        routes[p].Add(r);
        return r;
    }
    
    public void recalculateAll()
    {
        foreach(Route r in activeRoutes)
        {

            routes[r.priority].Add(r);
            if (r.route.Count <= Z)
            {
                r.active = false;
            }
        }
        activeRoutes.RemoveRange(0, activeRoutes.Count);
    }

    public void removeActiveRoute(Route r)
    {
        activeRoutes.Remove(r);
    }
}
