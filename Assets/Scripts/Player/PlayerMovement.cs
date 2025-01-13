using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Gravity")]
    public float gravity = -9.8f;
    public bool useGravity = true;
    Vector3 gravityVelocity;

    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    float movementSpeed;

    [Header("Jumping")]
    public float jumpHeight;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Ground Checking")]
    bool isGrounded;

    [Header("References")]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    CharacterController characterController;

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
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // >>> TODO: REMOVE THIS
        if (Input.GetKeyDown(KeyCode.F))
        {
            InputManager.Instance.active = false;
            List<DialogManager.Dialog> dialogs = new List<DialogManager.Dialog>
            {
                new DialogManager.Dialog
                {
                    character = DialogManager.Character.MainCharacter,
                    text = "Hi there.",
                    sound = null,
                },
                new DialogManager.Dialog
                {
                    character = DialogManager.Character.MainCharacter,
                    text = "Hi there. This is me. This is me. Hehehehehehehehehehehehehehehehehehehehehehehehe",
                    sound = null,
                }
            };
            DialogManager.Instance.PlayDialogs(
                dialogs,
                (int dialogId) =>
                {
                    if (dialogId == dialogs.Count - 1)
                    {
                        InputManager.Instance.active = true;
                    }
                }
            );
        }
        // <<< TODO: REMOVE THIS

        GroundChecking();
        UpdateMovementState();
        MovePlayer();
        UpdateGravity();
        UpdateAnimation();
    }

    void GroundChecking()
    {
        isGrounded = characterController.isGrounded;
    }

    void UpdateMovementState()
    {
        verticalInput = InputManager.Instance.verticalInput;
        horizontalInput = InputManager.Instance.horizontalInput;

        if (isGrounded)
        {
            if (InputManager.Instance.sprint)
            {
                currentState = MovementState.Sprinting;
                movementSpeed = sprintSpeed;
            } else
            {
                currentState = MovementState.Walking;
                movementSpeed = walkSpeed;
            }
        } else
        {
            currentState = MovementState.Air;
        }
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection = moveDirection.normalized;
        if (isGrounded)
        {
            characterController.Move(moveDirection * movementSpeed * Time.deltaTime);
        } else
        {
            characterController.Move(moveDirection * movementSpeed * airMultiplier * Time.deltaTime);
        }

        if (InputManager.Instance.jump && isGrounded)
        {
            Jump();
        }
    }

    void UpdateGravity()
    {
        if (isGrounded && gravityVelocity.y < 0)
            gravityVelocity.y = -2.0f;

        if (useGravity)
            gravityVelocity.y += gravity * Time.deltaTime;

        characterController.Move(gravityVelocity * Time.deltaTime);
    }

    void UpdateAnimation()
    {
        if (isGrounded)
        {
            animator.SetBool(k_air, false);
            bool wantToMove = (horizontalInput != 0 || verticalInput != 0);

            if (wantToMove)
            {
                animator.SetBool(k_moving, true);
                if (InputManager.Instance.sprint)
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

    void Jump()
    {
        gravityVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    void ResetJump()
    {
        readyToJump = true;
    }
}
