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
    [SerializeField]
    float deadzoneRadius = 0;
    float deadzoneOffsetX;
    float deadzoneOffsetY;
    Vector2 deadzone = new Vector2(Screen.width/2, Screen.height/2);
    float distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
    
        distance = Vector2.Distance(Mouse.current.position.ReadValue(),deadzone);

        if(distance>deadzoneRadius){
            // update camera position to player
            Vector3 temp = target.position;
            temp.z = transform.position.z;
            // Assign value to Camera position

            //modify postion according to mouse position
            // Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());


            temp.x += Mathf.Lerp(-maxLookout,maxLookout,- (temp.x - (worldPosition.x)) * mouseSensitivity + offset.x);
            temp.y += Mathf.Lerp(-maxLookout,maxLookout,- (temp.y - (worldPosition.y)) * mouseSensitivity + offset.y);
            
            transform.position = temp;
        }
    }
}
