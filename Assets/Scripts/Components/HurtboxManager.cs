using Back.Model.Type;
using Model;
using Model.Type;
using TMPro;
using UnityEngine;

namespace Components {
    public class HurtboxManager : MonoBehaviour {
        private ObjectHandler owner;
        public GameObject hit;
        public GameObject hit2;
        public GameObject hit3;
        public GameObject weaponHit;
        public GameObject weaponHit2;
        public GameObject weaponHit3;
        public GameObject defensePush;

        public float damageRestTimer;

        public TextMeshPro countText;

        // Start is called before the first frame update
        void Start() {
            owner = gameObject.GetComponentInParent<ObjectHandler>();
        }

        void Update() {
            SetupBodies();
            if (owner.externalItr != null && owner.externalItr.damageRestTU > 0 && damageRestTimer > 0) {
                if (owner.externalItr.resetDamageRestTime) {
                    damageRestTimer = 0;
                    owner.externalItr.resetDamageRestTime = false;
                } else {
                    damageRestTimer -= Time.fixedDeltaTime;
                }
            } else {
                damageRestTimer = 0;
                owner.SetSameExternalItr(false);
            }
        }

        private void SetupBodies() {
            if (owner.actualFrame != null && owner.actualFrame.bodies != null && owner.actualFrame.bodies.Length > 0) {
                Body mainBody = owner.actualFrame.bodies[0];
                owner.mainHurtbox.localPosition = mainBody.position;
                owner.mainHurtbox.localScale = mainBody.size;

                if (owner.mainHurtbox != null) {
                    if (owner.mainHurtbox.gameObject.activeSelf) {
                        SetupBody(owner.mainHurtbox.transform);
                    }
                }

                if (owner.additionalHurtBox1 != null) {
                    if (owner.additionalHurtBox1.gameObject.activeSelf) {
                        SetupBody(owner.additionalHurtBox1.transform);
                    }
                }

                if (owner.additionalHurtBox2 != null) {
                    if (owner.additionalHurtBox2.gameObject.activeSelf) {
                        SetupBody(owner.additionalHurtBox2.transform);
                    }
                }
            }
        }

        public void ObjectCollision(HitboxManager enemyOwner) {
            if (!owner.team.Equals(enemyOwner.team) || enemyOwner.team.Equals(TeamEnum.INDEPENDENT)) {

                Interaction itr = enemyOwner.GetHigherPriorityInteraction(); //talvez ser statico para poder saber se o dano foi duplicado em multilplos hurtboxes

                SpriteRenderer enemySpriteRenderer = enemyOwner.GetComponentInParent<SpriteRenderer>();

                if (itr != null && itr.active && enemySpriteRenderer != null) {
                    if (damageRestTimer.Equals(0)) {

                        float dvx = GetTrueDvxOfDamage(enemySpriteRenderer, itr);

                        switch (owner.actualFrame.core.hurtBoxType) {
                            case HurtboxEnum.NORMAL:
                                this.SetupNormalInjured(enemyOwner, itr, dvx);
                                break;
                            case HurtboxEnum.DEFENDING:
                                if (owner.externalItr != null &&
                                (owner.externalItr.level.Equals(LevelDamageEnum.LEVEL2) || owner.externalItr.level.Equals(LevelDamageEnum.LEVEL3)
                                || owner.externalItr.effect.Equals(InteractionEffectEnum.NONE))) {
                                    this.SetupNormalInjured(enemyOwner, itr, dvx);

                                } else {
                                    this.SetupDefendingInjured(enemyOwner, itr, dvx, "DefendingMovementDebug");
                                }

                                break;
                            case HurtboxEnum.DEFENDING_IN_JUMP:
                                this.SetupDefendingInjured(enemyOwner, itr, dvx, "JumpDefenseMovementDebug");
                                break;
                            case HurtboxEnum.STATIC:
                                this.SetupStaticInjured(enemyOwner, itr, dvx);
                                break;
                        }
                    }
                }
            }
        }

