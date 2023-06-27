using System;
using UnityEngine;

public class HurtboxController : MonoBehaviour
{

    public BodyData bdy;
    public BoxCollider mainCollider;
    public BoxCollider boxCollider;
    public SpriteRenderer spriteRenderer;
    public FrameController frame;
    public MeshRenderer meshRenderer;
    public ObjectPointController objectPointController;

    void Update()
    {
        if (mainCollider && transform)
        {
            mainCollider.center = transform.localPosition;
            mainCollider.size = transform.localScale;
        }

        if (frame.currentFrame.body != null && frame.currentFrame.body.HasValue())
        {
            boxCollider.enabled = true;
            meshRenderer.enabled = true;

            bdy = frame.currentFrame.body;

            transform.localPosition = new Vector3(bdy.x, bdy.y, bdy.z);
            transform.localScale = new Vector3(bdy.w, bdy.h, bdy.zwidth);
        }
        else
        {
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        HitboxController hitbox;
        if (collider.gameObject.TryGetComponent(out hitbox))
        {
            if (hitbox.frame.currentFrame.itr != null && hitbox.frame.currentFrame.itr.HasValue())
            {
                var selfType = frame.data.type;

                var otherObjTeam = hitbox.frame.team;
                var otherObjectId = hitbox.frame.selfId;
                var otherOwnerObjectId = hitbox.frame.ownerId;

                var selfObjTeam = frame.team;
                var selfObjectId = frame.selfId;
                var selfOwnerObjectId = frame.ownerId;

                switch (selfType)
                {
                    case ObjectTypeEnum.CHARACTER:
                        this.ProcessItrForCharacter(hitbox, hitbox.itr, otherObjTeam, otherObjectId, otherOwnerObjectId,
                                selfObjTeam, selfObjectId, selfOwnerObjectId);
                        break;
                    case ObjectTypeEnum.EFFECT:
                        break;
                    case ObjectTypeEnum.POWER:
                        this.ProcessItrForPower(hitbox, hitbox.itr, otherObjTeam, otherObjectId, otherOwnerObjectId,
                                selfObjTeam, selfObjectId, selfOwnerObjectId);
                        break;
                }
            }
        }
    }

    private void ProcessItrForPower(HitboxController hitbox, InteractionData itr, TeamEnum otherObjTeam,
            int otherObjectId, int otherOwnerObjectId, TeamEnum selfObjTeam, int selfObjectId, int selfOwnerObjectId)
    {
        switch (itr.kind)
        {
            case ItrKindEnum.CHAR_NORMAL_HIT:
            case ItrKindEnum.CHAR_SWORD_HIT:
                this.ApplyEnemyDamageForPower(hitbox, itr, otherObjTeam, otherObjectId, otherOwnerObjectId, selfObjTeam, selfObjectId, selfOwnerObjectId);
                break;
        }
    }

    private void ProcessItrForCharacter(HitboxController hitbox, InteractionData itr, TeamEnum otherObjTeam,
            int otherObjId, int otherOwnerObjectId, TeamEnum selfObjTeam, int selfObjId, int selfOwnerObjectId)
    {
        switch (itr.kind)
        {
            case ItrKindEnum.CHAR_NORMAL_HIT:
            case ItrKindEnum.CHAR_SWORD_HIT:
                this.ApplyEnemyDamageForCharacter(hitbox, itr, otherObjTeam, otherObjId, otherOwnerObjectId, selfObjTeam, selfObjId, selfOwnerObjectId);
                break;
            case ItrKindEnum.CHAR_SELF:
                if (selfObjId == otherObjId)
                {
                    Debug.Log("Self Hit");
                    return;
                }
                break;
            case ItrKindEnum.CHAR_ALLY:
                if (selfObjTeam == otherObjTeam || selfObjId == otherOwnerObjectId || otherObjId == selfOwnerObjectId)
                {
                    Debug.Log("Ally Hit");
                    return;
                }
                break;
        }
    }

    private void ApplyEnemyDamageForCharacter(HitboxController hitbox, InteractionData itr, TeamEnum otherObjTeam,
            int otherObjId, int otherOwnerObjectId, TeamEnum selfObjTeam, int selfObjId, int selfOwnerObjectId
    )
    {
        if (selfObjTeam != otherObjTeam || otherObjTeam == TeamEnum.INDEPENDENT || selfObjTeam == TeamEnum.INDEPENDENT)
        {
            if (selfObjId != otherOwnerObjectId && otherObjId != selfOwnerObjectId)
            {
                if (itr.kind == ItrKindEnum.CHAR_NORMAL_HIT)
                {
                    objectPointController.InvokeNormalHit(transform.position);
                }
                else
                {
                    objectPointController.InvokeSwordHit(transform.position);
                }

                Debug.Log("Enemy Hit");
                frame.externAction = true;
                frame.externItr = itr;
                if (frame.currentFrame.state == StateFrameEnum.DEFEND || frame.currentFrame.state == StateFrameEnum.JUMP_DEFEND)
                {
                    hitbox.DefendingImpact(itr);
                }
            }
        }
    }

    private void ApplyEnemyDamageForPower(HitboxController hitbox, InteractionData itr, TeamEnum otherObjTeam, int otherObjectId,
            int otherOwnerObjectId, TeamEnum selfObjTeam, int selfObjectId, int selfOwnerObjectId)
    {
        if (selfObjTeam != otherObjTeam || otherObjTeam == TeamEnum.INDEPENDENT || selfObjTeam == TeamEnum.INDEPENDENT)
        {
            if (selfObjectId != otherOwnerObjectId && otherObjectId != selfOwnerObjectId)
            {
                if (itr.kind == ItrKindEnum.CHAR_NORMAL_HIT)
                {
                    objectPointController.InvokeNormalHit(transform.position);
                }
                else
                {
                    objectPointController.InvokeSwordHit(transform.position);
                }

                Debug.Log("Enemy Hit Power");
                frame.externAction = true;
                frame.externItr = itr;
            }
        }
    }
}
