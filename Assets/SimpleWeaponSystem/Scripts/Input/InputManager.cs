using System;
using UnityEngine.InputSystem;
using WeaponSystem.Core;
using WeaponSystem.Input;

public class InputManager : SingletonController<InputManager>
{
    public event Action OnPrimaryPerformed;
    public event Action OnSecondaryPerformed;
    public event Action OnReloadPerformed;
    public event Action OnUsePerformed;

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
        InputActions.Default.ReloadAction.performed += HandleReloadPerformed;
        InputActions.Default.UseAction.performed += HandleUsePerformed;
    }

    private void DisableInput()
    {
        InputActions.Disable();
        InputActions.Default.PrimaryAction.performed -= HandlePrimaryPerformed;
        InputActions.Default.SecondaryAction.performed -= HandleSecondaryPerformed;
        InputActions.Default.ReloadAction.performed -= HandleReloadPerformed;
        InputActions.Default.UseAction.performed -= HandleUsePerformed;
    }

    private void HandlePrimaryPerformed(InputAction.CallbackContext context) => OnPrimaryPerformed?.Invoke();
    private void HandleSecondaryPerformed(InputAction.CallbackContext context) => OnSecondaryPerformed?.Invoke();
    private void HandleReloadPerformed(InputAction.CallbackContext context) => OnReloadPerformed?.Invoke();
    private void HandleUsePerformed(InputAction.CallbackContext context) => OnUsePerformed?.Invoke();
}
