using Model;
using UnityEngine;
using Components.Managers;
using Components.Handlers;
using Components.Controllers;

namespace Components.Handlers {
    public class HurtboxSpecifiedHandler : MonoBehaviour {
        [SerializeField]
        private ObjectHandler owner;
        [SerializeField]
        private HurtboxsManager manager;

        public int bodyNumber;
        private bool isMainBody;

        void Start() {
            isMainBody = bodyNumber == 0;
        }

        void Update() {
            SetupBody();
        }

        void OnTriggerStay(Collider enemy) {
            if (enemy.tag.Equals("HitBox")) {
                HitboxSpecifiedHandler enemyOwner = enemy.gameObject.GetComponent<HitboxSpecifiedHandler>();
                ObjectHandler enemyObject = enemyOwner.owner;

                if (owner != null) {
                    if (enemyObject != null) {
                        //Objeto ou seu dono nao ser atingido por ele mesmo
                        if (!enemyObject.name.Equals(owner.name) || (!enemyObject.owner != null && !enemyObject.owner.name.Equals(owner.name))) {
                            manager.ObjectCollision(enemyOwner);

                            //Objetos filhos nao poderem atingir seu pai ou dono
                        } else if (!enemyObject.name.Equals(owner.name) || (!enemyObject.owner != null && !enemyObject.owner.name.Equals(owner.name))) {
                            Debug.Log(enemy.transform.parent.parent.name + " > " + owner.gameObject.name);
                            manager.ObjectCollision(enemyOwner);

                            //Objeto pai ou dono nao poder atingir seus filhos
                        } else if (!owner.name.Equals(enemyObject.name.Equals(owner.name)) || (!enemyObject.owner != null && !owner.name.Equals(enemyObject.owner.name.Equals(owner.name)))) {
                            Debug.Log(enemy.transform.parent.parent.name + " > " + owner.gameObject.name);
                            manager.ObjectCollision(enemyOwner);
                        }
                    }
                }

            } else if (enemy.tag.Equals("HurtBox")) {
                HurtboxsManager enemyOwner = enemy.gameObject.GetComponentInParent<HurtboxsManager>();
                ObjectHandler enemyObject = enemyOwner.GetObjectHandler();

                if (owner != null) {
                    if (enemyObject != null) {
                        //Objeto ou seu dono nao ser atingido por ele mesmo
                        if (!enemyObject.name.Equals(owner.name) || (!enemyObject.owner != null && !enemyObject.owner.name.Equals(owner.name))) {
                            owner.hasTouchedHurtBox = true;

                            //Objetos filhos nao poderem atingir seu pai ou dono
                        } else if (!enemyObject.name.Equals(owner.name) || (!enemyObject.owner != null && !enemyObject.owner.name.Equals(owner.name))) {
                            owner.hasTouchedHurtBox = true;

                            //Objeto pai ou dono nao poder atingir seus filhos
                        } else if (!owner.name.Equals(enemyObject.name.Equals(owner.name)) || (!enemyObject.owner != null && !owner.name.Equals(enemyObject.owner.name.Equals(owner.name)))) {
                            owner.hasTouchedHurtBox = true;
                        }
                    }
                }
            }
        }

        private void SetupBody() {
            if (owner.actualFrame != null && owner.actualFrame.bodies != null && owner.actualFrame.bodies.Length > 0) {
                Body body = owner.actualFrame.bodies[bodyNumber];

                if (!owner.isFacingRight) {
                    if (body.Equals(owner.actualFrame.bodies[bodyNumber])) {
                        transform.localPosition = new Vector3(-body.position.x, body.position.y, body.position.z);
                        transform.localScale = new Vector3(body.size.x, body.size.y, body.size.z);
                        if (isMainBody) {
                            owner.boxCollider.center = transform.localPosition;
                            owner.boxCollider.size = transform.localScale;
                        }
                    }


                } else {
                    if (body.Equals(owner.actualFrame.bodies[bodyNumber])) {
                        transform.localPosition = new Vector3(body.position.x, body.position.y, body.position.z);
                        transform.localScale = new Vector3(body.size.x, body.size.y, body.size.z);
                        if (isMainBody) {
                            owner.boxCollider.center = transform.localPosition;
                            owner.boxCollider.size = transform.localScale;
                        }
                    }
                }
            }
        }
    }
}