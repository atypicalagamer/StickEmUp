using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_Win : MonoBehaviour{

    [SerializeField] 
    private GameObject WinScreen;

    private void OnTriggerEnter(Collider other)
    {
    Debug.Log("Chest_Win: Trigger with " + other.name);
        if (other.CompareTag("Player"))
        {
                        Debug.Log("Chest_Win: Player hit chest!");

            Debug.Log("Before: WinScreen active = " + WinScreen.activeSelf);
            WinScreen.SetActive(true);
            Debug.Log("After: WinScreen active = " + WinScreen.activeSelf);
            Time.timeScale = 0f;       
        }
    }
}
