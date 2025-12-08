using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHealth : MonoBehaviour
{
    static private Text uiText;

    private void Awake()
    {
        uiText = GetComponent<Text>();
    }

    public static void UpdateHealthUI(int currentHealth)
    {
        if (uiText != null)
        {
            uiText.text = "Health: " + currentHealth;
        }
    }
}
