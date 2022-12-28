using UnityEngine;

public class HurtboxController : MonoBehaviour {

    public int bodyNumber;
    public HurtboxesController hurtboxes;
    public BodyData bdy;
    public BoxCollider boxCollider;
    public SpriteRenderer spriteRenderer;
    public FrameController frame;
    public MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (frame.currentFrame.bodys != null && frame.currentFrame.bodys.Count >= bodyNumber) {
            boxCollider.enabled = true;
            meshRenderer.enabled = true;

            bdy = frame.currentFrame.bodys[bodyNumber - 1];

            var centerx = spriteRenderer.sprite.pivot.x / 100;

            float x = bdy.x / 100;

            float y = (((spriteRenderer.sprite.bounds.size.y * 100) - bdy.y) / 2) / 100;
            float w = (spriteRenderer.sprite.bounds.size.x * bdy.w) / (spriteRenderer.sprite.bounds.size.x * 100);
            float h = (spriteRenderer.sprite.bounds.size.x * bdy.h) / (spriteRenderer.sprite.bounds.size.y * 100);

            transform.localPosition = new Vector3((transform.localScale.x / 2) - centerx + x, y, bdy.z);
            transform.localScale = new Vector3(w, h, bdy.zwidth);
        } else {
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }
    }
}
