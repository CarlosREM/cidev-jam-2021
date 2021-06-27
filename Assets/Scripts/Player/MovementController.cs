using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    Rigidbody2D myRB;
    Animator playerAnimator;

    //Player movement
    [SerializeField] InputAction WASD;
    Vector2 movementInput;

    [SerializeField] float movementSpeed;

    
    [SerializeField] InputAction shootAction;
    bool isShooting = false;


    private void OnEnable() {
        WASD.Enable();
    }
    private void OnDisable() {
        WASD.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        Physics2D.IgnoreLayerCollision(9, 10);
        //bullets should not collide money
        Physics2D.IgnoreLayerCollision(11, 10);
        // myAvatar =transform.GetChild(0);

        shootAction.Enable();
        shootAction.started += context => { isShooting = true;};
        shootAction.canceled += context => { isShooting = false;};
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = WASD.ReadValue<Vector2>();

        float playAnimator = (movementInput.magnitude == 0) ? 0 : 1;

        if (playAnimator == 0) {
            string animState = (isShooting) ? "Attack" : "Movement";
            playerAnimator.Play(animState, 0, 0);
        }
        
        else {
            playerAnimator.SetFloat("Horizontal", movementInput.x);
            playerAnimator.SetFloat("Vertical", movementInput.y);
        }
        playerAnimator.SetFloat("AnimSpeed", playAnimator);

        playerAnimator.SetBool("Attack", isShooting);
    }

    private void FixedUpdate() {
        myRB.velocity = movementInput * movementSpeed;
    }

}
