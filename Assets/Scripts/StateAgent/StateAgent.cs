using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAgent : Agent
{
    [SerializeField] Perception perception;

    public PathFollower pathFollower;
    public StateMachine stateMachine = new StateMachine();

    public BoolRef enemySeen;
    public FloatRef enemyDistance;
    public FloatRef health;
    public FloatRef timer;

    public GameObject enemy { get; set; }

    private void Start()
    {
        stateMachine.AddState(new IdleState(this, typeof(IdleState).Name));
        stateMachine.AddState(new PatrolState(this, typeof(PatrolState).Name));
        stateMachine.AddState(new ChaseState(this, typeof(ChaseState).Name));
        stateMachine.AddState(new DeathState(this, typeof(DeathState).Name));
        stateMachine.AddState(new AttackState(this, typeof(AttackState).Name));

        stateMachine.AddTransition(typeof(IdleState).Name, new BoolTransition(enemySeen, true), typeof(ChaseState).Name);
        stateMachine.AddTransition(typeof(IdleState).Name, new FloatTransition(timer, Transition.Predicate.LESS, 0), typeof(PatrolState).Name);
        stateMachine.AddTransition(typeof(IdleState).Name, new FloatTransition(health, Transition.Predicate.LESS_EQUAL, 0), typeof(DeathState).Name);
        stateMachine.AddTransition(typeof(ChaseState).Name, new FloatTransition(enemyDistance, Transition.Predicate.LESS_EQUAL, 2), typeof(AttackState).Name);
        
        stateMachine.AddTransition(typeof(PatrolState).Name, new BoolTransition(enemySeen, true), typeof(ChaseState).Name);
        stateMachine.AddTransition(typeof(PatrolState).Name, new FloatTransition(health, Transition.Predicate.LESS_EQUAL, 0), typeof(DeathState).Name);
        
        stateMachine.AddTransition(typeof(ChaseState).Name, new BoolTransition(enemySeen, true), typeof(IdleState).Name);
        stateMachine.AddTransition(typeof(ChaseState).Name, new FloatTransition(health, Transition.Predicate.LESS_EQUAL, 0), typeof(DeathState).Name);
        stateMachine.AddTransition(typeof(ChaseState).Name, new FloatTransition(enemyDistance, Transition.Predicate.LESS_EQUAL, 2), typeof(AttackState).Name);
        
        stateMachine.AddTransition(typeof(AttackState).Name, new FloatTransition(timer, Transition.Predicate.LESS_EQUAL, 0), typeof(ChaseState).Name);

        stateMachine.SetState(stateMachine.StateFromName(typeof(IdleState).Name));
    }

    private void Update()
    {
        // update parameters
        var enemies = perception.GetGameObjects();
        enemySeen.value = (enemies.Length != 0);
        enemy = (enemies.Length != 0) ? enemies[0] : null;
        enemyDistance.value = (enemy != null) ? (Vector3.Distance(transform.position, enemy.transform.position)) : float.MaxValue;
        
        timer.value -= Time.deltaTime;

        stateMachine.Update();

        animator.SetFloat("speed", movement.velocity.magnitude);

        Debug.Log(typeof(IdleState).Name);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), stateMachine.GetStateName());
    }
}
