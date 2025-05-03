using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float camHeight;
    [SerializeField] private float speed;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float camSpeed;
    [SerializeField] private float minX; // Minimum X boundary for the camera
    [SerializeField] private float maxX; // Maximum X boundary for the camera
    private float lookAhead;
    private float currentPosx;
    private Vector3 velocity = Vector3.zero;



    private void Update()
    {   
        // Calculate the target position for the camera
        float targetX = player.position.x + lookAhead;
        targetX = Mathf.Clamp(targetX, minX, maxX); // Ensure the targetX stays within boundaries
        
        //Set the new position for the camera with a fixed y and z position
        transform.position = new Vector3(targetX, player.position.y + camHeight, transform.position.z);

        // Adjust the look-ahead distance smoothly
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * camSpeed);
        // transform.position = new Vector3(player.position.x + lookAhead, player.position.y + 5, transform.position.z);
        // lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * camSpeed);
    }

    
}
