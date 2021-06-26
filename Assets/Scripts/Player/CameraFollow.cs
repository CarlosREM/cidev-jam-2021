using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    Vector2 offset;
    [SerializeField]
    Transform target;
    [SerializeField]
    [Range(0.0F, 1.0F)]
    float mouseSensitivity;
    [SerializeField]
    float maxLookout;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
    
    // update camera position to player
    Vector3 temp = target.position;
    temp.z = transform.position.z;
    // Assign value to Camera position

    //modify postion according to mouse position
    // Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

    temp.x += Mathf.Lerp(-maxLookout,maxLookout,- (temp.x - worldPosition.x ) * mouseSensitivity + offset.x);
    temp.y +=  Mathf.Lerp(-maxLookout,maxLookout,- (temp.y - worldPosition.y )*mouseSensitivity + offset.y);
    
    transform.position = temp;
 }
}
