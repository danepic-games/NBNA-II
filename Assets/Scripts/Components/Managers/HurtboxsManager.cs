using Back.Model.Type;
using Components.Handlers;
using Model;
using Model.Type;
using TMPro;
using UnityEngine;
using Util;
using Models;
using Utils;

namespace Components.Managers {
    public class HurtboxsManager : MonoBehaviour {
        [SerializeField]
        private ObjectHandler owner;

        [SerializeField]
        private HurtboxSpecifiedHandler mainHurtbox;
        [SerializeField]
        private HurtboxSpecifiedHandler additionalHurtBox1;
        [SerializeField]
        private HurtboxSpecifiedHandler additionalHurtBox2;

        [SerializeField]
        private GameObject hit;
        [SerializeField]
        private GameObject hit2;
        [SerializeField]
        private GameObject hit3;
        [SerializeField]
        private GameObject weaponHit;
        [SerializeField]
        private GameObject weaponHit2;
        [SerializeField]
        private GameObject weaponHit3;
        [SerializeField]
        private GameObject defensePush;

        public float damageRestTimer;

        [SerializeField]
        private TextMeshPro countText;

        // Start is called before the first frame update
        void Start() {
            owner = gameObject.GetComponentInParent<ObjectHandler>();
        }

        void Update() {
            EnableDisableBodies();

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

        //Chamado pelo hurtbox especifico que gatilhou algum evento
        public void ObjectCollision(HitboxSpecifiedHandler enemyOwner) {
            if (!owner.team.Equals(enemyOwner.owner.team) || enemyOwner.owner.team.Equals(TeamEnum.INDEPENDENT)) {

                Interaction itr = enemyOwner.interaction; //talvez ser statico para poder saber se o dano foi duplicado em multilplos hurtboxes

                SpriteRenderer enemySpriteRenderer = enemyOwner.GetComponentInParent<SpriteRenderer>();

                if (itr != null && enemySpriteRenderer != null) {
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

        private void SetupNormalInjured(HitboxSpecifiedHandler enemyOwner, Interaction itr, float dvx) {
            if (itr.kind.Equals(InteractionKindEnum.NORMAL_DAMAGE) || itr.kind.Equals(InteractionKindEnum.FLOOR)) {
                if (owner.externalItr != null && owner.externalItr.origin != null && owner.externalItr.origin.name.Equals(enemyOwner.owner.gameObject.name)) {
                    owner.SetSameExternalItr(true);
                } else {
                    owner.SetSameExternalItr(false);
                }

                owner.externalItr = itr;
                owner.externalItr.force.x = dvx;
                owner.enableInjured = true;
                owner.actualFrame.core.isInjured = true;

                InvokeContactEffect(itr);
            }
        }

        private void SetupDefendingInjured(HitboxSpecifiedHandler enemyOwner, Interaction itr, float dvx, string damageAnimationType) {
            if (itr.kind.Equals(InteractionKindEnum.NORMAL_DAMAGE) || itr.kind.Equals(InteractionKindEnum.FLOOR)) {
                if (owner.externalItr != null && owner.externalItr.origin.name.Equals(enemyOwner.owner.gameObject.name)) {
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
                owner.actualFrame.core.isInjured = true;

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

        private void SetupStaticInjured(HitboxSpecifiedHandler enemyOwner, Interaction itr, float dvx) {
            if (itr.kind.Equals(InteractionKindEnum.NORMAL_DAMAGE) || itr.kind.Equals(InteractionKindEnum.FLOOR) || itr.kind.Equals(InteractionKindEnum.DIRECT_STATIC_MAP_OBJ)) {
                if (owner.externalItr != null && owner.externalItr.origin != null && owner.externalItr.origin.name.Equals(enemyOwner.owner.gameObject.name)) {
                    owner.SetSameExternalItr(true);
                } else {
                    owner.SetSameExternalItr(false);
                }

                owner.externalItr = itr;
                owner.externalItr.force = Vector3.zero;
                owner.enableInjured = true;
                owner.actualFrame.core.isInjured = true;

                InvokeContactEffect(itr);
            }
        }

        private void EnableDisableBodies() {
            if (owner.actualFrame.bodies.Length == 3) {
                mainHurtbox.gameObject.SetActive(true);
                additionalHurtBox1.gameObject.SetActive(true);
                additionalHurtBox2.gameObject.SetActive(true);
            } else if (owner.actualFrame.bodies.Length == 2) {
                mainHurtbox.gameObject.SetActive(true);
                additionalHurtBox1.gameObject.SetActive(true);
                DisableHurtboxes(additionalHurtBox2);
            } else if (owner.actualFrame.bodies.Length == 1) {
                mainHurtbox.gameObject.SetActive(true);
                DisableHurtboxes(additionalHurtBox1, additionalHurtBox2);
            } else if (owner.actualFrame.bodies.Length > 3) {
                ExceptionThrowUtil.LimitReached();
            } else {
                DisableHurtboxes(mainHurtbox, additionalHurtBox1, additionalHurtBox2);
            }
        }

        private void DisableHurtboxes(params HurtboxSpecifiedHandler[] hurtboxes) {
            foreach (HurtboxSpecifiedHandler hurtboxHandler in hurtboxes) {
                var hurtbox = hurtboxHandler.gameObject;
                hurtbox.transform.position = Vector3.zero;
                hurtbox.transform.localScale = Vector3.zero;
                hurtbox.SetActive(false);
            }
        }
    }
}