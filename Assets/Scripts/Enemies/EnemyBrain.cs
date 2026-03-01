using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    // ---- SCRIPTS ---- //
    EnemyMovement enemyMovement;
    EnemySensor enemySensor;
    EnemyStateMachine enemyStateMachine;

    private void Awake()
    {
        enemyStateMachine = GetComponent<EnemyStateMachine>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemySensor = GetComponent<EnemySensor>();
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        if (enemyMovement.target != null)
        {
            if (enemyMovement.target.CompareTag("Player"))
            {
                enemyStateMachine.ChangeState(EnemyStateMachine.State.Chase);
            }
        }
    }
}
