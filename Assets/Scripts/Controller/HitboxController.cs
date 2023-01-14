using UnityEngine;

public class HitboxController : MonoBehaviour {

    public int itrNumber;
    public HitboxesController hitboxes;
    public InteractionData itr;
    public BoxCollider boxCollider;
    public SpriteRenderer spriteRenderer;
    public FrameController frame;
    public MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (frame.currentFrame.itrs != null && frame.currentFrame.itrs.Count >= itrNumber) {
            boxCollider.enabled = true;
            meshRenderer.enabled = true;

            itr = frame.currentFrame.itrs[itrNumber - 1];

            var centerx = spriteRenderer.sprite.pivot.x / 100;

            float x = itr.x / 100;

            float y = (((spriteRenderer.sprite.bounds.size.y * 100) - itr.y) / 2) / 100;
            float w = (spriteRenderer.sprite.bounds.size.x * itr.w) / (spriteRenderer.sprite.bounds.size.x * 100);
            float h = (spriteRenderer.sprite.bounds.size.x * itr.h) / (spriteRenderer.sprite.bounds.size.y * 100);

            transform.localPosition = new Vector3((transform.localScale.x / 2) - centerx + x, y, itr.z);
            transform.localScale = new Vector3(w, h, itr.zwidthz);
        } else {
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }
    }

    void OnTriggerEnter(Collider collider) {
    }
}
