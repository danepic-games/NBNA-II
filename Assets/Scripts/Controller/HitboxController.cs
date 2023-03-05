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

            transform.localPosition = new Vector3(itr.x, itr.y, itr.z);
            transform.localScale = new Vector3(itr.w, itr.h, itr.zwidthz);
        } else {
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }
    }

    void OnTriggerEnter(Collider collider) {
    }

    public void DefendingImpact(InteractionData itr) {
        Debug.Log("DefendingImpact");
        frame.externAction = true;
        var newItr = new InteractionData();
        newItr.action = -1;
        newItr.dvx = itr.dvx;
        newItr.kind = ItrKindEnum.CHAR_NORMAL_HIT;
        newItr.defensable = false;
        frame.externItr = newItr;
    }
}
