using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    // ---- SCRIPTS ---- //
    EnemyStateMachine enemyStateMachine;
    public Transform target;
    public float speed = 3f;
    public float rotationSpeed = 5f;

    public List<Transform> waypoints;

    private Rigidbody rb;
    public int currentWaypointIndex = 0;

    private void Awake()
    {
        enemyStateMachine = GetComponent<EnemyStateMachine>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void FixedUpdate()
    {
        WaypointCheck();

        EnemyMove();
    }

    public Transform GetToNextWaypoint()
    {
        if (waypoints == null || waypoints.Count == 0) return null;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;

        target = waypoints[currentWaypointIndex];
        return target;
    }

    void WaypointCheck() 
    {
        if (enemyStateMachine.state == EnemyStateMachine.State.Patrol)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < 0.6f)
            {
                target = GetToNextWaypoint();
            }
        }
    }

    void EnemyMove() 
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }

        Vector3 move = direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }
}