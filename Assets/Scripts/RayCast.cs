using UnityEngine;

public class RayCast : MonoBehaviour {

    public Vector3 direction;
    public float maxDistance;
    void OnDrawGizmos() {
        RaycastHit hit;

        bool isHit = Physics.Raycast(transform.position, direction, out hit, maxDistance);
        if (isHit) {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, direction * hit.distance);
        } else {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, direction * maxDistance);
        }
    }
}
