using Components.Handlers;
using Model.Type;
using UnityEngine;

namespace Components.Scripts {
    public class GroundDetectionScript : MonoBehaviour {

        [SerializeField]
        private ObjectHandler objectHandler;

        private string TAG_TO_DETECT = "Ground";

        // Update is called once per frame
        void Update() {
            var customGroundDetectionPosition = objectHandler.actualFrame.physic.customGroundDetectionPosition;
            if (customGroundDetectionPosition != null) {
                transform.localPosition = customGroundDetectionPosition;
            }

            var customGroundDetectionSize = objectHandler.actualFrame.physic.customGroundDetectionSize;
            if (customGroundDetectionSize != null) {
                transform.localScale = customGroundDetectionSize;
            }
        }

        void OnTriggerStay(Collider hit) {
            if (objectHandler.objectType.Equals(ObjectEnum.CHARACTER)) {
                if (hit.tag.Equals(TAG_TO_DETECT)) {
                    objectHandler.onGround = true;
                    objectHandler.constantGravity = 0f;
                }
            }
        }

        void OnTriggerExit(Collider hit) {
            if (hit.tag.Equals(TAG_TO_DETECT)) {
                objectHandler.onGround = false;
                objectHandler.lockRightForce = objectHandler.isFacingRight;
            }
        }
    }
}

