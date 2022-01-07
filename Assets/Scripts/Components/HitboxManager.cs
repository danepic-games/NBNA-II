using Model;
using TMPro;
using UnityEngine;

namespace Components {
    public class HitboxManager : MonoBehaviour {
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
            SetupInteractions();

            SetupResetCount();
        }

        void SetupInteractions() {
            foreach (Interaction itr in owner.actualFrame.interactions) {
                if (itr != null && itr.gameObject.activeSelf) {
                    if (itr != null) {
                        if (!owner.isFacingRight) {
                            if (itr.Equals(owner.properties.interactionsArea[0])) {
                                hitBox1.transform.localPosition = new Vector3(-itr.position.x, itr.position.y, itr.position.z);
                                hitBox1.transform.localScale = new Vector3(itr.size.x, itr.size.y, itr.size.z);
                                interactionArea = itr;

                            } else if (itr.Equals(owner.properties.interactionsArea[1])) {
                                hitBox2.transform.localPosition = new Vector3(-itr.position.x, itr.position.y, itr.position.z);
                                hitBox2.transform.localScale = new Vector3(itr.size.x, itr.size.y, itr.size.z);

                            } else if (itr.Equals(owner.properties.interactionsArea[2])) {
                                hitBox3.transform.localPosition = new Vector3(-itr.position.x, itr.position.y, itr.position.z);
                                hitBox3.transform.localScale = new Vector3(itr.size.x, itr.size.y, itr.size.z);
                            }

                        } else {
                            if (itr.Equals(owner.properties.interactionsArea[0])) {
                                hitBox1.transform.localPosition = new Vector3(itr.position.x, itr.position.y, itr.position.z);
                                hitBox1.transform.localScale = new Vector3(itr.size.x, itr.size.y, itr.size.z);
                                interactionArea = itr;

                            } else if (itr.Equals(owner.properties.interactionsArea[1])) {
                                hitBox2.transform.localPosition = new Vector3(itr.position.x, itr.position.y, itr.position.z);
                                hitBox2.transform.localScale = new Vector3(itr.size.x, itr.size.y, itr.size.z);

                            } else if (itr.Equals(owner.properties.interactionsArea[2])) {
                                hitBox3.transform.localPosition = new Vector3(itr.position.x, itr.position.y, itr.position.z);
                                hitBox3.transform.localScale = new Vector3(itr.size.x, itr.size.y, itr.size.z);
                            }
                        }
                    }
                }
            }
        }

        public ObjectHandler GetObjectHandler() {
            return this.owner;
        }

        public Interaction GetHigherPriorityInteraction() {
            if (owner.mainHitbox != null) {
                return owner.actualFrame.interactions[0];
            }
            if (owner.additionalHitbox1 != null) {
                return owner.actualFrame.interactions[1];
            }
            if (owner.additionalHitbox2 != null) {
                return owner.actualFrame.interactions[2];
            }
            return null;
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