        private float GetTrueDvxOfDamage(SpriteRenderer enemySpriteRenderer, Interaction itr) {
            float dvx = 0f;
            bool enemyIsFacingRight = !enemySpriteRenderer.flipX;
            if (!itr.inverseForceDvx)
            {
                if (enemyIsFacingRight && owner.isFacingRight)
                {
                    dvx = Mathf.Abs(itr.force.x);
                }
                else if (enemyIsFacingRight && !owner.isFacingRight)
                {
                    dvx = Mathf.Abs(itr.force.x);
                }
                else if (!enemyIsFacingRight && owner.isFacingRight)
                {
                    dvx = -itr.force.x;
                }
                else if (!enemyIsFacingRight && !owner.isFacingRight)
                {
                    dvx = -itr.force.x;
                }
            }
            else
            {
                if (enemyIsFacingRight && owner.isFacingRight)
                {
                    dvx = -Mathf.Abs(itr.force.x);
                }
                else if (enemyIsFacingRight && !owner.isFacingRight)
                {
                    dvx = -Mathf.Abs(itr.force.x);
                }
                else if (!enemyIsFacingRight && owner.isFacingRight)
                {
                    dvx = Mathf.Abs(itr.force.x);
                }
                else if (!enemyIsFacingRight && !owner.isFacingRight)
                {
                    dvx = Mathf.Abs(itr.force.x);
                }
            }

            return owner.isFacingRight ? dvx : -dvx;
        }

        private Interaction BuildInteractionWithEnemyItr(HitboxManager enemyOwner) {
            Interaction itr = new Interaction();
            itr.kind = enemyOwner.interaction.kind;
            itr.force.x = enemyOwner.interaction.force.x;
            itr.inverseForceDvx = enemyOwner.interaction.inverseForceDvx;
            itr.force.y = enemyOwner.interaction.force.y;
            itr.force.z = enemyOwner.interaction.force.z;
            itr.damageRestTU = enemyOwner.interaction.damageRestTU;
            itr.resetDamageRestTime = enemyOwner.interaction.resetDamageRestTime;
            itr.affectedAnimation = enemyOwner.interaction.affectedAnimation;
            itr.nextAnimation = enemyOwner.interaction.nextAnimation;
            itr.injury = enemyOwner.interaction.injury;
            itr.effect = enemyOwner.interaction.effect;
            itr.level = enemyOwner.interaction.level;
            itr.origin = enemyOwner.transform.parent.gameObject;
            return itr;
        }

        private InteractionArea BuildInteractionAreaWithEnemyItr(MainHitboxsHandler enemyOwner) {
            InteractionArea itr = new InteractionArea();
            itr.position.x = enemyOwner.interactionArea.position.x;
            itr.position.y = enemyOwner.interactionArea.position.y;
            itr.position.z = enemyOwner.interactionArea.position.z;
            itr.size.x = enemyOwner.interactionArea.size.x;
            itr.size.y = enemyOwner.interactionArea.size.y;
            itr.size.z = enemyOwner.interactionArea.size.z;
            return itr;
        }

        private void InvokeContactEffect(Interaction itr, float x, float y, float z) {
            ContactEffect(itr, x, y, z);
        }

        private void InvokeContactEffect(Interaction itr) {
            float x = owner.transform.position.x;
            float y = owner.transform.position.y + (owner.transform.localScale.y / 1.75f);
            float z = owner.transform.position.z - (owner.transform.localScale.z / 2);
            ContactEffect(itr, x, y, z);
        }

        private void ContactEffect(Interaction itr, float x, float y, float z) {
            switch (itr.effect) {
                case InteractionEffectEnum.SUPER_ATTACK:
                case InteractionEffectEnum.NORMAL_ATTACK:
                    Instantiate(hit, new Vector3(x, y, z), Quaternion.identity);

                    Instantiate(hit2, new Vector3(x, y, z), Quaternion.identity);

                    Instantiate(hit3, new Vector3(x, y, z), Quaternion.identity);
                    break;
                case InteractionEffectEnum.NORMAL_WEAPON:
                    Instantiate(weaponHit, new Vector3(x, y, z), Quaternion.identity);

                    Instantiate(weaponHit2, new Vector3(x, y, z), Quaternion.identity);

                    Instantiate(weaponHit3, new Vector3(x, y, z), Quaternion.identity);
                    break;
            }

            countText.text = itr.injury.ToString();
            Instantiate(countText.gameObject, new Vector3(x, y, z), Quaternion.identity);
        }

        private void SetupNormalInjured(HitboxManager enemyOwner, Interaction itr, float dvx) {
            if (itr.kind.Equals(InteractionKindEnum.NORMAL_DAMAGE) || itr.kind.Equals(InteractionKindEnum.FLOOR)) {
                if (owner.externalItr != null && owner.externalItr.origin != null && owner.externalItr.origin.Equals(enemyOwner.transform.parent.gameObject)) {
                    owner.SetSameExternalItr(true);
                } else {
                    owner.SetSameExternalItr(false);
                }

                owner.externalItr = itr;
                owner.externalItr.force.x = dvx;
                owner.enableInjured = true;
                owner.isInjured = true;

                InvokeContactEffect(itr);
            }
        }

