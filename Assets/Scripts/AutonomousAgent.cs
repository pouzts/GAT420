using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutonomousAgent : Agent
{
    [SerializeField] Perception perception;
    [SerializeField] Perception flockPerception;
    [SerializeField] ObstaclePerception obstaclePerception;
    [SerializeField] Steering steering;
    [SerializeField] AutonomousAgentData agentData;

    public float maxSpeed { get { return agentData.maxSpeed; } }
    public float maxForce { get { return agentData.maxForce; } }

    public Vector3 velocity { get; set; } = Vector3.zero;

    void Update()
    {
        Vector3 acceleration = Vector3.zero;

        GameObject[] gameObjects = perception.GetGameObjects();
        if (gameObjects.Length == 0)
        {
            acceleration += steering.Wander(this);
        }
        
        // Seek / Flee
        if (gameObjects.Length != 0)
        {
            acceleration += steering.Seek(this, gameObjects[0]) * agentData.seekWeight;
            acceleration += steering.Flee(this, gameObjects[0]) * agentData.fleeWeight;
        }

        // Flocking
        gameObjects = flockPerception.GetGameObjects();
        if (gameObjects.Length != 0)
        {
            acceleration += steering.Cohesion(this, gameObjects) * agentData.cohesionWeight;
            acceleration += steering.Seperation(this, gameObjects, agentData.separationRadius) * agentData.separationWeight;
            acceleration += steering.Alignment(this, gameObjects) * agentData.alignmentWeight;
        }

        // obstacle avoidance
        if (obstaclePerception.IsObstacleInFront())
        {
            Vector3 direction = obstaclePerception.GetOpenDirection();
            acceleration += steering.CalculateSteering(this, direction) * agentData.obstacleWeight;
        }

        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;

        if (velocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }

        transform.position = Utilities.Wrap(transform.position, new Vector3(-20, -20, -20), new Vector3(20, 20, 20), new Vector3(1, 1, 1));
    }
}
