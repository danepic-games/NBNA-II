using Model;
using UnityEngine;
using Components.Managers;
using Components.Handlers;
using Components.Controllers;

namespace Components.Handlers {
    public class HitboxSpecifiedHandler : MonoBehaviour {
        public ObjectHandler owner;
        [SerializeField]
        private HitboxsManager controller;

        public Interaction interaction;

        public int interactionNumber;

        void Update() {
            SetupInteraction();
        }

        void OnTriggerStay(Collider other) {
            if (other.tag.Equals("HurtBox")) {
                if (owner != null && owner.actualFrame.interactions.Length > 0 && !owner.transform.name.Equals(other.transform.parent.name)) {
                    owner.isAttacking = true;
                    controller.SetupCountHits();
                }
            }
        }

        private void SetupInteraction() {
            if (owner.actualFrame.interactions.Length > 0 && gameObject.activeSelf) {
                if (owner.actualFrame.interactions[interactionNumber] != null) {
                    Interaction itr = owner.actualFrame.interactions[interactionNumber];
                    if (!owner.isFacingRight) {
                        transform.localPosition = new Vector3(-itr.position.x, itr.position.y, itr.position.z);
                        transform.localScale = new Vector3(itr.size.x, itr.size.y, itr.size.z);

                    } else {
                        transform.localPosition = new Vector3(itr.position.x, itr.position.y, itr.position.z);
                        transform.localScale = new Vector3(itr.size.x, itr.size.y, itr.size.z);
                    }
                }
            }
        }
    }
}