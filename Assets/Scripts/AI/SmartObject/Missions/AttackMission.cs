using UnityEngine;
using System.Collections.Generic;

public class AttackMission : BasicMission {
    [HideInInspector]  public GameObject target;
    Health targetHealth;
    void Start()
    {
        target = transform.parent.gameObject;
        targetHealth = target.GetComponent<Health>();
        //actionSequence.Add(new Attack(target));
    }

    void Update()
    {
        if (target == null || !targetHealth.isAlive())
        {
            endSmartObject();
        }
    }
}
