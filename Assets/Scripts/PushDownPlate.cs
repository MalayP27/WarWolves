using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushDownPlate : MonoBehaviour
{
    [SerializeField] private GameObject door; // Assign the door in the inspector
    [SerializeField] private float slideDistance = 5f; // How far the door slides down
    [SerializeField] private float slideSpeed = 2f; // Speed of door movement
    [SerializeField] private Animator anim;
    private Vector3 initialDoorPosition;
    private bool isActivated = false;
    private int objectsOnPlate = 0; // Keeps track of objects on the plate

    private void Start()
    {
        if (door != null)
        {
            initialDoorPosition = door.transform.position;
        }
        else
        {
            Debug.LogWarning("Door not assigned in PushDownPlate script!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MoveableBlock"))
        {
            objectsOnPlate++;
            if (!isActivated)
            {
                anim.SetBool("steppedOn", true);
                ActivateDoor();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MoveableBlock"))
        {
            objectsOnPlate--;
            if (objectsOnPlate <= 0)
            {
                anim.SetBool("steppedOn", false);
                objectsOnPlate = 0; // Just to be safe against negative values
                DeactivateDoor();
            }
        }
    }

    private void ActivateDoor()
    {
        if (door != null)
        {
            isActivated = true;
            StopAllCoroutines(); // Stop any existing door movement coroutine
            StartCoroutine(SlideDoorDown());
        }
    }

    private void DeactivateDoor()
    {
        if (door != null)
        {
            isActivated = false;
            StopAllCoroutines(); // Stop any existing door movement coroutine
            StartCoroutine(SlideDoorUp());
        }
    }

    private IEnumerator SlideDoorDown()
    {
        Vector3 targetPosition = initialDoorPosition + Vector3.down * slideDistance;

        while (Vector3.Distance(door.transform.position, targetPosition) > 0.01f)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, targetPosition, slideSpeed * Time.deltaTime);
            yield return null;
        }

        // Snap to the target position to avoid tiny discrepancies
        door.transform.position = targetPosition;
    }

    private IEnumerator SlideDoorUp()
    {
        while (Vector3.Distance(door.transform.position, initialDoorPosition) > 0.01f)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, initialDoorPosition, slideSpeed * Time.deltaTime);
            yield return null;
        }

        // Snap to the initial position to avoid tiny discrepancies
        door.transform.position = initialDoorPosition;
    }
}
