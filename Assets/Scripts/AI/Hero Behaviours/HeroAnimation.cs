using UnityEngine;
using System.Collections;

public class HeroAnimation : MonoBehaviour {

    [HideInInspector]    public Animator animController;
    [HideInInspector]    public enum MoveState { Idle, Walking, Attacking, Dying, LookingMap };
    [HideInInspector]    public MoveState moveState;

    [HideInInspector]    public FollowPath followPath;
    [HideInInspector]    public Hero stateMachine;
    [HideInInspector]    public Health health;

    private MoveState lastMoveState;
    private Animation lastAnimation;
    // Use this for initialization
    void Start () {
        animController = GetComponent<Animator>();
        followPath = GetComponent<FollowPath>();
        stateMachine = GetComponent<Hero>();
        health = GetComponent<Health>();
        lastMoveState = 0;
    }
	
	// Update is called once per frame
	void Update () {
        MoveState newMoveState = moveState;
        Vector2 movement = followPath.GetMovementDirection();
        bool isWalking = (Mathf.Abs(movement.x) + Mathf.Abs(movement.y)) > 0;
        if (!health.isAlive())
        {
            animController.SetInteger("MoveState", (int) MoveState.Dying);
            return;
        }
        else if (stateMachine.target != null && Vector3.Distance(transform.position, stateMachine.target.transform.position) <= stateMachine.weaponBehaviour.minAttackDistance)
        {
            newMoveState = MoveState.Attacking;
            followPath.Stay();
        }
        else if (isWalking)
        {
            newMoveState = MoveState.Walking;
            animController.SetFloat("SenMovement", movement.y);
            animController.SetFloat("CosMovement", movement.x);
        }
        else {
            newMoveState = MoveState.Idle;
            followPath.Stay();
        }
        changeMoveState(newMoveState);
    }

    private void changeMoveState(MoveState newMoveState)
    {
        if (lastMoveState != newMoveState)
        {
            lastMoveState = moveState;
        }
        
        animController.SetInteger("MoveState", (int)newMoveState);
    }
}
