using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamState : State
{
    public RoamState(StateAgent owner, string name) : base(owner, name) { }

    public override void OnEnter()
    {
        Quaternion rotation = Quaternion.AngleAxis(Random.Range(-90.0f, 90.0f), Vector3.up);
        Vector3 forward = rotation * owner.transform.forward;
        Vector3 destination = owner.transform.position + forward * Random.Range(10.0f, 15.0f);

        owner.movement.MoveTowards(destination);
        owner.movement.Resume();
        owner.atDestination.value = false;
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        if (Vector3.Distance(owner.transform.position, owner.movement.destination) <= 1.5f)
        {
            owner.atDestination.value = true;
        }
    }
}
