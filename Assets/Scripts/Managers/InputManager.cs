using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Keybinds")]
    public KeyCode[] jumpKeys;
    public KeyCode[] interactKeys;
    public KeyCode[] sprintKeys;
    public KeyCode[] crouchKeys;


    public bool active = true;
    public float verticalInput { get; private set; }
    public float horizontalInput { get; private set; }

    public float verticalInputRaw { get; private set; }
    public float horizontalInputRaw { get; private set; }

    public bool jump { get; private set; }
    public bool interact { get; private set; }
    public bool sprint { get; private set; }
    public bool crouch { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (active)
        {
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");

            verticalInputRaw = Input.GetAxisRaw("Vertical");
            horizontalInputRaw = Input.GetAxisRaw("Horizontal");

            jump = false;
            foreach (KeyCode key in jumpKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    jump = true;
                    break;
                }
            }

            interact = false;
            foreach (KeyCode key in interactKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    interact = true;
                    break;
                }
            }

            sprint = false;
            foreach (KeyCode key in sprintKeys)
            {
                if (Input.GetKey(key))
                {
                    sprint = true;
                    break;
                }
            }

            crouch = false;
            foreach (KeyCode key in crouchKeys)
            {
                if (Input.GetKey(key))
                {
                    crouch = true;
                    break;
                }
            }
        }
        else
        {
            verticalInput = 0;
            horizontalInput = 0;

            verticalInputRaw = 0;
            horizontalInputRaw = 0;

            jump = false;
            interact = false;
            sprint = false;
            crouch = false;
        }
    }

    public KeyCode InteractKey()
    {
        if (active && interactKeys.Length > 0)
            return interactKeys[0];
        else
            return KeyCode.None;
    }

    public KeyCode JumpKey()
    {
        if (active && jumpKeys.Length > 0)
            return jumpKeys[0];
        else
            return KeyCode.None;
    }

    public KeyCode SprintKey()
    {
        if (active && sprintKeys.Length > 0)
            return sprintKeys[0];
        else
            return KeyCode.None;
    }

    public KeyCode CrouchKey()
    {
        if (active && crouchKeys.Length > 0)
            return crouchKeys[0];
        else
            return KeyCode.None;
    }
}
