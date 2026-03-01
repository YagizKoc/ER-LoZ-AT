using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class EnemySensor : MonoBehaviour
{
    public Transform player;
    float distance;                         //Distance to Player
    Vector3 direction;                      //Direction to Player
    public float detectionRange = 10f;
    public float chaseStopDistance = 15f;
    public LayerMask obstacleMask;
    public bool lockedOn;
    private Transform waypointCache;

    // ---- SCRIPTS ---- //
    EnemyMovement enemyMovement;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }
    private void Update()
    {
        // ---- UPDATING VARIABLES ----

        distance = Vector3.Distance(transform.position, player.position); //Distance to Player is updated
        direction = (player.position - transform.position).normalized; // //Direction to Player is updated

        if (lockedOn && distance >= chaseStopDistance)
        {
          LockOff();
        }

        if (distance <= detectionRange && !lockedOn)
        { 
          CastRay(); 
        }
        
        
    }

    void CastRay()
    {
        if (Physics.Raycast(transform.position + Vector3.up, direction, out RaycastHit hit, detectionRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                LockOn();
            }
            else
            {
                Debug.Log("Önümde engel var: " + hit.transform.name);
            }
        }
    }

    void LockOn() 
    {
        lockedOn = true;
        waypointCache = enemyMovement.target;
        enemyMovement.target = player;
    }

    void LockOff() 
    {
        lockedOn = false;
        enemyMovement.target = waypointCache;
    }
}
