using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    float movementSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.C;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundMask;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    RaycastHit slopeHit;
    bool exitingSlope;

    [Header("References")]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    Animator animator;
    string k_moving = "Moving";
    string k_sprinting = "Sprinting";
    string k_air = "Air";

    public MovementState currentState;
    public enum MovementState
    {
        Walking,
        Sprinting,
        Air,
        Crouching
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();

        startYScale = transform.localScale.y;
    }

    void Update()
    {
        UpdateGroundCheck();
        UpdateInput();
        SpeedControl();
        MovementStateHanlder();
        UpdateAnimation();
    }

    void FixedUpdate()
    {

        MovePlayer();    
    }

    void UpdateGroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, groundMask);

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    void UpdateInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    void MovementStateHanlder()
    {
        if (Input.GetKey(crouchKey))
        {
            currentState = MovementState.Crouching;
            movementSpeed = crouchSpeed;
        }
        else if (grounded && Input.GetKey(sprintKey))
        {
            currentState = MovementState.Sprinting;
            movementSpeed = sprintSpeed;
        } else if (grounded)
        {
            currentState = MovementState.Walking;
            movementSpeed = walkSpeed;
        } else
        {
            currentState = MovementState.Air;
        }
    }

    void UpdateAnimation()
    {
        if (grounded)
        {
            animator.SetBool(k_air, false);
            bool wantToMove = (horizontalInput != 0 || verticalInput != 0);

            if (wantToMove)
            {
                animator.SetBool(k_moving, true);
                if (Input.GetKey(sprintKey))
                    animator.SetBool(k_sprinting, true);
                else
                    animator.SetBool(k_sprinting, false);
            }
            else
            {
                animator.SetBool(k_moving, false);
                animator.SetBool(k_sprinting, false);
            }
        } else
        {
            animator.SetBool(k_sprinting, false);
            animator.SetBool(k_moving, false);
            animator.SetBool(k_air, true);
        }
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if  (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * movementSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (grounded)
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);

        rb.useGravity = !OnSlope();
    }

    void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > movementSpeed)
            {
                rb.velocity = rb.velocity.normalized * movementSpeed;
            }
        }
        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVelocity.magnitude > movementSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }

    void Jump()
    {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.1f))
        {
            Debug.Log("On slope");
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
