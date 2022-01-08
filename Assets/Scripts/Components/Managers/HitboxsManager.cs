using Model;
using TMPro;
using UnityEngine;
using Components.Handlers;
using Components.Controllers;

namespace Components.Managers {
    public class HitboxsManager : MonoBehaviour {
        public TeamEnum team;
        private ObjectHandler owner;
        private ObjectHandler parentOwner;

        public GameObject hitBox1;
        public GameObject hitBox2;
        public GameObject hitBox3;

        public TextMeshPro countText;

        public int hitsCount = 0;
        public float countResetHits;
        public float intervalResetHits;
        private bool flagToDisableAsyncHits = false;

        void Start() {
            owner = gameObject.GetComponentInParent <ObjectHandler>();
            if (owner != null) {
                team = owner.team;

                if (owner.transform.parent != null) {
                    parentOwner = owner.transform.parent.GetComponentInParent <ObjectHandler>();
                }
            }
        }

        void Update() {
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
    }
}