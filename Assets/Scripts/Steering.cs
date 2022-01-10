using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    [SerializeField] float wanderDistance = 1.0f;
    [SerializeField] float wanderRadius = 3.0f;
    [SerializeField] float wanderDisplacement = 5.0f;

    float wanderAngle = 0;


    public Vector3 Seek(AutonomousAgent agent, GameObject target) {
        Vector3 force = CalculateSteering(agent, target.transform.position - agent.transform.position);
        
        return force;
    }

    public Vector3 Flee(AutonomousAgent agent, GameObject target)
    {
        Vector3 force = CalculateSteering(agent, agent.transform.position - target.transform.position);

        return force;
    }

    public Vector3 Wander(AutonomousAgent agent, GameObject target)
    {
        return Vector3.zero;
    }

    Vector3 CalculateSteering(AutonomousAgent agent, Vector3 vector)
    {
        Vector3 direction = vector.normalized;
        Vector3 desired = direction * agent.maxSpeed;
        Vector3 steer = desired - agent.velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, agent.maxForce);
        
        return force;
    }
}
