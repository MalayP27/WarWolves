// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UIElements;


// #if UNITY_EDITOR
// using UnityEditor;
// #endif

// public class AbilityUI : MonoBehaviour
// {
//     [SerializeField] private PlayerAttacks playerAttacks;
//     [SerializeField] private Image specialAttackCooldownImage;
//     [SerializeField] private Image basicAttackCooldownImage;

//     private void Update()
//     {
//         UpdateCooldownUI();
//     }

//     private void UpdateCooldownUI()
//     {
//         // Update the fill amount for the special attack cooldown UI element
//         if (playerAttacks.SpecialAttackCooldown > 0)
//         {
//             specialAttackCooldownImage.fillAmount = playerAttacks.SpecialAttackCooldown / playerAttacks.SpecialAttackCD;
//         }
//         else
//         {
//             specialAttackCooldownImage.fillAmount = 0;
//         }

//         // Update the fill amount for the basic attack cooldown UI element
//         if (playerAttacks.BasicAttackCooldown > 0)
//         {
//             basicAttackCooldownImage.fillAmount = playerAttacks.BasicAttackCooldown / playerAttacks.BasicAttackCD;
//         }
//         else
//         {
//             basicAttackCooldownImage.fillAmount = 0;
//         }
//     }
// }

// #if UNITY_EDITOR
// [CustomEditor(typeof(AbilityUI))]
// public class AbilityUIEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();
//         if (GUILayout.Button("Assign Default UI Elements"))
//         {
//             AbilityUI abilityUI = (AbilityUI)target;
//             abilityUI.specialAttackCooldownImage = GameObject.Find("SpecialAttackCooldownImage")?.GetComponent<Image>();
//             abilityUI.basicAttackCooldownImage = GameObject.Find("BasicAttackCooldownImage")?.GetComponent<Image>();
//         }
//     }
// }
// #endif
