using UnityEngine;

public class HitboxController : MonoBehaviour
{

    public InteractionData itr;
    public BoxCollider boxCollider;
    public SpriteRenderer spriteRenderer;
    public FrameController frame;
    public MeshRenderer meshRenderer;
    public PhysicController physicController;

    void Update()
    {
        if (frame.currentFrame.itr != null && frame.currentFrame.itr.HasValue())
        {
            boxCollider.enabled = true;
            meshRenderer.enabled = true;

            itr = frame.currentFrame.itr;

            transform.localPosition = new Vector3(itr.x, itr.y, itr.z);
            transform.localScale = new Vector3(itr.w, itr.h, itr.zwidthz);
        }
        else
        {
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }
    }

    public void DefendingImpact(InteractionData itr)
    {
        this.physicController.defendingImpact = true;
        this.physicController.externForce = new Vector3(itr.dvx, itr.dvy, itr.dvz);
    }

    public void NextIfHit()
    {
        this.frame.enableNextIfHit = true;
    }
}
