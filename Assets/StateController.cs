using UnityEngine;

public class StateController : MonoBehaviour {

    public PhysicController physic;
    public FrameController frame;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        CharacterStateFrameEnum currentState = this.frame.currentFrame.state;

        if (!this.physic.isGrounded && currentState != CharacterStateFrameEnum.JUMPING) {
            this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.JUMPING_FALLING);
        }

        if (this.frame.currentFrame.state == CharacterStateFrameEnum.STANDING || this.frame.currentFrame.state == CharacterStateFrameEnum.WALKING) {
            if ((this.frame.inputDirection.x != 0f || this.frame.inputDirection.y != 0f) && this.frame.currentFrame.state != CharacterStateFrameEnum.WALKING) {
                this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.WALKING, false);
            }

            if ((this.frame.inputDirection.x == 0f && this.frame.inputDirection.y == 0f) && this.frame.currentFrame.state == CharacterStateFrameEnum.WALKING) {
                this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.STANDING, false);
            }

            // Run Right
            if (this.frame.runningRightEnable && this.frame.inputDirection.x > 0) {
                this.frame.runningRightEnable = false;
                this.frame.runningLeftEnable = false;
                this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.RUNNING, false);
            }

            // Run Left
            if (this.frame.runningLeftEnable && this.frame.inputDirection.x < 0) {
                this.frame.runningRightEnable = false;
                this.frame.runningLeftEnable = false;
                this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.RUNNING, false);
            }
        }

        switch (currentState) {
            case CharacterStateFrameEnum.WALKING:
                frame.Flip(this.frame.inputDirection);
                break;

            case CharacterStateFrameEnum.RUNNING:
                if (this.frame.facingRight && this.frame.inputDirection.x < 0) {
                    this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.STOP_RUNNING, false);
                    return;
                }
                if (!this.frame.facingRight && this.frame.inputDirection.x > 0) {
                    this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.STOP_RUNNING, false);
                    return;
                }
                break;

            case CharacterStateFrameEnum.STOP_RUNNING:
                frame.runningLeftCount = 0;
                frame.runningLeftEnable = false;
                frame.countLeftEnable = false;

                frame.runningRightCount = 0;
                frame.runningRightEnable = false;
                frame.countRightEnable = false;
                break;

            case CharacterStateFrameEnum.JUMPING_FALLING:
                if (this.physic.isGrounded && this.frame.currentFrame.hit_ground == null) {
                    this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.CROUCH);
                }
                break;
        }
    }
}
