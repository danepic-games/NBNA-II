using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour {

    public FrameController frame;

    public PlayerEnum input;

    private PlayerInputActions playerInputActions;

    void Start() {
        switch (input) {
            case PlayerEnum.PLAYER_ONE:
                playerInputActions = new PlayerInputActions();
                playerInputActions.@Player1.Enable();
                playerInputActions.@Player1.@Jump.started += HitJump;
                playerInputActions.@Player1.@Attack.started += HitAttack;
                playerInputActions.@Player1.@MoveX.started += HitMovementX;
                playerInputActions.@Player1.@MoveX.canceled += CancelMovementX;
                playerInputActions.@Player1.@MoveZ.started += HitMovementZ;
                playerInputActions.@Player1.@MoveZ.canceled += CancelMovementZ;
                break;
            case PlayerEnum.PLAYER_TWO:
                playerInputActions = new PlayerInputActions();
                playerInputActions.@Player2.Enable();
                playerInputActions.@Player2.@Jump.started += HitJump;
                playerInputActions.@Player2.@Attack.started += HitAttack;
                playerInputActions.@Player2.@MoveX.started += HitMovementX;
                playerInputActions.@Player2.@MoveX.canceled += CancelMovementX;
                playerInputActions.@Player2.@MoveZ.started += HitMovementZ;
                playerInputActions.@Player2.@MoveZ.canceled += CancelMovementZ;
                break;
            default:
                break;
        }
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
