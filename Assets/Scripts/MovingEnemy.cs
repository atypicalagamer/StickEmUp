using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    public enum EnemyState { Idle, Chase, Attack }
    private EnemyState currentState = EnemyState.Idle; // Default state

    [Header("Movement & Range")]
    public float moveSpeed = 5f;
    public float attackRange = 3f;
    public float rotationSpeed = 4f;

    [Header("Attack Settings")]
    [SerializeField] private GameObject attackHitboxPrefab;
    [SerializeField] private float attackCooldown = 2.0f;
    [SerializeField] private Vector3 hitboxOffset = new Vector3(0f, 0f, 1.5f);
    private bool isAttacking = false;


    private Rigidbody EnemyRB;
    private Transform playerTarget;
    private float timeSinceLastAttack = 0f;

    void Start()
    {
        EnemyRB = GetComponent<Rigidbody>();
        if (EnemyRB == null)
        {
            Debug.LogError("Enemy doesn't have a Rigidbody component.. somehow.");
        }
        EnemyRB.isKinematic = false;
        EnemyRB.freezeRotation = true;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Chase:
                if (playerTarget != null)
                {
                    float distance = Vector3.Distance(transform.position, playerTarget.position);

                    if (distance <= attackRange && timeSinceLastAttack >= attackCooldown)
                    {
                        currentState = EnemyState.Attack;
                    }
                    else if (distance <= attackRange && timeSinceLastAttack < attackCooldown)
                    {
                        RotateTowardsPlayer();
                    }
                    else // distance > attackRange
                    {
                        RotateTowardsPlayer();
                    }
                }
                break;
            case EnemyState.Attack:
                if (!isAttacking)
                {
                    StartCoroutine(AttackRoutine());
                }
                break;
        }

        if (timeSinceLastAttack < attackCooldown)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
    }
    
    void FixedUpdate()
    {
        if (currentState == EnemyState.Chase && playerTarget != null)
        {
            float distance = Vector3.Distance(transform.position, playerTarget.position);
            
            if (distance > attackRange)
            {
                Vector3 moveDirection = transform.forward * moveSpeed;
                EnemyRB.velocity = new Vector3(moveDirection.x, EnemyRB.velocity.y, moveDirection.z);
            }
            else
            {
                EnemyRB.velocity = Vector3.zero;
            }
        }
        else
        {
            EnemyRB.velocity = Vector3.zero;
        }
    }

    private void RotateTowardsPlayer()
    {
        if (playerTarget != null)
        {
            Vector3 direction = playerTarget.position - transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
    
    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        
        EnemyRB.velocity = Vector3.zero;

        Debug.Log("Enemy is attacking!");
        
        Vector3 spawnPosition = transform.position + transform.rotation * hitboxOffset;
        GameObject hitbox = Instantiate(attackHitboxPrefab, spawnPosition, transform.rotation);
        
        timeSinceLastAttack = 0f;

        yield return new WaitForSeconds(0.1f); 

        currentState = EnemyState.Chase; 
        isAttacking = false;
    }
    
    // Trigger functions for detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered detection zone. Switching to Chase state.", this);
            playerTarget = other.transform;
            currentState = EnemyState.Chase;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left detection zone. Switching to Idle state.", this);
            playerTarget = null;
            currentState = EnemyState.Idle;
        }
    }
}
