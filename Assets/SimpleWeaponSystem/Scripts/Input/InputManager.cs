using System;
using UnityEngine.InputSystem;
using WeaponSystem.Core;
using WeaponSystem.Input;

public class InputManager : SingletonController<InputManager>
{
    public event Action OnPrimaryPerformed;
    public event Action OnSecondaryPerformed;

    public BasicControls InputActions;

    private void OnEnable()
    {
        InputActions = new BasicControls();
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void EnableInput()
    {
        InputActions.Enable();
        InputActions.Default.PrimaryAction.performed += HandlePrimaryPerformed;
        InputActions.Default.SecondaryAction.performed += HandleSecondaryPerformed;
    }

    private void DisableInput()
    {
        InputActions.Disable();
        InputActions.Default.PrimaryAction.performed -= HandlePrimaryPerformed;
        InputActions.Default.SecondaryAction.performed -= HandleSecondaryPerformed;
    }

    private void HandlePrimaryPerformed(InputAction.CallbackContext context) => OnPrimaryPerformed?.Invoke();
    private void HandleSecondaryPerformed(InputAction.CallbackContext context) => OnSecondaryPerformed?.Invoke();
}
