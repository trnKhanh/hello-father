using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Camera Camera;
    private NavMeshAgent Agent;

    [SerializeField]
    private float walkSpeed = 3.5f; 
    [SerializeField]
    private float runSpeed = 10f; 
    private float currentSpeed;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        float moveForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1f : 0f;
        float moveBackward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? -1f : 0f;
        float moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? 1f : 0f;
        float moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? -1f : 0f;

        Vector3 direction = new Vector3(0f, 0f, moveForward + moveBackward);

        if (direction.magnitude > 0)
        {
            Agent.Move(transform.TransformDirection(direction) * currentSpeed * Time.deltaTime);
        }

        Agent.speed = currentSpeed;
    }
}