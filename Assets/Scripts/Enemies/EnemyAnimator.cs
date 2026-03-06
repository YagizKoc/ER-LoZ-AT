using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    // ---- SCRIPTS ---- //
    Animator animator;
    EnemyStateMachine enemyStateMachine;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyStateMachine = GetComponent<EnemyStateMachine>();
    }

    private void Update()
    {
        /*if (enemyStateMachine.state == EnemyStateMachine.State.Idle)
        {
            animator.SetInteger("State", 0);
        }
        else if (enemyStateMachine.state == EnemyStateMachine.State.Patrol)
        {
            animator.SetInteger("State", 2);
        }
        else if (enemyStateMachine.state == EnemyStateMachine.State.Chase)
        {
            animator.SetInteger("State", 3);
        }*/
    }

    public void PlayAttack()
    {
        Debug.Log("Play Attack çağrıldı");
        animator.SetTrigger("Attack");
    }
}