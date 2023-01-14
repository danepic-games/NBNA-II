using UnityEngine;

public class BoxCast : MonoBehaviour {

    public Vector3 direction;
    public float maxDistance;
    public BoxCollider boxCollider;

    void Update() {
//        transform.position =
    }

    void OnDrawGizmos() {
        RaycastHit raycastHit;
//
//        bool isHit = Physics.BoxCast(transform.position, transform.lossyScale / 2, direction, out hit,
//                transform.rotation, maxDistance);

        bool isHit = Physics.BoxCast(transform.position, transform.position / 2, Vector3.down, out raycastHit,
                transform.rotation, maxDistance);
        if (isHit) {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, direction * raycastHit.distance);
            Gizmos.DrawWireCube(transform.position + direction * raycastHit.distance, transform.lossyScale);
        } else {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, direction * maxDistance);
        }
    }
}
