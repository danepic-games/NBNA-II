using Components.Handlers;
using Model.Type;
using UnityEngine;

namespace Components.Scripts {
    public class GroundDetectionScript : MonoBehaviour {

        [SerializeField]
        private ObjectHandler objectHandler;

        [SerializeField]
        private BoxCollider boxCollider;

        private string TAG_TO_DETECT = "Ground";

        // Update is called once per frame
        void Update() {
            var customGroundDetectionPosition = objectHandler.actualFrame.physic.customGroundDetectionPosition;
            if (customGroundDetectionPosition != null) {
                if (objectHandler.isFacingRight) {
                    transform.localPosition = customGroundDetectionPosition;
                } else {
                    transform.localPosition = new Vector3(-customGroundDetectionPosition.x, customGroundDetectionPosition.y,
                            customGroundDetectionPosition.z);
                }
                boxCollider.center = transform.localPosition;
            }

            var customGroundDetectionSize = objectHandler.actualFrame.physic.customGroundDetectionSize;
            if (customGroundDetectionSize != null) {
                transform.localScale = customGroundDetectionSize;
                boxCollider.size = transform.localScale;
            }
        }

        void OnTriggerStay(Collider hit) {
            if (objectHandler.objectType.Equals(ObjectEnum.CHARACTER)) {
                if (hit.tag.Equals(TAG_TO_DETECT)) {
                    objectHandler.onGround = true;
                    objectHandler.constantGravity = 0f;
                    objectHandler.InvokeCheckEvents();
                }
            }
        }

        void OnTriggerExit(Collider hit) {
            if (objectHandler.objectType.Equals(ObjectEnum.CHARACTER)) {
                if (hit.tag.Equals(TAG_TO_DETECT)) {
                    objectHandler.onGround = false;
                    objectHandler.lockRightForce = objectHandler.isFacingRight;
                }
            }
        }
    }
}

