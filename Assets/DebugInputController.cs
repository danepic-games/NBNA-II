using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugInputController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.@Debug.Enable();
        playerInputActions.@Debug.@ReloadCurrentScene.started += ReloadCurrentScene;
    }

    private void ReloadCurrentScene(InputAction.CallbackContext context) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
