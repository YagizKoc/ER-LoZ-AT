using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //---- SCRIPTS ---- //
    EnemyMovement enemyMovement;
    EnemySensor enemySensor;
    EnemyAnimator enemyAnimator;
    EnemyBrain enemyBrain;
    EnemyStateMachine enemyStateMachine;

    public Transform player;
    public float timeBetweenAttacks = 5f; // İki saldırı arası bekleme süresi
    private float nextAttackTime;         // Bir sonraki saldırı ne zaman yapılabilecek?
    
    private float distanceBetweenPlayer;
    public float meleeAttackRange = 2f;

    private void Awake()
    {
        enemyMovement = GetComponentInParent<EnemyMovement>();
        enemySensor = GetComponentInParent<EnemySensor>();
        enemyAnimator = GetComponentInParent<EnemyAnimator>();
        enemyBrain = GetComponentInParent<EnemyBrain>();
        enemyStateMachine = GetComponentInParent<EnemyStateMachine>();
    }

    void Update()
    {
        if (enemyStateMachine.state == EnemyStateMachine.State.Chase) 
        {
            EnemyCombatMode();
        }
    }

    void EnemyCombatMode()
    {
        if (player == null) return;

        distanceBetweenPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceBetweenPlayer <= meleeAttackRange && Time.time >= nextAttackTime)
        {
            Attack();
        }
    }

    void Attack()
    {
        nextAttackTime = Time.time + timeBetweenAttacks;

        enemyAnimator.PlayAttack();
        enemyStateMachine.ChangeState(EnemyStateMachine.State.Attack);

        Debug.Log("Enemy Attacked!");
    }

    public void AttackEnd() 
    {
        Debug.Log("AttackEnd çağrıldı");
        enemyStateMachine.ChangeState(EnemyStateMachine.State.Chase);
    }
}