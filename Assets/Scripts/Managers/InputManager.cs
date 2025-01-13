using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Control Keybinds")]
    public KeyCode[] jumpKeys;
    public KeyCode[] interactKeys;
    public KeyCode[] sprintKeys;
    public KeyCode[] crouchKeys;

    [Header("UI Keybinds")]
    public KeyCode[] escapeKeys;


    public bool controlActive = true;
    public bool UIActive = true;
    public float verticalInput { get; private set; }
    public float horizontalInput { get; private set; }

    public float verticalInputRaw { get; private set; }
    public float horizontalInputRaw { get; private set; }

    public bool jump { get; private set; }
    public bool interact { get; private set; }
    public bool sprint { get; private set; }
    public bool crouch { get; private set; }
    public bool escape { get; private set; }

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
        if (controlActive)
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

        if (UIActive)
        {
            escape = false;
            foreach (KeyCode key in escapeKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    escape = true;
                    break;
                }
            }
        } else
        {
            escape = false;
        }
    }

    public KeyCode InteractKey()
    {
        if (controlActive && interactKeys.Length > 0)
            return interactKeys[0];
        else
            return KeyCode.None;
    }

    public KeyCode JumpKey()
    {
        if (controlActive && jumpKeys.Length > 0)
            return jumpKeys[0];
        else
            return KeyCode.None;
    }

    public KeyCode SprintKey()
    {
        if (controlActive && sprintKeys.Length > 0)
            return sprintKeys[0];
        else
            return KeyCode.None;
    }

    public KeyCode CrouchKey()
    {
        if (controlActive && crouchKeys.Length > 0)
            return crouchKeys[0];
        else
            return KeyCode.None;
    }

    public KeyCode EscapeKey()
    {
        if (UIActive && escapeKeys.Length > 0)
            return escapeKeys[0];
        else
            return KeyCode.None;
    }
}
