using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public PhysicController physic;
    public FrameController frame;

    private List<StateFrameEnum> airFramesEnabled = new List<StateFrameEnum>();

    // Start is called before the first frame update
    void Start() {
        airFramesEnabled.Add(StateFrameEnum.JUMPING);
        airFramesEnabled.Add(StateFrameEnum.JUMPING_FALLING);
        airFramesEnabled.Add(StateFrameEnum.DOUBLE_JUMPING_FALLING);
        airFramesEnabled.Add(StateFrameEnum.JUMP_DASH);
        airFramesEnabled.Add(StateFrameEnum.JUMP_DEFEND);
        airFramesEnabled.Add(StateFrameEnum.HIT_JUMP_DEFEND);
        airFramesEnabled.Add(StateFrameEnum.JUMP_OTHER);
        airFramesEnabled.Add(StateFrameEnum.DASH_JUMPING);
    }

    // Update is called once per frame
    void FixedUpdate() {
        StateFrameEnum currentState = this.frame.currentFrame.state;

        switch (this.frame.data.type) {
            case ObjectTypeEnum.CHARACTER:
                ApplyStateForCharacter(currentState);
                break;
            case ObjectTypeEnum.POWER:
                ApplyStateForPower(currentState);
                break;
        }

        switch (currentState) {
            case StateFrameEnum.REPEAT_ANIMATION_AT:
                this.frame.repeatAnimationAt = this.frame.currentFrame.repeat_at > 0 ? this.frame.currentFrame.repeat_at / 30 : 0;
                break;
        }
    }

    private void ApplyStateForCharacter(StateFrameEnum currentState) {
        if (!this.physic.isGrounded && !airFramesEnabled.Contains(currentState)) {
            this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.JUMPING_FALLING);
        }

        if (frame.debugFrameToGo) {
            Debug.Log("currentState: " + currentState + " frame id: " + frame.currentFrame.id);
        }

        if (currentState != StateFrameEnum.INJURED && currentState != StateFrameEnum.INJURED_2) {
            this.frame.injuredCount = 0;
        }

        if (this.physic.isGrounded) {
            this.frame.ChangeFrame(this.frame.currentFrame.hit_ground);
        }

        if (this.physic.isWalled) {
            this.frame.ChangeFrame(this.frame.currentFrame.hit_wall);
        }

        if (!this.physic.isGrounded && this.frame.currentFrame.state == StateFrameEnum.JUMPING || this.frame.currentFrame.state == StateFrameEnum.JUMPING_FALLING) {
            // Run Right
            if (this.frame.runningRightEnable && this.frame.inputDirection.x > 0) {
                this.frame.runningRightEnable = false;
                this.frame.runningLeftEnable = false;
                this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.JUMP_DASH, false);
            }

            // Run Left
            if (this.frame.runningLeftEnable && this.frame.inputDirection.x < 0) {
                this.frame.runningRightEnable = false;
                this.frame.runningLeftEnable = false;
                this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.JUMP_DASH, false);
            }
        }

        if (this.frame.currentFrame.state == StateFrameEnum.STANDING || this.frame.currentFrame.state == StateFrameEnum.WALKING) {
            if ((this.frame.inputDirection.x != 0f || this.frame.inputDirection.y != 0f) && this.frame.currentFrame.state != StateFrameEnum.WALKING) {
                this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.WALKING, false);
            }

            if ((this.frame.inputDirection.x == 0f && this.frame.inputDirection.y == 0f) && this.frame.currentFrame.state == StateFrameEnum.WALKING) {
                this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.STANDING, false);
            }

            // Run Right
            if (this.frame.runningRightEnable && this.frame.inputDirection.x > 0) {
                this.frame.runningRightEnable = false;
                this.frame.runningLeftEnable = false;
                this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.SIMPLE_DASH, false);
            }

            // Run Left
            if (this.frame.runningLeftEnable && this.frame.inputDirection.x < 0) {
                this.frame.runningRightEnable = false;
                this.frame.runningLeftEnable = false;
                this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.SIMPLE_DASH, false);
            }

            // Side Dash Up
            if (this.frame.sideDashUpEnable && this.frame.inputDirection.y > 0) {
                this.frame.sideDashUpEnable = false;
                this.frame.sideDashDownEnable = false;
                this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.SIDE_DASH, false);
            }

            // Side Dash Down
            if (this.frame.sideDashDownEnable && this.frame.inputDirection.y < 0) {
                this.frame.sideDashUpEnable = false;
                this.frame.sideDashDownEnable = false;
                this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.SIDE_DASH, false);
            }
        }

        switch (currentState) {
            case StateFrameEnum.WALKING:
                frame.Flip(this.frame.inputDirection);
                break;

            case StateFrameEnum.RUNNING:
                if (this.frame.facingRight && this.frame.inputDirection.x < 0) {
                    this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.STOP_RUNNING, false);
                    return;
                }
                if (!this.frame.facingRight && this.frame.inputDirection.x > 0) {
                    this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.STOP_RUNNING, false);
                    return;
                }
                break;

            case StateFrameEnum.STOP_RUNNING:
                frame.runningLeftCount = 0;
                frame.runningLeftEnable = false;
                frame.countLeftEnable = false;

                frame.runningRightCount = 0;
                frame.runningRightEnable = false;
                frame.countRightEnable = false;
                break;

            case StateFrameEnum.DOUBLE_JUMPING_FALLING:
            case StateFrameEnum.JUMPING_FALLING:
                frame.Flip(this.frame.inputDirection);
                if (this.physic.isGrounded && this.frame.currentFrame.hit_ground == null) {
                    this.frame.ChangeFrame(CharacterSpecialStartFrameEnum.CROUCH);
                }
                break;

            case StateFrameEnum.DASH_JUMPING:
            case StateFrameEnum.JUMPING:
                frame.Flip(this.frame.inputDirection);
                break;

            case StateFrameEnum.JUMPING_CHARGE:
                frame.Flip(this.frame.inputDirection);
                this.physic.lockInputDirection = this.frame.inputDirection;
                break;

            case StateFrameEnum.HIT_JUMP_DEFEND:
            case StateFrameEnum.HIT_DEFEND:
                this.physic.externForce = new Vector3(this.frame.externItr.dvx, this.frame.externItr.dvy, this.frame.externItr.dvz);
                this.physic.enemyFacingRight = this.frame.enemyFacingRight;
                break;

            case StateFrameEnum.INJURED:
            case StateFrameEnum.INJURED_2:
                this.physic.externForce = new Vector3(this.frame.externItr.dvx, this.frame.externItr.dvy, this.frame.externItr.dvz);
                this.physic.enemyFacingRight = this.frame.enemyFacingRight;
                break;
        }
    }

    private void ApplyStateForPower(StateFrameEnum currentState) {
        if (this.physic.isGrounded) {
            this.frame.ChangeFrame(this.frame.currentFrame.hit_ground);
        }

        if (this.physic.isWalled) {
            this.frame.ChangeFrame(this.frame.currentFrame.hit_wall);
        }
    }
}
