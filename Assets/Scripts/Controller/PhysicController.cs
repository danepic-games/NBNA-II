using System;
using UnityEngine;

public class PhysicController : MonoBehaviour {
    [Header("Generic properties")]
    public Rigidbody rigidbody;

    public CharacterDataController characterDataController;

    public EffectDataController effectDataController;

    public PowerDataController powerDataController;

    public HurtboxController mainHurtbox;
    public HurtboxController secondHurtbox;
    public HurtboxController thirdHurtbox;

    public FrameController frame;

    //Lock by input direction
    public Vector2 lockInputDirection;
    public bool enable_dvz_invocation;

    public bool physicsOneTimePerFrame;

    public Vector3 externForce;
    public bool isExternForce;

    public int currentFrameId;

    public ObjectTypeEnum type;

    // 550 movement
    private float STOP_MOVEMENT = 0;
    private float STOP_MOVEMENT_FRAME_VALUE = 550;

    //Ground Check
    [Header("Character properties")]
    public LayerMask whatIsGround;
    public bool isGrounded;
    public float distanceToCheckGround;
    public float yGroundOrigin;
    public Vector3 groundOrigin = Vector3.zero;

    //Wall Check
    public Transform hurtboxForWall;
    public LayerMask whatIsWall;
    public bool isWalled;
    public float distanceToCheckWallLeftRight;
    public float distanceToCheckWallBackFront;
    public float xWallOrigin;
    public Vector3 wallLeftOrigin = Vector3.zero;
    public Vector3 wallRightOrigin = Vector3.zero;
    public Vector3 wallFrontOrigin = Vector3.zero;
    public Vector3 wallBackOrigin = Vector3.zero;

    void Start() {
        this.physicsOneTimePerFrame = true;
        this.isGrounded = false;
        this.isWalled = false;
        this.currentFrameId = -1;
        this.externForce = Vector3.zero;

        type = GetObjectType();
    }

    void FixedUpdate() {
        if (this.currentFrameId != this.frame.currentFrame.id) {
            this.physicsOneTimePerFrame = true;
            this.currentFrameId = this.frame.currentFrame.id;

            switch (this.frame.currentFrame.state) {
                case StateFrameEnum.JUMPING:
                case StateFrameEnum.JUMPING_FALLING:
                case StateFrameEnum.DOUBLE_JUMPING_FALLING:
                case StateFrameEnum.STOP_RUNNING:
                case StateFrameEnum.JUMP_DEFEND:
                case StateFrameEnum.JUMP_OTHER:
                case StateFrameEnum.DASH_JUMPING:
                    break;
                default:
                    this.rigidbody.velocity = Vector3.zero;
                    break;
            }
        }

        switch (this.type) {
            case ObjectTypeEnum.CHARACTER:
                ApplyCharacterPhysics();
                break;
            case ObjectTypeEnum.EFFECT:
                ApplyEffectPhysics();
                break;
            case ObjectTypeEnum.POWER:
                ApplyPowerPhysics();
                break;
        }
    }

