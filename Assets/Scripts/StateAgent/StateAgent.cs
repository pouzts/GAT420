using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAgent : Agent
{
    public StateMachine stateMachine = new StateMachine();
    public AgentPath path;

    private void Start()
    {
        stateMachine.AddState(new IdleState(this, "idle"));
        stateMachine.AddState(new PatrolState(this, "patrol"));
        stateMachine.SetState(stateMachine.StateFromName("idle"));
    }

    private void Update()
    {
        stateMachine.Update();

        if (movement.velocity.magnitude > 0.5f)
        {
            animator.SetBool("walk", true);
        }
        else 
        {
            animator.SetBool("walk", false);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), stateMachine.GetStateName());
    }
}
