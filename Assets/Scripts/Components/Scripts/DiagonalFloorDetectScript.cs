using Components.Handlers;
using UnityEngine;

namespace Components.Scripts {
    public class DiagonalFloorDetectScript : MonoBehaviour {
        public ObjectHandler objectHandler;

        void OnTriggerStay(Collider hit) {
            if (hit.tag.Equals("Ground")) {
                objectHandler.onGround = true;
                objectHandler.constantGravity = 0f;
            }
        }

        void OnTriggerExit(Collider hit) {
            if (hit.tag.Equals("Ground")) {
                objectHandler.onGround = false;
                objectHandler.lockRightForce = objectHandler.isFacingRight;
            }
        }
    }
}