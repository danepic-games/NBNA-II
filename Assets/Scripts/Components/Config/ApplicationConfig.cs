using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components.Config {
    public class ApplicationConfig : MonoBehaviour {
        void Awake() {
            Application.targetFrameRate = 60;
        }

        void Update() {
#if UNITY_EDITOR
            if(Input.GetKey(KeyCode.Backspace)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
#endif
        }
    }
}