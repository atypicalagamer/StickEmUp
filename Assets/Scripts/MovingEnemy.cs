using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    public enum EnemyState {
    Idle,
    Chase,
    Attack
    }

    public EnemyState currentState = EnemyState.Idle;

    [SerializeField]
    public float moveSpeed = 8f;
    public float attackRange = 2f;
    private Transform playerTarget;
    private float attackRate = 1.2f;
    private float attackDelay = 0f;

    // Trigger functions for detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentState = EnemyState.Chase;
            playerTarget = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentState = EnemyState.Idle;
            playerTarget = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
            case EnemyState.Attack:
                AttackPlayer();
                break;
        }
    }

    void ChasePlayer()
    {
        if (playerTarget == null) return;

        float distance = Vector3.Distance(transform.position, playerTarget.position);

        if (distance <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else
        {
            Vector3 direction = (playerTarget.position - transform.position).normalized;
            direction.y = 0;

            transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, moveSpeed * Time.deltaTime);

            Quaternion lookingRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookingRotation, Time.deltaTime * 10f);
        }
    }

    void AttackPlayer()
    {
        if (playerTarget == null) return;

        float distance = Vector3.Distance(transform.position, playerTarget.position);

        if (distance > attackRange + 0.2f)
        {
            currentState = EnemyState.Chase;
            return;
        }

        Vector3 direction = (playerTarget.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookingRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookingRotation, Time.deltaTime * 10f);

        if (Time.time >= attackDelay)
        {
            Attack();
            attackDelay = Time.time + 2f / attackRate;
        }
    }
    
    void Attack()
    {
        Debug.Log("Enemy is attacking!!");
    }
}
