using UnityEngine;

public class HurtboxController : MonoBehaviour {

    public int bodyNumber;
    public HurtboxesController hurtboxes;
    public BodyData bdy;
    public BoxCollider boxCollider;
    public SpriteRenderer spriteRenderer;
    public FrameController frame;
    public MeshRenderer meshRenderer;

    public GameObject normalHit;
    public GameObject swordHit;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (frame.currentFrame.bodys != null && frame.currentFrame.bodys.Count >= bodyNumber) {
            boxCollider.enabled = true;
            meshRenderer.enabled = true;

            bdy = frame.currentFrame.bodys[bodyNumber - 1];

            transform.localPosition = new Vector3(bdy.x, bdy.y, bdy.z);
            transform.localScale = new Vector3(bdy.w, bdy.h, bdy.zwidth);
        } else {
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }
    }

    void OnTriggerEnter(Collider collider) {
        HitboxController hitbox;
        if (collider.gameObject.TryGetComponent(out hitbox)) {
            var selfType = frame.data.type;

            var otherObjTeam = hitbox.frame.team;
            var otherObjectId = hitbox.frame.selfId;
            var otherOwnerObjectId = hitbox.frame.ownerId;

            var selfObjTeam = frame.team;
            var selfObjectId = frame.selfId;
            var selfOwnerObjectId = frame.ownerId;

            switch (selfType) {
                case ObjectTypeEnum.CHARACTER:
                    this.ProcessItrForCharacter(hitbox, hitbox.itr, otherObjTeam, otherObjectId, otherOwnerObjectId,
                            selfObjTeam, selfObjectId, selfOwnerObjectId);
                    break;
                case ObjectTypeEnum.EFFECT:
                    break;
                case ObjectTypeEnum.POWER:
                    break;
            }
        }
    }

    private void ProcessItrForCharacter(HitboxController hitbox, InteractionData itr, TeamEnum otherObjTeam,
            int otherObjId, int otherOwnerObjectId, TeamEnum selfObjTeam, int selfObjId, int selfOwnerObjectId) {
        switch (itr.kind) {
            case ItrKindEnum.CHAR_NORMAL_HIT:
                this.ApplyEnemyDamage(hitbox, itr, otherObjTeam, otherObjId, otherOwnerObjectId, selfObjTeam, selfObjId, selfOwnerObjectId, normalHit);
                break;
            case ItrKindEnum.CHAR_SWORD_HIT:
                this.ApplyEnemyDamage(hitbox, itr, otherObjTeam, otherObjId, otherOwnerObjectId, selfObjTeam, selfObjId, selfOwnerObjectId, swordHit);
                break;
            case ItrKindEnum.CHAR_SELF:
                if (selfObjId == otherObjId) {
                    Debug.Log("Self Hit");
                    return;
                }
                break;
            case ItrKindEnum.CHAR_ALLY:
                if (selfObjTeam == otherObjTeam || selfObjId == otherOwnerObjectId || otherObjId == selfOwnerObjectId) {
                    Debug.Log("Ally Hit");
                    return;
                }
                break;
        }
    }

    private void ApplyEnemyDamage(HitboxController hitbox, InteractionData itr, TeamEnum otherObjTeam,
            int otherObjId, int otherOwnerObjectId, TeamEnum selfObjTeam, int selfObjId, int selfOwnerObjectId,
            GameObject hitEffect
    ) {
        if (selfObjTeam != otherObjTeam || otherObjTeam == TeamEnum.INDEPENDENT || selfObjTeam == TeamEnum.INDEPENDENT) {
            if (selfObjId != otherOwnerObjectId && otherObjId != selfOwnerObjectId) {
                Instantiate(hitEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                Debug.Log("Enemy Hit");
                frame.externAction = true;
                frame.externItr = itr;
                if (frame.currentFrame.state == StateFrameEnum.DEFEND || frame.currentFrame.state == StateFrameEnum.JUMP_DEFEND) {
                    hitbox.DefendingImpact(itr);
                }
            }
        }
    }
}
