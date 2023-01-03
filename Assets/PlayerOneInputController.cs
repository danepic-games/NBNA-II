using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOneInputController : MonoBehaviour {

    public FrameController frame;

    private PlayerInputActions playerInputActions;

    void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.@Player.Enable();
        playerInputActions.@Player.@Jump.started += HitJump;
        playerInputActions.@Player.@Attack.started += HitAttack;
        playerInputActions.@Player.@MoveX.started += HitMovementX;
        playerInputActions.@Player.@MoveX.canceled += CancelMovementX;
        playerInputActions.@Player.@MoveZ.started += HitMovementZ;
        playerInputActions.@Player.@MoveZ.canceled += CancelMovementZ;
    }

    private void HitJump(InputAction.CallbackContext context) {
        this.frame.hitJump = true;
    }

    private void HitAttack(InputAction.CallbackContext context) {
        this.frame.hitAttack = true;
    }

    private void HitMovementX(InputAction.CallbackContext context) {
        this.frame.inputDirection.x = context.ReadValue<Vector2>().x;
    }

    private void CancelMovementX(InputAction.CallbackContext context) {
        this.frame.inputDirection.x = context.ReadValue<Vector2>().x;
        if (!this.frame.countRightEnable && this.frame.facingRight) {
            this.frame.countRightEnable = true;
            this.frame.countLeftEnable = false;
            return;
        }
        if (!this.frame.countLeftEnable && !this.frame.facingRight) {
            this.frame.countLeftEnable = true;
            this.frame.countRightEnable = false;
            return;
        }
    }

    private void HitMovementZ(InputAction.CallbackContext context) {
        this.frame.inputDirection.y = context.ReadValue<Vector2>().y;
    }

    private void CancelMovementZ(InputAction.CallbackContext context) {
        this.frame.inputDirection.y = context.ReadValue<Vector2>().y;
    }
}
