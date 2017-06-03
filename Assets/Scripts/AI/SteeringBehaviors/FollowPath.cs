using UnityEngine;
using System.Collections.Generic;

public class FollowPath : SteeringBehavior {
    public float followRadius = 1.0f, wFollow = 0.0f, stopDistance = 0.0f;
    public Vector3 destiny;
    bool active = false;
    public Vector3 objectivePosition;
    GraphManager graphManager;
    PathFindingManager pathFinding;
    Route route;

    public float wAvoidance = 0f, maxSee = 1f, maxAvoidForce = 1;
    public LayerMask enemyLayerMask = 1 << 8;
    public LayerMask buildingLayerMask = 1 << 8;
    public LayerMask tileLayerMask = 1 << 8;


    // Use this for initialization
    void Start () {
        //route = new List<Node>();
        pathFinding = FindObjectOfType<PathFindingManager>();
	}

    public override void sumForces()
    {
        //setDestiny(new Vector3(10, -10, 0));
        base.sumForces();
        if (active)
        {
            force += wFollow * Follow();
            force += wAvoidance * AvoidCollision();
        }
    }

    void OnDrawGizmosSelected()
    {
        if(active && route != null && route.hasPath() && route.route != null)
        {
            foreach(Node n in route.route)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(n.transform.position, 0.1f);
            }
        }
    }

    Vector3 AvoidCollision()
    {
        Vector3 topahead, ahead, bottomahead;
        
        Vector3 avoidanceForce = Vector3.zero;
        ahead = velocity.normalized * maxSee;
        topahead = Quaternion.Euler(0, 0, 20) * velocity.normalized * maxSee /2;
        bottomahead = Quaternion.Euler(0, 0, -20) * velocity.normalized * maxSee /2;
        //Create the avoidance rays
        RaycastHit2D topHit =  Physics2D.Raycast(transform.position, topahead, maxSee, buildingLayerMask);
        RaycastHit2D centralHit = Physics2D.Raycast(transform.position, ahead, maxSee, buildingLayerMask);
        RaycastHit2D bottomHit = Physics2D.Raycast(transform.position, bottomahead, maxSee, buildingLayerMask);

        //Draw the rays
        Debug.DrawRay(transform.position, topahead);
        Debug.DrawRay(transform.position, ahead);
        Debug.DrawRay(transform.position, bottomahead);

        //Tests if a ray detected anything. If so, calculate and add new "force"
        if (topHit)
        {
            avoidanceForce -= (topahead - new Vector3(topHit.point.x, topHit.point.y, 0.0f)) ;
        }
        if (centralHit)
        {
            avoidanceForce -= (ahead - new Vector3(centralHit.point.x, centralHit.point.y, 0.0f));
        }
        if (bottomHit)
        {
            avoidanceForce -= (bottomahead - new Vector3(bottomHit.point.x, bottomHit.point.y, 0.0f));
        }
        return avoidanceForce.normalized * maxAvoidForce;
    }

    Vector3 Follow() {
        if (active)
        {
            if (route == null)
                return Vector3.zero;
            else if (!route.hasPath())
            {
                //Waiting for the pathFindingManager
                return Vector3.zero;
            }
            else if(route.route == null)
            {
                //Debug.Log("Dont have a route to this location =/");
                abandonPath();
                return Vector3.zero;
            }
            else{
                float d = Vector3.Distance(transform.position, route.route[route.routeIndex].transform.position);
                if (d <= followRadius)
                    route.nextNode();
                if (route.hasPathEnd())
                {
                    abandonPath();
                    return Vector3.zero;
                }
            }
            return Seek(route.route[route.routeIndex].transform.position);
        }
        return Vector3.zero;
    }

    public void setDestiny(Vector3 destinyPosition)
    {
        route = pathFinding.newRequest(transform.position, destinyPosition, Priority.heroRequest);
        if(route == null)
        {
            Stay();
            active = false;
            route = null;
            pathFinding.removeActiveRoute(route);
            return;
        }
        active = true;
    }

    public bool hasPathEnded()
    {
        if (route == null)
            return true;
        return true;
    }

    public bool hasPath()
    {
        return active;
    }
    public void Stop()
    {
        active = false;
    }

    public void abandonPath() {
        active = false;
        Stay();
        pathFinding.removeActiveRoute(route);
        route = null;
    }
}
