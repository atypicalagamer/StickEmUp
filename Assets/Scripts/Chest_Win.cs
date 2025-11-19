using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_Win : MonoBehaviour{

    [SerializeField] 
    private GameObject WinScreen;

    private void OnTriggerEnter(Collider other)
    {
    
        if (other.CompareTag("Player"))
        {
            WinScreen.SetActive(true); 
            Time.timeScale = 0f;       
        }
    }
}
