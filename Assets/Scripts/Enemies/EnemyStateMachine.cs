using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Stunned
    }

    [Header("Current State")]
    public State state;

    private void Awake()
    {
        state = State.Patrol;
    }
    
    private void Update()
    {
        
    }

    public void ChangeState(State newState)
    {
        if (state == newState) return;
        state = newState;
    }
}