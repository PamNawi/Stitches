using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DefenseMission : BasicMission
{

    //[HideInInspector]
    //public GameObject target;
    //public int decayValue = 1;
    //public float decayInterval = 10.0f;
    //public List<Vector2> patrolPoints;
    //public float distancePatrol;
    //public float patrolAngle;

    //void Start()
    //{
    //    Vector3 pPoint3d = Vector3.zero;
    //    value += decayValue;
    //    StartCoroutine(LowerValue());
    //    DefineAllPatrolPoints();

    //    foreach (Vector2 pPoint in patrolPoints)
    //    {
    //        pPoint3d.x = pPoint.x;
    //        pPoint3d.y = pPoint.y;
    //        actionSequence.Add(new Come(pPoint3d));
    //    }
    //}

    //void Update()
    //{
    //    Health targetHealth = target.GetComponent<Health>();
    //    if (value <= 0 || targetHealth == null || !targetHealth.isAlive())
    //    {
    //        value = 0;
    //        endThisMission();
    //    }
    //}
    //private IEnumerator LowerValue()
    //{
    //    while (value > 0)
    //    {
    //        value -= decayValue;
    //        yield return new WaitForSeconds(decayInterval);
    //    }

    //}

    //public override Action getNextAction(BasicAgent agent)
    //{
    //    if (!interactingAgents.ContainsKey(agent))
    //        return null;
    //    interactingAgents[agent] = (interactingAgents[agent] + 1) % actionSequence.Count;
    //    return actionSequence[interactingAgents[agent]];
    //}

    //void DefineAllPatrolPoints()
    //{
    //    Vector2 direction = new Vector2(1, 1);
    //    Vector2 pPoint = Vector2.zero;
    //    int totalPoints = (int)(360 / patrolAngle);
    //    for (float i = 0; i < totalPoints; i++)
    //    {

    //        direction.x = Mathf.Sin((int)(i * patrolAngle)) * distancePatrol;
    //        direction.y = Mathf.Cos((int)(i * patrolAngle)) * distancePatrol;
    //        pPoint = direction;
    //        pPoint.x += transform.parent.position.x;
    //        pPoint.y += transform.parent.position.y;
    //        patrolPoints.Add(pPoint);
    //    }
    //}
}
