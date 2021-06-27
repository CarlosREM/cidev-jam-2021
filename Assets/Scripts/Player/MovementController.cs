using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    Rigidbody2D myRB;
    // Transform myAvatar;

    //Player movement
    [SerializeField] 
    InputAction WASD;
    Vector2 movementInput;
    [SerializeField] 
    float movementSpeed;


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
        Physics2D.IgnoreLayerCollision(9, 10);
        // myAvatar =transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = WASD.ReadValue<Vector2>();

        if(movementInput.x != 0){
            transform.localScale = new Vector2(-Mathf.Sign(movementInput.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    private void FixedUpdate() {
        myRB.velocity = movementInput * movementSpeed;
    }

}
