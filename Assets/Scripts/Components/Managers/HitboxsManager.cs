using Components.Handlers;
using TMPro;
using UnityEngine;
using Util;

namespace Components.Managers {
    public class HitboxsManager : MonoBehaviour {
        [SerializeField]
        private TeamEnum team;
        [SerializeField]
        private ObjectHandler owner;
        private ObjectHandler parentOwner;

        [SerializeField]
        private GameObject mainHitbox;

        [SerializeField]
        private GameObject additionalHitbox1;

        [SerializeField]
        private GameObject additionalHitbox2;

        [SerializeField]
        private TextMeshPro countText;

        public int hitsCount = 0;
        public float countResetHits;
        public float intervalResetHits;
        private bool flagToDisableAsyncHits = false;

        void Start() {
            team = owner.team;

            if (owner.transform.parent != null) {
                parentOwner = owner.transform.parent.GetComponentInParent <ObjectHandler>();
            }
        }

        void Update() {
            EnableDisableInteractions();
            SetupResetCount();
        }

        public ObjectHandler GetObjectHandler() {
            return this.owner;
        }

        public void SetupCountHits() {
            if (!owner.hasAttacked) {
                flagToDisableAsyncHits = false;
            }

            if (!flagToDisableAsyncHits) {
                countResetHits = intervalResetHits;
                hitsCount++;

                if (hitsCount > 1) {
                    countText.transform.parent.gameObject.SetActive(true);
                }

                countText.text = hitsCount.ToString();

                flagToDisableAsyncHits = true;
            }
        }

        private void SetupResetCount() {
            if (countResetHits >= 0f) {
                countResetHits -= Time.fixedDeltaTime;
            } else {
                countResetHits = 0f;
                hitsCount = 0;
                countText.transform.parent.gameObject.SetActive(false);
                flagToDisableAsyncHits = false;
            }
        }

        private void EnableDisableInteractions() {
            if (owner.actualFrame.interactions.Length == 3) {
                mainHitbox.gameObject.SetActive(true);
                additionalHitbox1.gameObject.SetActive(true);
                additionalHitbox2.gameObject.SetActive(true);
            } else if (owner.actualFrame.interactions.Length == 2) {
                mainHitbox.gameObject.SetActive(true);
                additionalHitbox1.gameObject.SetActive(true);
                DisableHitbox(additionalHitbox2);
            } else if (owner.actualFrame.interactions.Length == 1) {
                mainHitbox.gameObject.SetActive(true);
                DisableHitbox(additionalHitbox1, additionalHitbox2);
            } else if (owner.actualFrame.interactions.Length > 3) {
                ExceptionThrowUtil.LimitReached();
            } else {
                DisableHitbox(mainHitbox, additionalHitbox1, additionalHitbox2);
            }
        }

        private void DisableHitbox(params GameObject[] hitboxes) {
            foreach (GameObject hitbox in hitboxes) {
                hitbox.transform.position = Vector3.zero;
                hitbox.transform.localScale = Vector3.zero;
                hitbox.SetActive(false);
            }
        }
    }
}