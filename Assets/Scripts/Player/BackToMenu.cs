using UnityEngine;
using UnityEngine.InputSystem;
public class BackToMenu : MonoBehaviour
{
    [SerializeField] TransitionManager transitionManager;

    [SerializeField] InputAction escapeAction;
    
    void Start() {
        escapeAction.Enable();
        escapeAction.performed += context => { ReturnToMenu(); };
    }

    void ReturnToMenu() {
        transitionManager.ChangeScene("MainMenu");
    }
}
