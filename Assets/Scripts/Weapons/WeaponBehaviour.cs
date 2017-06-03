using UnityEngine;
using System.Collections;

public class WeaponBehaviour : MonoBehaviour
{
    public float minAttackDistance = 0;
    public int damage = 1;

    float distanceTarget = 0;

    [HideInInspector] public GameObject target;
    
    //Weapon Animation
    [HideInInspector] Animator animController;
    [HideInInspector] public enum WeaponState { Idle, Attacking, Reloading };
    [HideInInspector] public WeaponState weaponState;

    public void Start() {
        animController = GetComponent<Animator>();
    }

    public void Update() {
        if (target != null)
        {
            distanceTarget = Vector3.Distance(transform.position, target.transform.position);
            UpdateAnimation();
            AttackTarget();
        }
        else
        {
            animController.SetInteger("WeaponState", (int)WeaponState.Idle);
        }
    }

    public void Attack(GameObject target) {
        this.target = target;
    }

    public void UpdateAnimation() {
        if (target != null && minAttackDistance >= distanceTarget)
            animController.SetInteger("WeaponState", (int) WeaponState.Attacking);
        else
            animController.SetInteger("WeaponState", (int) WeaponState.Idle);
    }

    void AttackTarget()
    {
        if(target != null && minAttackDistance >= distanceTarget)
        {
            target.GetComponent<Health>().takeDamage(damage);
        }
    }

}
