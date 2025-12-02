using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [Range(0, 100)] public int startHealth = 100, currentHealth;

    public static bool isDead;

    private void Start() {
        currentHealth = startHealth; // Sets health to start health at the beginning of the game/scene
    }

    private void Update() {
        if (currentHealth <= 0 && !isDead) {
            isDead = true;
            Debug.Log("The player is dead!");
        }
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;
    }
}
