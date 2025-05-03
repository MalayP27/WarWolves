using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemyUIHealthBar : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private Health healthScript; // Reference to the Health script
    [SerializeField] private Image healthBarFill; // UI Image to show the health bar fill

    private void LateUpdate()
    {
        if (healthScript != null && healthBarFill != null)
        {
            // Update the health bar fill amount based on the current health percentage
            healthBarFill.fillAmount = healthScript.currentHP / healthScript.startingHP;
        }
    }
}
