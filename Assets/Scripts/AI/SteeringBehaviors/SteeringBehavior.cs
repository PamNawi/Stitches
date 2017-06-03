using UnityEngine;
using System.Collections.Generic;


enum movementDirection { right, d1, up, d2, left, d3, down, d4};

public class SteeringBehavior: MonoBehaviour
{
    //Basic Variables
    public float maxVelocity = 10.0f, mass = 1.0f;
    public float slowingRadius = 10;

    //Behaviors Weight
    public float wSeek = 0, wFlee = 0, wArrival = 0;
    public float wWander = 0, wPursuit = 0, wEvade = 0, wSwarm = 0;
    public float wAlign = 0, wCohese = 0, wSeparate = 0, wGroupBehaviour = 0;

    //Specific Variables
    public float wanderAngle = 1.0f, wanderingRadius = 1.0f, wanderingDistance = 10.0f, angleChange = 5.0f;
    public GameObject ePursuit = null, eEvade = null;
    public Vector3 seekPosition = Vector3.zero, fleePosition = Vector3.zero, arrivalPosition = Vector3.zero;

    public Vector3 runningPosition = Vector3.zero, swarmForce = Vector3.zero;
    [HideInInspector] public Vector3 force, aceleration, velocity;
    FlockingSensor fl;

    void Awake()
    {
        fl = GetComponentInChildren<FlockingSensor>();
    }

    void FixedUpdate()
    {
        if (enabled)
        {
            force = Vector3.zero;
            sumForces();
            aceleration = force / mass;
            velocity += aceleration;
            if (velocity.magnitude > maxVelocity)
                velocity = velocity.normalized * maxVelocity;
            transform.position = transform.position + velocity;
        }
    }

    public void Stay()
    {
        velocity = Vector3.zero;
        force = Vector3.zero;
        aceleration = Vector3.zero;
    }

    public virtual void sumForces()
    {
        force =     wSeek       * Seek(seekPosition);
        force +=    wFlee       * Flee(fleePosition);
        force +=    wArrival    * Arrival(arrivalPosition);
        force +=    wWander     * Wander();
        force +=    wPursuit    * Pursuit(ePursuit);
        force +=    wEvade      * Evade(eEvade);
        //force +=    wGroupBehaviour * GroupBehaviour();
        //force +=    wAlign      * Align(heroesST);
        //force +=    wCohese     * Cohese(heroesST);
        //force +=    wSeparate   * Separate(heroesST);
    }

    public Vector3 Seek(Vector3 position)
    {
        return (position - transform.position).normalized * maxVelocity;
    }

    public Vector3 Flee(Vector3 position)
    {
        return (transform.position - position).normalized * maxVelocity;
    }

    public Vector3 Arrival(Vector3 position)
    {
        Vector3 desired = position - transform.position;
        float distance = desired.magnitude;
        if (distance <= slowingRadius)
            desired = desired.normalized * (maxVelocity * distance / slowingRadius);
        else
            desired = desired.normalized * maxVelocity;
        return desired - velocity;
    }

    public Vector3 Wander()
    {
        Vector3 circleCenter = velocity;
        circleCenter = circleCenter.normalized * wanderingDistance;

        Vector3 displacement = new Vector3(0, -1);
        displacement = setAngle(displacement, wanderAngle);
        wanderAngle += Random.value * (angleChange - angleChange * 0.5f);

        return circleCenter + displacement;

    }

    Vector3 setAngle(Vector3 vec, float value)
    {
        float len = vec.magnitude;
        vec.x = len * Mathf.Cos(value);
        vec.y = len * Mathf.Sin(value);
        return vec;
    }

    public Vector3 Pursuit(GameObject ePursuit)
    {
        if (ePursuit == null)
            return Vector3.zero;
        float distance = Vector3.Distance(ePursuit.transform.position, transform.position);
        float updatesNeeded = distance / maxVelocity;
        return Arrival(ePursuit.transform.position + velocity.normalized * updatesNeeded);
    }

    public Vector3 Evade(GameObject eEvade)
    {
        if (eEvade == null)
            return Vector3.zero;
        float distance = Vector3.Distance(ePursuit.transform.position, transform.position);
        float updatesNeeded = distance / maxVelocity;
        return Flee(eEvade.transform.position + velocity.normalized * updatesNeeded);
    }

    public Vector3 GroupBehaviour()
    {
        List<SteeringBehavior> heroesST = fl.getAllHeroesMovement();
        if (heroesST.Count == 0)
            return Vector3.zero;
        Vector3 Align = Vector3.zero;
        Vector3 Cohese = Vector3.zero;
        Vector3 Separate = Vector3.zero;
        Vector3 sumHeroesVelocity = Vector3.zero;
        Vector3 massCenter = Vector3.zero;
        foreach(SteeringBehavior heroST in heroesST)
        {
            sumHeroesVelocity += heroST.velocity;
            massCenter += heroST.transform.position;

        }
        Align = sumHeroesVelocity / heroesST.Count;
        Cohese = massCenter / heroesST.Count - transform.position;
        Separate = -Cohese;
        return wAlign * Align + wCohese * Cohese + wSeparate * Separate;
    }

    public Vector2 GetMovementDirection()
    {
        Vector2 movement = new Vector2(aceleration.x, aceleration.y).normalized * 100;
        Vector2 movementDirection;
        movementDirection.x = movement.x / movement.magnitude;
        movementDirection.y = movement.y / movement.magnitude;
        return movementDirection;
    }

}

