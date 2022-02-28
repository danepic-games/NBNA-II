using UnityEngine;

namespace Components.Scripts {
    public class RaycastDebugScript : MonoBehaviour {

        public float x;
        public float y;
        public float z;

        public Vector3 direction;
        public float distance;

        public string layer;

        void Update() {
            bool grounded = (Physics.Raycast((new Vector2(x, y)), direction, distance, 1 << LayerMask.NameToLayer(layer)));

            if(direction.x != 0) {
                Debug.DrawRay((new Vector3(x, y, z)), new Vector3(distance * direction.x, direction.y, direction.z), Color.red);

            } else if (direction.y != 0) {
                Debug.DrawRay((new Vector3(x, y, z)), new Vector3(direction.x, distance * direction.y, direction.z), Color.red);

            } else if (direction.z != 0) {
                Debug.DrawRay((new Vector3(x, y, z)), new Vector3(direction.x, direction.y, distance * direction.z), Color.red);
            }
        }

    }
}