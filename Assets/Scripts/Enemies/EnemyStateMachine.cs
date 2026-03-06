using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{

    //---- SCRIPTS ---- //
    Animator animator;
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
        animator = GetComponentInChildren<Animator>();
        ChangeState(State.Patrol);
    }
    
    private void Update()
    {
        
    }

    public void ChangeState(State newState)
    {
        if (state == newState) return;
        state = newState;

        if (state == State.Idle) 
        {
            animator.SetInteger("State", 0);
        }
        else if (state == State.Patrol)
        {
            animator.SetInteger("State", 1);
        }
        else if (state == State.Chase)
        {
            animator.SetInteger("State", 2);
        }
        

    }
}