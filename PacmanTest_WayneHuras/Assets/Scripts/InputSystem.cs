using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class InputSystem : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.PlayerActionMap.Enable();
        playerInputActions.PlayerActionMap.Movement.performed += MovementRequest;
    }

    public void MovementRequest(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        playerMovement.ReceiveMovementRequest(direction);
    }
}