    private void ApplyCharacterPhysics() {
        this.isGrounded = this.IsGroundedRaycast();
        this.isWalled = this.IsWalledRaycast();

        var header = this.characterDataController.header;
        if (this.frame.currentFrame.state == StateFrameEnum.WALKING) {
            float xValue = this.frame.inputDirection.y != 0 ? this.frame.inputDirection.x * (header.walking_speedz / 2) : this.frame.inputDirection.x * (header.walking_speed / 2);
            rigidbody.velocity = new Vector3(xValue, 0, this.frame.inputDirection.y * (header.walking_speedz / 2));
            return;
        }

        if (this.frame.currentFrame.state == StateFrameEnum.RUNNING) {
            float direction = this.frame.facingRight ? 1 : -1;
            float xValue = this.frame.inputDirection.y != 0 ? direction * (((header.running_speedz + header.running_speed) / 2) / 2) : direction * (header.running_speed / 2);
            rigidbody.velocity = new Vector3(xValue, 0, this.frame.inputDirection.y * (header.running_speedz / 2));
            return;
        }

        if (this.physicsOneTimePerFrame) {
            float x = 0;
            float y = 0;
            float z = 0;
            bool affectedByFacing = true;

            if (this.frame.currentFrame.dvx == STOP_MOVEMENT_FRAME_VALUE) {
                rigidbody.velocity = new Vector3(0f, rigidbody.velocity.y, rigidbody.velocity.z);
            } else {
                x = (this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvx);
            }

            if (this.frame.currentFrame.dvy == STOP_MOVEMENT_FRAME_VALUE) {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
            } else {
                y = (this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvy * -1);
            }

            if (this.frame.currentFrame.dvz == STOP_MOVEMENT_FRAME_VALUE) {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0f);
            } else {
                z = frame.inputDirection.y * ((this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvz));
            }

            switch (frame.currentFrame.state) {
                case StateFrameEnum.JUMPING:
                case StateFrameEnum.JUMP_DEFEND:
                case StateFrameEnum.JUMP_OTHER:
                    if (lockInputDirection != Vector2.zero) {
                        x = MathF.Abs(x) * lockInputDirection.x;
                        z = MathF.Abs(z) * lockInputDirection.y;
                        affectedByFacing = false;
                        lockInputDirection = Vector2.zero;
                    } else {
                        x = 0f;
                        z = 0f;
                    }
                    break;
                case StateFrameEnum.DASH_JUMPING:
                    affectedByFacing = true;
                    break;
                case StateFrameEnum.SIDE_DASH:
                    if (this.frame.facingUp) {
                        z = MathF.Abs(this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvz) * 1;
                        affectedByFacing = false;
                    } else {
                        z = MathF.Abs(this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvz) * -1;
                        affectedByFacing = false;
                    }
                    break;
                case StateFrameEnum.HIT_JUMP_DEFEND:
                case StateFrameEnum.HIT_DEFEND:
                    if (this.externForce != Vector3.zero) {
                        x = -this.externForce.x / 2;
                        y = 0f;
                        z = 0f;
                        affectedByFacing = false;
                    }
                    break;
            }

            if (this.frame.externAction) {
                if (this.frame.externItr.action == -1) {
                    //                    Debug.Log("DefendingImpact2 " + this.frame.externItr.x);
                    x = this.frame.externItr.dvx / 2;
                    y = 0f;
                    z = 0f;
                    affectedByFacing = false;
                }
                this.frame.externAction = false;
            }

            ApplyImpulseForce(x, y, z, affectedByFacing);

            return;
        }
    }

    private void ApplyEffectPhysics() {
        if (this.physicsOneTimePerFrame) {
            float x = 0;
            float y = 0;
            float z = 0;

            if (this.frame.currentFrame.dvx == STOP_MOVEMENT_FRAME_VALUE) {
                rigidbody.velocity = new Vector3(0f, rigidbody.velocity.y, rigidbody.velocity.z);
            } else {
                x = (this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvx);
            }

            if (this.frame.currentFrame.dvy == STOP_MOVEMENT_FRAME_VALUE) {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
            } else {
                y = (this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvy * -1);
            }

            if (this.frame.currentFrame.dvz == STOP_MOVEMENT_FRAME_VALUE) {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0f);
            } else {
                z = (this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvz);
            }

            ApplyImpulseForce(x, y, z);
            return;
        }
    }

    private void ApplyPowerPhysics() {
        this.isGrounded = this.IsGroundedRaycast();
        this.isWalled = this.IsWalledRaycast();

        Debug.Log(enable_dvz_invocation + " - " + lockInputDirection);

        if (this.physicsOneTimePerFrame) {
            float x = 0;
            float y = 0;
            float z = 0;

            if (this.frame.currentFrame.dvx == STOP_MOVEMENT_FRAME_VALUE) {
                rigidbody.velocity = new Vector3(0f, rigidbody.velocity.y, rigidbody.velocity.z);
            } else {
                x = (this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvx);
            }

            if (this.frame.currentFrame.dvy == STOP_MOVEMENT_FRAME_VALUE) {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
            } else {
                y = (this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvy * -1);
            }

            if (this.frame.currentFrame.dvz == STOP_MOVEMENT_FRAME_VALUE) {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0f);
            } else {
                z = (this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvz);
            }

            if (enable_dvz_invocation) {
                z = z * lockInputDirection.y;
            }

            ApplyImpulseForce(x, y, z);
            return;
        }
    }

    private ObjectTypeEnum GetObjectType() {
        if (characterDataController != null) {
            return characterDataController.type;
        }

        if (effectDataController != null) {
            return effectDataController.type;
        }

        if (powerDataController != null) {
            return powerDataController.type;
        }

        throw new MissingFieldException("Objeto Data não encontrado no script de fisíca!");
    }

    private void ApplyImpulseForce(float x, float y, float z, bool affectedByFacing = true) {
        if (affectedByFacing) {
            x = PositionUtil.FacingByX(x, frame.facingRight);
        }
        var velocity = new Vector3(x, y, z);
        if (velocity != Vector3.zero) {
            //            Debug.Log(name + ":" + frame.currentFrame.id + ":" + velocity);
            rigidbody.AddForce(velocity, ForceMode.Impulse);
            this.physicsOneTimePerFrame = false;
        }
    }

    private bool DownRaycastIsHit(CastOrientationEnum orientation) {
        RaycastHit hit;
        switch (orientation) {
            case CastOrientationEnum.CENTER:
                groundOrigin = new Vector3(mainHurtbox.transform.position.x, mainHurtbox.transform.position.y - (mainHurtbox.transform.localScale.y / 2) - yGroundOrigin, mainHurtbox.transform.position.z);
                break;
            case CastOrientationEnum.LEFT:
                groundOrigin = new Vector3(mainHurtbox.transform.position.x - (mainHurtbox.transform.localScale.x / 2), mainHurtbox.transform.position.y - (mainHurtbox.transform.localScale.y / 2) - yGroundOrigin, mainHurtbox.transform.position.z);
                break;
            case CastOrientationEnum.RIGHT:
                groundOrigin = new Vector3(mainHurtbox.transform.position.x + (mainHurtbox.transform.localScale.x / 2), mainHurtbox.transform.position.y - (mainHurtbox.transform.localScale.y / 2) - yGroundOrigin, mainHurtbox.transform.position.z);
                break;
            case CastOrientationEnum.FRONT:
                groundOrigin = new Vector3(mainHurtbox.transform.position.x, mainHurtbox.transform.position.y - (mainHurtbox.transform.localScale.y / 2) - yGroundOrigin, mainHurtbox.transform.position.z - (mainHurtbox.transform.localScale.z / 2));
                break;
            case CastOrientationEnum.BACK:
                groundOrigin = new Vector3(mainHurtbox.transform.position.x, mainHurtbox.transform.position.y - (mainHurtbox.transform.localScale.y / 2) - yGroundOrigin, mainHurtbox.transform.position.z + (mainHurtbox.transform.localScale.z / 2));
                break;
            case CastOrientationEnum.LEFT_DOWN:
                groundOrigin = new Vector3(mainHurtbox.transform.position.x - (mainHurtbox.transform.localScale.x / 2), mainHurtbox.transform.position.y - (mainHurtbox.transform.localScale.y / 2) - yGroundOrigin, mainHurtbox.transform.position.z - (mainHurtbox.transform.localScale.z / 2));
                break;
            case CastOrientationEnum.LEFT_UP:
                groundOrigin = new Vector3(mainHurtbox.transform.position.x - (mainHurtbox.transform.localScale.x / 2), mainHurtbox.transform.position.y - (mainHurtbox.transform.localScale.y / 2) - yGroundOrigin, mainHurtbox.transform.position.z + (mainHurtbox.transform.localScale.z / 2));
                break;
            case CastOrientationEnum.RIGHT_DOWN:
                groundOrigin = new Vector3(mainHurtbox.transform.position.x + (mainHurtbox.transform.localScale.x / 2), mainHurtbox.transform.position.y - (mainHurtbox.transform.localScale.y / 2) - yGroundOrigin, mainHurtbox.transform.position.z - (mainHurtbox.transform.localScale.z / 2));
                break;
            case CastOrientationEnum.RIGHT_UP:
                groundOrigin = new Vector3(mainHurtbox.transform.position.x + (mainHurtbox.transform.localScale.x / 2), mainHurtbox.transform.position.y - (mainHurtbox.transform.localScale.y / 2) - yGroundOrigin, mainHurtbox.transform.position.z + (mainHurtbox.transform.localScale.z / 2));
                break;
        }

        return Physics.Raycast(groundOrigin, Vector3.down, out hit, distanceToCheckGround, whatIsGround);
    }

    private bool IsGroundedRaycast() {
        if (mainHurtbox) {
            return DownRaycastIsHit(CastOrientationEnum.CENTER) || DownRaycastIsHit(CastOrientationEnum.LEFT) ||
            DownRaycastIsHit(CastOrientationEnum.RIGHT) || DownRaycastIsHit(CastOrientationEnum.FRONT) ||
            DownRaycastIsHit(CastOrientationEnum.BACK);
        }

        return false;
    }

    private bool IsWalledRaycast() {
        if (mainHurtbox) {
            if (mainHurtbox.bdy != null && mainHurtbox.bdy.wallCheck) {
                this.hurtboxForWall = mainHurtbox.transform;
                return IsLeftWalledRaycast() || IsRightWalledRaycast() || IsFrontWalledRaycast()|| IsBackWalledRaycast();
            }
        }

        if (secondHurtbox) {
            if (secondHurtbox.bdy != null && secondHurtbox.bdy.wallCheck) {
                this.hurtboxForWall = secondHurtbox.transform;
                return IsLeftWalledRaycast() || IsRightWalledRaycast() || IsFrontWalledRaycast()|| IsBackWalledRaycast();
            }
        }

        if (thirdHurtbox) {
            if (thirdHurtbox.bdy != null && thirdHurtbox.bdy.wallCheck) {
                this.hurtboxForWall = thirdHurtbox.transform;
                return IsLeftWalledRaycast() || IsRightWalledRaycast() || IsFrontWalledRaycast()|| IsBackWalledRaycast();
            }
        }
        return false;
    }

    private bool IsLeftWalledRaycast() {
        return LeftRaycastIsHit(CastOrientationEnum.CENTER) || LeftRaycastIsHit(CastOrientationEnum.LEFT) ||
        LeftRaycastIsHit(CastOrientationEnum.RIGHT) || LeftRaycastIsHit(CastOrientationEnum.FRONT) ||
        LeftRaycastIsHit(CastOrientationEnum.BACK);
    }

    private bool IsRightWalledRaycast() {
        return RightRaycastIsHit(CastOrientationEnum.CENTER) || RightRaycastIsHit(CastOrientationEnum.LEFT) ||
        RightRaycastIsHit(CastOrientationEnum.RIGHT) || RightRaycastIsHit(CastOrientationEnum.FRONT) ||
        RightRaycastIsHit(CastOrientationEnum.BACK);
    }

    private bool IsFrontWalledRaycast() {
        return FrontRaycastIsHit(CastOrientationEnum.CENTER) || FrontRaycastIsHit(CastOrientationEnum.LEFT) ||
        FrontRaycastIsHit(CastOrientationEnum.RIGHT) || FrontRaycastIsHit(CastOrientationEnum.FRONT) ||
        FrontRaycastIsHit(CastOrientationEnum.BACK);
    }

    private bool IsBackWalledRaycast() {
        return BackRaycastIsHit(CastOrientationEnum.CENTER) || BackRaycastIsHit(CastOrientationEnum.LEFT) ||
        BackRaycastIsHit(CastOrientationEnum.RIGHT) || BackRaycastIsHit(CastOrientationEnum.FRONT) ||
        BackRaycastIsHit(CastOrientationEnum.BACK);
    }

    private bool LeftRaycastIsHit(CastOrientationEnum orientation) {
        RaycastHit hit;

        switch (orientation) {
            case CastOrientationEnum.CENTER:
                wallLeftOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2) - xWallOrigin, hurtboxForWall.position.y, hurtboxForWall.position.z);
                break;
            case CastOrientationEnum.LEFT:
                wallLeftOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2) - xWallOrigin, hurtboxForWall.position.y, hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2));
                break;
            case CastOrientationEnum.RIGHT:
                wallLeftOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2) - xWallOrigin, hurtboxForWall.position.y, hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2));
                break;
            case CastOrientationEnum.UP:
                wallLeftOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2) - xWallOrigin, hurtboxForWall.position.y + (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z);
                break;
            case CastOrientationEnum.DOWN:
                wallLeftOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2) - xWallOrigin, hurtboxForWall.position.y - (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z);
                break;
            case CastOrientationEnum.LEFT_DOWN:
                wallLeftOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2) - xWallOrigin, hurtboxForWall.position.y - (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2));
                break;
            case CastOrientationEnum.LEFT_UP:
                wallLeftOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2) - xWallOrigin, hurtboxForWall.position.y + (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2));
                break;
            case CastOrientationEnum.RIGHT_DOWN:
                wallLeftOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2) - xWallOrigin, hurtboxForWall.position.y - (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2));
                break;
            case CastOrientationEnum.RIGHT_UP:
                wallLeftOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2) - xWallOrigin, hurtboxForWall.position.y + (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2));
                break;
        }

        return Physics.Raycast(wallLeftOrigin, Vector3.left, out hit, distanceToCheckWallLeftRight, whatIsWall);
    }

    private bool RightRaycastIsHit(CastOrientationEnum orientation) {
        RaycastHit hit;

        switch (orientation) {
            case CastOrientationEnum.CENTER:
                wallRightOrigin = new Vector3(hurtboxForWall.position.x + (hurtboxForWall.localScale.x / 2) + xWallOrigin, hurtboxForWall.position.y, hurtboxForWall.position.z);
                break;
            case CastOrientationEnum.LEFT:
                wallRightOrigin = new Vector3(hurtboxForWall.position.x + (hurtboxForWall.localScale.x / 2) + xWallOrigin, hurtboxForWall.position.y, hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2));
                break;
            case CastOrientationEnum.RIGHT:
                wallRightOrigin = new Vector3(hurtboxForWall.position.x + (hurtboxForWall.localScale.x / 2) + xWallOrigin, hurtboxForWall.position.y, hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2));
                break;
            case CastOrientationEnum.UP:
                wallRightOrigin = new Vector3(hurtboxForWall.position.x + (hurtboxForWall.localScale.x / 2) + xWallOrigin, hurtboxForWall.position.y + (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z);
                break;
            case CastOrientationEnum.DOWN:
                wallRightOrigin = new Vector3(hurtboxForWall.position.x + (hurtboxForWall.localScale.x / 2) + xWallOrigin, hurtboxForWall.position.y - (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z);
                break;
            case CastOrientationEnum.LEFT_DOWN:
                wallRightOrigin = new Vector3(hurtboxForWall.position.x + (hurtboxForWall.localScale.x / 2) + xWallOrigin, hurtboxForWall.position.y - (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2));
                break;
            case CastOrientationEnum.LEFT_UP:
                wallRightOrigin = new Vector3(hurtboxForWall.position.x + (hurtboxForWall.localScale.x / 2) + xWallOrigin, hurtboxForWall.position.y + (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2));
                break;
            case CastOrientationEnum.RIGHT_DOWN:
                wallRightOrigin = new Vector3(hurtboxForWall.position.x + (hurtboxForWall.localScale.x / 2) + xWallOrigin, hurtboxForWall.position.y - (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2));
                break;
            case CastOrientationEnum.RIGHT_UP:
                wallRightOrigin = new Vector3(hurtboxForWall.position.x + (hurtboxForWall.localScale.x / 2) + xWallOrigin, hurtboxForWall.position.y + (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2));
                break;
        }

        return Physics.Raycast(wallRightOrigin, Vector3.right, out hit, distanceToCheckWallLeftRight, whatIsWall);
    }

    private bool FrontRaycastIsHit(CastOrientationEnum orientation) {
        RaycastHit hit;

        switch (orientation) {
            case CastOrientationEnum.CENTER:
                wallFrontOrigin = new Vector3(hurtboxForWall.position.x, hurtboxForWall.position.y, hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2) + xWallOrigin);
                break;
            case CastOrientationEnum.LEFT:
                wallFrontOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2), hurtboxForWall.position.y, hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2) + xWallOrigin);
                break;
            case CastOrientationEnum.RIGHT:
                wallFrontOrigin = new Vector3(hurtboxForWall.position.x + (hurtboxForWall.localScale.x / 2), hurtboxForWall.position.y, hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2) + xWallOrigin);
                break;
            case CastOrientationEnum.UP:
                wallFrontOrigin = new Vector3(hurtboxForWall.position.x, hurtboxForWall.position.y + (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2) + xWallOrigin);
                break;
            case CastOrientationEnum.DOWN:
                wallFrontOrigin = new Vector3(hurtboxForWall.position.x, hurtboxForWall.position.y - (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2) + xWallOrigin);
                break;
            case CastOrientationEnum.LEFT_DOWN:
                wallFrontOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2), hurtboxForWall.position.y - (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2) + xWallOrigin);
                break;
            case CastOrientationEnum.LEFT_UP:
                wallFrontOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2), hurtboxForWall.position.y + (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2) + xWallOrigin);
                break;
            case CastOrientationEnum.RIGHT_DOWN:
                wallFrontOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2), hurtboxForWall.position.y - (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2) + xWallOrigin);
                break;
            case CastOrientationEnum.RIGHT_UP:
                wallFrontOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2), hurtboxForWall.position.y + (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z - (hurtboxForWall.localScale.z / 2) + xWallOrigin);
                break;
        }

        return Physics.Raycast(wallFrontOrigin, Vector3.forward, out hit, distanceToCheckWallBackFront, whatIsWall);
    }

    private bool BackRaycastIsHit(CastOrientationEnum orientation) {
        RaycastHit hit;

        switch (orientation) {
            case CastOrientationEnum.CENTER:
                wallBackOrigin = new Vector3(hurtboxForWall.position.x, hurtboxForWall.position.y, hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2) - xWallOrigin);
                break;
            case CastOrientationEnum.LEFT:
                wallBackOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2), hurtboxForWall.position.y, hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2) - xWallOrigin);
                break;
            case CastOrientationEnum.RIGHT:
                wallBackOrigin = new Vector3(hurtboxForWall.position.x + (hurtboxForWall.localScale.x / 2), hurtboxForWall.position.y, hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2) - xWallOrigin);
                break;
            case CastOrientationEnum.UP:
                wallBackOrigin = new Vector3(hurtboxForWall.position.x, hurtboxForWall.position.y + (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2) - xWallOrigin);
                break;
            case CastOrientationEnum.DOWN:
                wallBackOrigin = new Vector3(hurtboxForWall.position.x, hurtboxForWall.position.y - (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2) - xWallOrigin);
                break;
            case CastOrientationEnum.LEFT_DOWN:
                wallBackOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2), hurtboxForWall.position.y - (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2) - xWallOrigin);
                break;
            case CastOrientationEnum.LEFT_UP:
                wallBackOrigin = new Vector3(hurtboxForWall.position.x - (hurtboxForWall.localScale.x / 2), hurtboxForWall.position.y + (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2) - xWallOrigin);
                break;
            case CastOrientationEnum.RIGHT_DOWN:
                wallBackOrigin = new Vector3(hurtboxForWall.position.x + (hurtboxForWall.localScale.x / 2), hurtboxForWall.position.y - (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2) - xWallOrigin);
                break;
            case CastOrientationEnum.RIGHT_UP:
                wallBackOrigin = new Vector3(hurtboxForWall.position.x + (hurtboxForWall.localScale.x / 2), hurtboxForWall.position.y + (hurtboxForWall.localScale.y / 2), hurtboxForWall.position.z + (hurtboxForWall.localScale.z / 2) - xWallOrigin);
                break;
        }

        return Physics.Raycast(wallBackOrigin, Vector3.back, out hit, distanceToCheckWallBackFront, whatIsWall);
    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        var values = Enum.GetValues(typeof(CastOrientationEnum));
        Gizmos.color = Color.green;

        foreach (CastOrientationEnum orientation in values) {

            if (mainHurtbox) {
                bool isGroundHit = this.DownRaycastIsHit(orientation);
                Gizmos.DrawRay(groundOrigin, Vector3.down * distanceToCheckGround);
            }

            if (hurtboxForWall) {
                bool isLeftWallHit = this.LeftRaycastIsHit(orientation);
                bool isRightWallHit = this.RightRaycastIsHit(orientation);
                bool isFrontWallHit = this.FrontRaycastIsHit(orientation);
                bool isBackWallHit = this.BackRaycastIsHit(orientation);

                Gizmos.DrawRay(wallLeftOrigin, Vector3.left * distanceToCheckWallLeftRight);
                Gizmos.DrawRay(wallRightOrigin, Vector3.right * distanceToCheckWallLeftRight);
                Gizmos.DrawRay(wallFrontOrigin, Vector3.forward * distanceToCheckWallBackFront);
                Gizmos.DrawRay(wallBackOrigin, Vector3.back * distanceToCheckWallBackFront);
            }
        }
    }
#endif
}
