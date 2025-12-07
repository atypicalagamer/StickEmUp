using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    // --- Public Fields for Unity Inspector ---
    [Header("Health Stats")]
    [SerializeField]
    private float maxHealth = 100f; // Maximum health the player can have
    private float currentHealth; // The player's current health

    [Header("Regeneration")]
    [SerializeField]
    private float healthRegenRate = 0.5f; // Health gained per second
    [SerializeField]
    private float regenDelayAfterHit = 3.0f; // Delay before regeneration starts
    private float lastHitTime;

    public UnityEvent OnPlayerDeath;
    public UnityEvent<float> OnHealthChanged; // Passes the new current health

    void Start()
    {
        // Initialize current health to max health at the start of the game
        currentHealth = maxHealth;
        lastHitTime = Time.time;
        OnHealthChanged.Invoke(currentHealth); // Fire event to update any initial UI
    }

    void Update()
    {
        // Handle regeneration if the player hasn't been hit recently and is not at max health
        if (currentHealth < maxHealth && Time.time > lastHitTime + regenDelayAfterHit)
        {
            RegenerateHealth();
        }
    }

    // --- Core Logic: Receiving Damage ---
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) 
            return; // Already dead, ignore damage

        currentHealth -= damage;
        lastHitTime = Time.time; // Reset the regen timer

        // Clamp the health value so it doesn't go below zero
        currentHealth = Mathf.Max(0, currentHealth);

        Debug.Log($"Player took {damage} damage! Current Health: {currentHealth}");
        OnHealthChanged.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // --- Regeneration Logic ---
    private void RegenerateHealth()
    {
        // Add health based on rate and frame time
        currentHealth += healthRegenRate * Time.deltaTime;

        // Clamp the health value so it doesn't exceed max health
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        
        // Only update the UI event if health actually changed this frame
        OnHealthChanged.Invoke(currentHealth); 
    }

    private void Die()
    {
        Debug.Log("Player has died! Game Over.");

        OnPlayerDeath.Invoke();

        // 2. Disable/Stop Player Functionality
        // Disable movement (uncomment the line below and the reference in Start if you have a movement script)
        // if (playerMovement != null) playerMovement.enabled = false;

        // Disable input processing or the collider
        GetComponent<Collider>().enabled = false; 

        // 3. Implement Game Over State (e.g., load game over scene or display menu)
    }

    // Optional: Public getter for health (read-only access)
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
