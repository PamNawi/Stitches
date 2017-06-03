using UnityEngine;
public class CollisionAvoidance : SteeringBehavior
{
    public float wAvoidance, maxSee = 1f, maxAvoidForce = 1f;
    Vector3 ahead;

    public override void sumForces()
    {
        //setDestiny(new Vector3(10, -10, 0));
        base.sumForces();
    }

    Vector3 AvoidCollision()
    {
        Vector3 avoidanceForce = Vector3.zero ;
        ahead = transform.position + velocity.normalized * maxSee;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, ahead);
        Debug.DrawRay(transform.position, ahead);
        //if (hit)
        //{
        //    avoidanceForce = (ahead - hit.transform.position).normalized * maxAvoidForce;
        //}
        return avoidanceForce;
    }
}
