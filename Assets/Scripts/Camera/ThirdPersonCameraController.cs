using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObject;
    public Transform combatLookAt;
    public Rigidbody rb;

    public GameObject basicCamera;
    public GameObject combatCamera;

    [Header("Camera settings")]
    public float rotationSpeed;


    public CameraStyle currentStyle;
    public enum CameraStyle
    {
        Basic,
        Combat
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (currentStyle == CameraStyle.Basic)
        {
            // Calculate the current view direction and set it to the orientation
            Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z); ;
            orientation.forward = viewDirection.normalized;

            // Obtain the input and calculate the input direction
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 inputDirection = verticalInput * orientation.forward + horizontalInput * orientation.right;

            // Rotate player based on inputs
            if (inputDirection != Vector3.zero)
                playerObject.forward = Vector3.Slerp(playerObject.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        } else if (currentStyle == CameraStyle.Combat)
        {
            Vector3 directionToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = directionToCombatLookAt.normalized;

            playerObject.forward = orientation.forward;
        }        
    }

    public void ChangeCameraStyle(CameraStyle newStyle)
    {
        basicCamera.SetActive(false);
        combatCamera.SetActive(false);

        if (newStyle == CameraStyle.Basic)
        {
            basicCamera.SetActive(true);
        } else if (newStyle == CameraStyle.Combat)
        {
            combatCamera.SetActive(true);
        }

        currentStyle = newStyle;
    }
}
