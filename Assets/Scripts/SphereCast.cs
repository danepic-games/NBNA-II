using UnityEngine;

public class SphereCast : MonoBehaviour {

    public Vector3 direction;
    public float maxDistance;
    public bool isHit;
    void OnDrawGizmos() {
        RaycastHit hit;

        bool isHit = Physics.SphereCast(transform.position, transform.lossyScale.x / 2, direction, out hit,
                maxDistance);
        if (isHit) {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, direction * hit.distance);
            Gizmos.DrawWireSphere(transform.position + direction * hit.distance, transform.lossyScale.x / 2);
        } else {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, direction * maxDistance);
        }
    }
}
