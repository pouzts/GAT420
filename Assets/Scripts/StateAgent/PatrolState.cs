using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public PatrolState(StateAgent owner, string name) : base(owner, name)
    { 
    
    }

    public override void OnEnter()
    {
        owner.pathFollower.targetNode = owner.pathFollower.pathNodes.GetNearestNode(owner.transform.position);
        owner.movement.Resume();
        Debug.Log(name + " enter");
    }

    public override void OnExit()
    {
        owner.movement.Stop();
        Debug.Log(name + " exit");
    }

    public override void OnUpdate()
    {
        Debug.Log(name + " update");

        owner.pathFollower.Move(owner.movement);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            owner.stateMachine.SetState(owner.stateMachine.StateFromName(typeof(IdleState).Name));
        }
    }
}
