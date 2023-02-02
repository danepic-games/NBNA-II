using System;
using UnityEngine;

public class PhysicController : MonoBehaviour {
    [Header("Generic properties")]
    public Rigidbody rigidbody;

    public CharacterDataController characterDataController;

    public EffectDataController effectDataController;

    public FrameController frame;

    //Lock by input direction
    public Vector2 lockInputDirection;

    public bool physicsOneTimePerFrame;

    public Vector3 externForce;
    public bool isExternForce;

    public int currentFrameId;

    public ObjectTypeEnum type;

    // 550 movement
    private float STOP_MOVEMENT = 0;
    private float STOP_MOVEMENT_FRAME_VALUE = 550;

    [Header("Character properties")]

    //Ground Check
    public LayerMask whatIsGround;
    public bool isGrounded;
    public float distanceToCheckGround;
    public float yGroundOrigin;
    public float xGroundOrigin;
    public float zGroundOrigin;
    public Vector3 groundOrigin = Vector3.zero;

    void Start() {
        this.physicsOneTimePerFrame = true;
        this.isGrounded = false;
        this.currentFrameId = -1;

        type = GetObjectType();
    }

    void FixedUpdate() {
        if (this.currentFrameId != this.frame.currentFrame.id) {
            this.physicsOneTimePerFrame = true;
            this.currentFrameId = this.frame.currentFrame.id;

            switch (this.frame.currentFrame.state) {
                case StateFrameEnum.JUMPING:
                case StateFrameEnum.JUMPING_FALLING:
                case StateFrameEnum.STOP_RUNNING:
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
        }
    }

    private bool IsGroundedRaycast() {
        bool centerRaycastHit = CheckRaycast(RaycastOrientationEnum.CENTER);
        bool leftRaycastHit = CheckRaycast(RaycastOrientationEnum.LEFT);
        bool rightRaycastHit = CheckRaycast(RaycastOrientationEnum.RIGHT);
        bool frontRaycastHit = CheckRaycast(RaycastOrientationEnum.FRONT);
        bool backRaycastHit = CheckRaycast(RaycastOrientationEnum.BACK);
        return centerRaycastHit || leftRaycastHit || rightRaycastHit || frontRaycastHit || backRaycastHit;
    }

    private bool CheckRaycast(RaycastOrientationEnum orientation) {
        RaycastHit hit;

        switch (orientation) {
            case RaycastOrientationEnum.CENTER:
                groundOrigin = new Vector3(transform.position.x, transform.position.y - yGroundOrigin, transform.position.z);
                break;
            case RaycastOrientationEnum.LEFT:
                groundOrigin = new Vector3(transform.position.x - xGroundOrigin, transform.position.y - yGroundOrigin, transform.position.z);
                break;
            case RaycastOrientationEnum.RIGHT:
                groundOrigin = new Vector3(transform.position.x + xGroundOrigin, transform.position.y - yGroundOrigin, transform.position.z);
                break;
            case RaycastOrientationEnum.FRONT:
                groundOrigin = new Vector3(transform.position.x, transform.position.y - yGroundOrigin, transform.position.z - zGroundOrigin);
                break;
            case RaycastOrientationEnum.BACK:
                groundOrigin = new Vector3(transform.position.x, transform.position.y - yGroundOrigin, transform.position.z + zGroundOrigin);
                break;
        }

        return Physics.Raycast(groundOrigin, Vector3.down, out hit, distanceToCheckGround, whatIsGround);
    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        RaycastHit hit;

        var values = Enum.GetValues(typeof(RaycastOrientationEnum));

        foreach (RaycastOrientationEnum orientation in values) {
            switch (orientation) {
                case RaycastOrientationEnum.CENTER:
                    groundOrigin = new Vector3(transform.position.x, transform.position.y - yGroundOrigin, transform.position.z);
                    break;
                case RaycastOrientationEnum.LEFT:
                    groundOrigin = new Vector3(transform.position.x - xGroundOrigin, transform.position.y - yGroundOrigin, transform.position.z);
                    break;
                case RaycastOrientationEnum.RIGHT:
                    groundOrigin = new Vector3(transform.position.x + xGroundOrigin, transform.position.y - yGroundOrigin, transform.position.z);
                    break;
                case RaycastOrientationEnum.FRONT:
                    groundOrigin = new Vector3(transform.position.x, transform.position.y - yGroundOrigin, transform.position.z - zGroundOrigin);
                    break;
                case RaycastOrientationEnum.BACK:
                    groundOrigin = new Vector3(transform.position.x, transform.position.y - yGroundOrigin, transform.position.z + zGroundOrigin);
                    break;
            }

            bool isHit = Physics.Raycast(groundOrigin, Vector3.down, out hit, distanceToCheckGround, whatIsGround);
            if (isHit) {
                Gizmos.color = Color.red;
            } else {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(groundOrigin, Vector3.down * distanceToCheckGround);
            }
        }
    }
#endif

    private void ApplyCharacterPhysics() {
        this.isGrounded = this.IsGroundedRaycast();

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
                case StateFrameEnum.SIDE_DASH:
                    if (this.frame.facingUp) {
                        z = MathF.Abs(this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvz) * 1;
                        affectedByFacing = false;
                    } else {
                        z = MathF.Abs(this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvz) * -1;
                        affectedByFacing = false;
                    }
                    Debug.Log(z);
                    break;
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

    private ObjectTypeEnum GetObjectType() {
        if (characterDataController != null) {
            return characterDataController.type;
        }

        if (effectDataController != null) {
            return effectDataController.type;
        }

        throw new MissingFieldException("Objeto Data não encontrado no script de fisíca!");
    }

    private void ApplyImpulseForce(float x, float y, float z, bool affectedByFacing = true) {
        if (affectedByFacing) {
            x = PositionUtil.FacingByX(x, frame.facingRight);
        }
        var velocity = new Vector3(x, y, z);
        if (velocity != Vector3.zero) {
            rigidbody.AddForce(velocity, ForceMode.Impulse);
            this.physicsOneTimePerFrame = false;
        }
    }
}
