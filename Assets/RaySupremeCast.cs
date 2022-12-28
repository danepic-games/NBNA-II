using UnityEngine;

public class RaySupremeCast : MonoBehaviour {

    public Vector3 direction;
    public float maxDistance;
    public float yOrigin;

    void OnDrawGizmos() {
        RaycastHit hit;

        var origin = new Vector3(transform.position.x, transform.position.y - yOrigin, transform.position.z);

        bool isHit = Physics.Raycast(origin, direction, out hit, maxDistance);
        if (isHit) {
            Debug.Log(hit.collider.name);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, direction * hit.distance);
        } else {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(origin, direction * maxDistance);
        }
    }
}
