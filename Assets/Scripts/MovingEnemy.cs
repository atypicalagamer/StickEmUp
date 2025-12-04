using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingEnemy : MonoBehaviour {
    [Range(0, 80)] [SerializeField] float attackRange = 2f, sightRange = 8f, timeBetweenAttacks = 1.5f;

    [Range(0, 50)] [SerializeField] int power; // The amount of damage the enemy does

    private NavMeshAgent thisEnemy;
    public Transform playerPos;

    private bool isAttacking; // If the enemy is attacking
    // private bool noticePlayer; // If the enemy notices/hears/sees the player

    private void Start() {
        thisEnemy = GetComponent<NavMeshAgent>();
        playerPos = FindObjectOfType<PlayerHealth>().transform;
        // noticePlayer = false;
    }

    private void Update() {
        float distanceFromPlayer = Vector3.Distance(playerPos.position, this.transform.position); // The distance between player and enemy

        if (distanceFromPlayer <= sightRange && distanceFromPlayer > attackRange && !PlayerHealth.isDead) { // If the player is in sight, not in attack range, and isn't dead
            // NoticePlayer(); // Look towards player
            isAttacking = false; // Don't attack
            thisEnemy.isStopped = false; // Keep moving
            StopAllCoroutines(); // Stop attack function

            ChasePlayer();
        }

        if (distanceFromPlayer <= attackRange && !isAttacking && !PlayerHealth.isDead) {
            thisEnemy.isStopped = true; // Stop the enemy from moving
            StartCoroutine(AttackPlayer()); // Start attacking
        }

        if (PlayerHealth.isDead) {
            thisEnemy.isStopped = true;
        }
    }

    /*
    private IEnumerator NoticePlayer() {
        noticePlayer = true;
        thisEnemy.SetDestination(playerPos.position);
        yield return new WaitForSeconds(1f);
        noticePlayer = false;
    }
    */

    private void ChasePlayer() {
        thisEnemy.SetDestination(playerPos.position); // Set the enemy's destination to the player
    }

    private IEnumerator AttackPlayer() {
        isAttacking = true;
        yield return new WaitForSeconds(timeBetweenAttacks); // Wait for the time between attacks
        FindObjectOfType<PlayerHealth>().TakeDamage(power); // Damage the player with power damage
        isAttacking = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }
}
// Credit to Developer Jake on YT for this and PlayerHealth scripts!
