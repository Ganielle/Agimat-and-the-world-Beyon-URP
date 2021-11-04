using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuInputController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [Header("MainMenu DEBUGGER")]
    [ReadOnly] public bool interactInput;

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            interactInput = true;
        }

        else if (context.canceled)
        {
            interactInput = false;
        }
    }
}
