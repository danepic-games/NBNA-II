using UnityEngine;

namespace Components.Config {
    public class ApplicationConfig : MonoBehaviour {
        void Awake() {
            Application.targetFrameRate = 60;
        }
    }
}