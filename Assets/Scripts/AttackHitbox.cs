using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField]
    public float damage = 5f;
    
    void Start()
    {
        Destroy(gameObject, 0.2f); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Hit Player for " + damage + " damage!");
            }
            else
            {
                Debug.LogWarning("Hit Player, but no PlayerHealth script found on target!");
            }
            
            Destroy(gameObject);
        }
    }
}
