using System.Collections.Generic;
using UnityEngine;

public class ObjectPointController : MonoBehaviour {
    public FrameController frame;

    public List<ObjectPointData> opoints;

    public bool opointOneTimePerFrame;

    public int currentFrameId;

    void Start() {
        this.opointOneTimePerFrame = true;
        this.currentFrameId = -1;
    }

    void Update() {
        if (this.currentFrameId != this.frame.currentFrame.id) {
            this.opointOneTimePerFrame = true;
            this.currentFrameId = this.frame.currentFrame.id;
        }

        if (this.opointOneTimePerFrame) {
            if (frame.currentFrame.opoints != null && frame.currentFrame.opoints.Count > 0) {
                opoints = frame.currentFrame.opoints;
                return;
            }

            if (opoints != null) {
                foreach (ObjectPointData opoint in opoints) {
                    var opointSpawn = Resources.Load<GameObject>(opoint.object_id);
                    var spawnFrame = opointSpawn.GetComponent<FrameController>();
                    var spawnPhysics = opointSpawn.GetComponent<PhysicController>();

                    spawnFrame.ownerId = this.frame.selfId;
                    spawnFrame.team = this.frame.team;

                    float x = transform.position.x + PositionUtil.FacingByX(opoint.x, frame.facingRight);
                    float y = transform.position.y + opoint.y;
                    float z = transform.position.z + opoint.z;

                    opointSpawn.transform.position = new Vector3(x, y, z);
                    spawnFrame.summonAction = opoint.action;

                    spawnPhysics.externForce = new Vector3(opoint.dvx, opoint.dvy, opoint.dvz);
                    spawnPhysics.isExternForce = true;
                    spawnPhysics.enable_dvz_invocation = opoint.enable_dvz_invocation;
                    if (spawnPhysics.enable_dvz_invocation) {
                        spawnPhysics.lockInputDirection = this.frame.inputDirection;
                    }

                    spawnFrame.facingRight = frame.facingRight ? opoint.facing : !opoint.facing;

                    opoint.quantity = opoint.quantity == null || opoint.quantity <= 0 ? 0 : opoint.quantity;
                    opoint.quantity = opoint.quantity > 5 ? 5 : opoint.quantity;

                    for (int i = 0; i < opoint.quantity; i++) {

                        var actualPos = opointSpawn.transform.position;
                        switch (i + 1) {
                            case 1:
                                Instantiate(opointSpawn);
                                break;
                            case 2:
                                Instantiate(opointSpawn, new Vector3(actualPos.x, actualPos.y, actualPos.z + opoint.z_division_per_quantity), Quaternion.identity);
                                Instantiate(opointSpawn, new Vector3(actualPos.x, actualPos.y, actualPos.z - opoint.z_division_per_quantity), Quaternion.identity);
                                break;
                            case 3:
                                Instantiate(opointSpawn);
                                Instantiate(opointSpawn, new Vector3(actualPos.x, actualPos.y, actualPos.z + opoint.z_division_per_quantity), Quaternion.identity);
                                Instantiate(opointSpawn, new Vector3(actualPos.x, actualPos.y, actualPos.z - opoint.z_division_per_quantity), Quaternion.identity);
                                break;
                            case 4:
                                Instantiate(opointSpawn, new Vector3(actualPos.x, actualPos.y, actualPos.z + opoint.z_division_per_quantity), Quaternion.identity);
                                Instantiate(opointSpawn, new Vector3(actualPos.x, actualPos.y, actualPos.z - opoint.z_division_per_quantity), Quaternion.identity);
                                Instantiate(opointSpawn, new Vector3(actualPos.x, actualPos.y, actualPos.z + (opoint.z_division_per_quantity * 2)), Quaternion.identity);
                                Instantiate(opointSpawn, new Vector3(actualPos.x, actualPos.y, actualPos.z - (opoint.z_division_per_quantity * 2)), Quaternion.identity);
                                break;
                            default:
                                Instantiate(opointSpawn);
                                Instantiate(opointSpawn, new Vector3(actualPos.x, actualPos.y, actualPos.z + opoint.z_division_per_quantity), Quaternion.identity);
                                Instantiate(opointSpawn, new Vector3(actualPos.x, actualPos.y, actualPos.z - opoint.z_division_per_quantity), Quaternion.identity);
                                Instantiate(opointSpawn, new Vector3(actualPos.x, actualPos.y, actualPos.z + (opoint.z_division_per_quantity * 2)), Quaternion.identity);
                                Instantiate(opointSpawn, new Vector3(actualPos.x, actualPos.y, actualPos.z - (opoint.z_division_per_quantity * 2)), Quaternion.identity);
                                break;
                        }
                        this.opointOneTimePerFrame = false;
                        this.opoints = null;
                    }
                }
            }
        }
    }
}
