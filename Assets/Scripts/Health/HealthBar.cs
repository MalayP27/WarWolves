using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHP;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start(){
        totalHealthBar.fillAmount = playerHP.currentHP / 10;
    }
    private void Update(){
        currentHealthBar.fillAmount = playerHP.currentHP / 10;
    }
}
