using System;
using UnityEngine;

public class PhysicController : MonoBehaviour {
    public Rigidbody rigidbody;

    public BoxCollider boxCollider;

    public CharacterDataController data;

    public FrameController frame;

    public bool physicsOneTimePerFrame;

    public int currentFrameId;

    // 550 movement
    private float STOP_MOVEMENT = 0;

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
        this.currentFrameId = 0;
    }

    void FixedUpdate() {
        this.isGrounded = this.IsGroundedRaycast();

        if (this.currentFrameId != this.frame.currentFrame.id) {
            this.physicsOneTimePerFrame = true;
            this.currentFrameId = this.frame.currentFrame.id;
        }

        if (this.frame.currentFrame.state == CharacterStateFrameEnum.WALKING) {
            float xValue = this.frame.inputDirection.y != 0 ? this.frame.inputDirection.x * (this.data.header.walking_speedz / 2) : this.frame.inputDirection.x * (this.data.header.walking_speed / 2);
            rigidbody.velocity = new Vector3(xValue, 0, this.frame.inputDirection.y * (this.data.header.walking_speedz / 2));
        }

        if (this.frame.currentFrame.state == CharacterStateFrameEnum.RUNNING) {
            float direction = this.frame.facingRight ? 1 : -1;
            float xValue = this.frame.inputDirection.y != 0 ? direction * (((this.data.header.running_speedz + this.data.header.running_speed) / 2) / 2) : direction * (this.data.header.running_speed / 2);
            rigidbody.velocity = new Vector3(xValue, 0, this.frame.inputDirection.y * (this.data.header.running_speedz / 2));
        }

        if (this.physicsOneTimePerFrame) {
            float x = 0;
            float y = 0;
            float z = 0;

            if (this.frame.currentFrame.state == CharacterStateFrameEnum.JUMPING) {
                if (this.frame.currentFrame.dvy != 0 && this.frame.currentFrame.dvy != STOP_MOVEMENT) {
                    y = (this.frame.currentFrame.wait * 0.375f) * (this.frame.currentFrame.dvy * -1);
                }

                if (this.frame.currentFrame.dvy == STOP_MOVEMENT) {
                    y = 0f;
                }

                var velocity = new Vector3(x, y, z);

                if (velocity != Vector3.zero) {
                    rigidbody.AddForce(velocity, ForceMode.Impulse);
                    this.physicsOneTimePerFrame = false;
                }
            }
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
}
