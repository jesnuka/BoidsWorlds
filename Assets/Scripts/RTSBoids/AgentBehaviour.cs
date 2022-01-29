using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehaviour : MonoBehaviour
{
    [SerializeField]
    Agent agent;

    public MovementState movementState; //Current state of movement
    public ActionState actionState; //Current state of action

    public bool targetAttackable;

    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<Agent>();
    }

    public void SetTarget(GameObject newTarget)
    {
        agent.target = newTarget;
        if (CheckTargetIsEnemy())
            targetAttackable = true;
        else
            targetAttackable = false;
    }
    bool CheckTargetIsEnemy()
    {
        if (agent.target.gameObject.GetComponent<Agent>() != null)
            return true;
        else
            return false;
    }
    public enum MovementState
    {
        Idle, //Stay still
        Move, //Move to target
        Flee,
        Seek //Move towards enemy
    }
   
    public enum ActionState
    {
        Idle, //Don't perform actions
        Attack, //Attack target
        Gather //Gather target material / resource
    }

    //Display Unit State as a label above it for debug purposes
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.Label(transform.position + Vector3.up * 1, movementState.ToString() + " - " + actionState.ToString());
    }

    public float MapToRange(float rotation)
    {
        rotation %= 360f;
        if (Mathf.Abs(rotation) > 180f)
        {
            if (rotation > 0f)
                rotation += 360f;
            else
                rotation -= 360f;
        }

        return rotation;
    }

    public void ChangeMovementState(MovementState newMovementState)
    {
        movementState = newMovementState;
    }

    public void ChangeActionState(ActionState newActionState)
    {
        actionState = newActionState;
    }

    private void Update()
    {
        PerformAction();
        agent.SetSteering(GetSteering());

    }

    public void PerformAction()
    {
        switch (actionState) //Placeholder methods
        {
            case ActionState.Idle:
                {
                    IdleAction();
                    break;
                }
            case ActionState.Attack:
                {
                    AttackAction();
                    break;
                }
        }
    }

    private void IdleAction()
    {

    }

    private void AttackAction()
    {

    }

    public virtual Steering GetSteering()
    {
        Steering steering = new Steering();
        float distanceToTarget = (agent.target.transform.position - transform.position).magnitude;
        float approachDistance = (targetAttackable) ? agent.closestApproachDistance : 0f;
        switch (movementState)
        {
            case MovementState.Idle:
                {
                    steering.linear = Vector3.zero;
                    if (distanceToTarget > approachDistance)
                    {
                        ChangeMovementState(MovementState.Move);
                    }
                    break;
                }
            case MovementState.Move:
                {
                    if (distanceToTarget > approachDistance)
                    {
                        steering.linear = agent.target.transform.position - transform.position;
                        steering.linear.Normalize();
                        steering.linear = steering.linear * agent.maxAcceleration;
                    }
                    else
                    {
                        ChangeMovementState(MovementState.Idle);
                    }
                    break;
                }
            case MovementState.Flee:
                {
                    steering.linear = transform.position - agent.target.transform.position;
                    steering.linear.Normalize();
                    steering.linear = steering.linear * agent.maxAcceleration;
                    break;
                }
        }

        return steering;
        
    }

}
