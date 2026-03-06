using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    // ---- SCRIPTS ---- //
    EnemyMovement enemyMovement;
    EnemySensor enemySensor;
    EnemyStateMachine enemyStateMachine;

    public float enemyAttackCoolDown;
    float decisionTimer;
    public float decisionInterval = 2f;

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
            if (enemyMovement.target.CompareTag("Player") & enemyStateMachine.state != EnemyStateMachine.State.Attack)
            {
                enemyStateMachine.ChangeState(EnemyStateMachine.State.Chase);
            }
        }

        decisionTimer -= Time.deltaTime;

        if (decisionTimer <= 0f)
        {
            decisionTimer = decisionInterval;
            TryDecision();
        }
    }

    int Roll(int range) // You gotta set a range, every act counts on different intervels
    {
        return Random.Range(0, range);
    }

    void TryDecision()
    {
        if (enemySensor.distance <= 10)
        {
            int roll = Roll(5);

            if (roll < 2)
            {
                enemyMovement.movementLock();
            }
        }
    }

}
