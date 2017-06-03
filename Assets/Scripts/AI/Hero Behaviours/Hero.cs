using UnityEngine;
using System.Collections;


public class Hero : BasicHero {
    [HideInInspector] public GameObject target;
    //Weapon Variables
    [HideInInspector] public GameObject weapon;
    [HideInInspector] public WeaponBehaviour weaponBehaviour;

    private void Awake()
    {
        followPath = GetComponent<FollowPath>();
        viewSight = GetComponentInChildren<ViewSight>();
        health = GetComponent<Health>();
        weaponBehaviour = GetComponentInChildren<WeaponBehaviour>();
        weapon = weaponBehaviour.gameObject;

        emotionalState = GetComponent<EmotionalState>();
        agentName = new NameGenerator().createNewName();

        agentTag = heroClass.ToString();

        if (log)
            createLogFile();
    }

    void Start() {
    }

    public void Attack()
    {

        if (target != null)
        {
            Debug.Log("ATTACK!!!");
            if (Vector3.Distance(transform.position, target.transform.position) >= weaponBehaviour.minAttackDistance)
            {
                followPath.setDestiny(target.transform.position);
                followPath.stopDistance = weaponBehaviour.minAttackDistance;
            }
            else {
                weaponBehaviour.Attack(target);
                followPath.Stop();
            }
        }
        else
        {
            followPath.stopDistance = 1.0f;
        }
    }
}