        private void SetupDefendingInjured(HitboxManager enemyOwner, Interaction itr, float dvx, string damageAnimationType) {
            if (itr.kind.Equals(InteractionKindEnum.NORMAL_DAMAGE) || itr.kind.Equals(InteractionKindEnum.FLOOR)) {
                if (owner.externalItr != null && owner.externalItr.origin.Equals(enemyOwner.transform.parent.gameObject)) {
                    owner.SetSameExternalItr(true);
                } else {
                    owner.SetSameExternalItr(false);
                }

                owner.externalItr = itr;

                owner.externalItr.force.x = dvx;
                owner.externalItr.injury = 0;
                owner.externalItr.force.y = 0f;

                owner.externalItr.affectedAnimation = damageAnimationType;
                owner.enableInjured = true;
                owner.isInjured = true;

                float x = owner.transform.position.x;
                float y = owner.transform.position.y + (owner.transform.localScale.y / 1.75f);
                float z = owner.transform.position.z - (owner.transform.localScale.z / 2);

                InvokeContactEffect(itr, x, y, z);

                Instantiate(defensePush, new Vector3(x, y, z), Quaternion.identity);
            }
        }

        public ObjectHandler GetObjectHandler() {
            return this.owner;
        }

        private void SetupStaticInjured(HitboxManager enemyOwner, Interaction itr, float dvx) {
            if (itr.kind.Equals(InteractionKindEnum.NORMAL_DAMAGE) || itr.kind.Equals(InteractionKindEnum.FLOOR) || itr.kind.Equals(InteractionKindEnum.DIRECT_STATIC_MAP_OBJ)) {
                if (owner.externalItr != null && owner.externalItr.origin != null && owner.externalItr.origin.Equals(enemyOwner.transform.parent.gameObject)) {
                    owner.SetSameExternalItr(true);
                } else {
                    owner.SetSameExternalItr(false);
                }

                owner.externalItr = itr;
                owner.externalItr.force = Vector3.zero;
                owner.enableInjured = true;
                owner.isInjured = true;

                InvokeContactEffect(itr);
            }
        }

        private void SetupBody(Transform body) {
            if (!owner.isFacingRight) {
                if (bdy.Equals(owner.actualFrame.bodies[0])) {
                    hurtBox1.transform.localPosition = new Vector3(-bdy.position.x, bdy.position.y, bdy.position.z);
                    hurtBox1.transform.localScale = new Vector3(bdy.size.x, bdy.size.y, bdy.size.z);
                    owner.boxCollider.center = hurtBox1.transform.localPosition;
                    owner.boxCollider.size = hurtBox1.transform.localScale;

                } else if (bdy.Equals(owner.actualFrame.bodies[1])) {
                    hurtBox2.transform.localPosition = new Vector3(-bdy.position.x, bdy.position.y, bdy.position.z);
                    hurtBox2.transform.localScale = new Vector3(bdy.size.x, bdy.size.y, bdy.size.z);

                } else if (bdy.Equals(owner.actualFrame.bodies[2])) {
                    hurtBox3.transform.localPosition = new Vector3(-bdy.position.x, bdy.position.y, bdy.position.z);
                    hurtBox3.transform.localScale = new Vector3(bdy.size.x, bdy.size.y, bdy.size.z);
                }


            } else {

                if (bdy.Equals(owner.actualFrame.bodies[0])) {
                    hurtBox1.transform.localPosition = new Vector3(bdy.position.x, bdy.position.y, bdy.position.z);
                    hurtBox1.transform.localScale = new Vector3(bdy.size.x, bdy.size.y, bdy.size.z);
                    owner.boxCollider.center = hurtBox1.transform.localPosition;
                    owner.boxCollider.size = hurtBox1.transform.localScale;

                } else if (bdy.Equals(owner.actualFrame.bodies[1])) {
                    hurtBox2.transform.localPosition = new Vector3(bdy.position.x, bdy.position.y, bdy.position.z);
                    hurtBox2.transform.localScale = new Vector3(bdy.size.x, bdy.size.y, bdy.size.z);

                } else if (bdy.Equals(owner.actualFrame.bodies[2])) {
                    hurtBox3.transform.localPosition = new Vector3(bdy.position.x, bdy.position.y, bdy.position.z);
                    hurtBox3.transform.localScale = new Vector3(bdy.size.x, bdy.size.y, bdy.size.z);

                }
            }
        }
    }
}