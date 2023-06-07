using UnityEngine;

public class HurtboxesController : MonoBehaviour {

    public BoxCollider boxCollider;

    public Transform mainHurtbox;

    void Update() {
        if (boxCollider && mainHurtbox) {
            boxCollider.center = mainHurtbox.localPosition;
            boxCollider.size = mainHurtbox.localScale;
        }
    }
}
