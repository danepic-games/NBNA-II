using UnityEngine;

public class HitboxController : MonoBehaviour {

    public InteractionData itr;
    public BoxCollider boxCollider;
    public SpriteRenderer spriteRenderer;
    public FrameController frame;
    public MeshRenderer meshRenderer;

    void Update() {
        if (frame.currentFrame.itr != null && frame.currentFrame.itr.HasValue()) {
            boxCollider.enabled = true;
            meshRenderer.enabled = true;

            itr = frame.currentFrame.itr;

            transform.localPosition = new Vector3(itr.x, itr.y, itr.z);
            transform.localScale = new Vector3(itr.w, itr.h, itr.zwidthz);
        } else {
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }
    }

    public void DefendingImpact(InteractionData itr) {
        frame.externAction = true;
        var newItr = new InteractionData();
        newItr.action = -1;
        newItr.dvx = itr.dvx;
        newItr.kind = ItrKindEnum.CHAR_NORMAL_HIT;
        newItr.defensable = false;
        frame.externItr = newItr;
    }
}
