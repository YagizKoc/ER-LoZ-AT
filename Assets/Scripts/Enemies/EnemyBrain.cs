using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    // ---- SCRIPTS ---- //
    EnemyMovement enemyMovement;
    EnemySensor enemySensor;
    EnemyStateMachine enemyStateMachine;
    EnemyAttack enemyAttack;

    public float enemyAttackCoolDown;
    float decisionTimer;
    public float decisionInterval = 2f;

    private void Awake()
    {
        enemyStateMachine = GetComponent<EnemyStateMachine>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemySensor = GetComponent<EnemySensor>();
        enemyAttack = GetComponent<EnemyAttack>();
    }
    private void Start()
    {
        
    }

    private void Update()
    {

        decisionTimer -= Time.deltaTime;

        if (decisionTimer <= 0f)
        {
            decisionTimer = decisionInterval;
            TryDecision();
        }
    }

    int Roll(int range) // You gotta set a range, every act counts on different intervels
    {
        return Random.Range(1, range);
    }

    void TryDecision()
    {
        if (enemySensor.distance <= enemySensor.detectionRange)
        {
            int roll = Roll(10);
            Debug.Log("Roll: " + roll + " geldi");
            if (roll < 2)
            {
                enemyMovement.movementLock();
                Debug.Log("Movement lock çalıştı");
            }
            if (roll >= 2 & roll >=3)
            {
                enemyMovement.movementUnlock();
                Debug.Log("Movement unlock çalıştı");
            }
            if (enemySensor.distance <= enemyAttack.meleeAttackRange & roll >= 4)
            {
                enemyAttack.Attack();
                Debug.Log("Attack çalıştı");
            }
        }
    }

}
