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
    float deadzoneX = 0.3f;
    float deadzoneY = 0.2f;

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
    float x_lookout = - (temp.x - worldPosition.x ) * mouseSensitivity;
    float y_lookout = - (temp.y - worldPosition.y ) * mouseSensitivity;

    if(Mathf.Sign(x_lookout)> 0 ){
        temp.x += Mathf.Lerp( 0,maxLookout,x_lookout - deadzoneX);

    }else{
        temp.x += Mathf.Lerp( 0,-maxLookout, Mathf.Abs(x_lookout) - deadzoneX);
    }

    if(Mathf.Sign(y_lookout)> 0 ){
        temp.y +=  Mathf.Lerp(0,maxLookout,y_lookout - deadzoneY );
    }else{
        temp.y +=  Mathf.Lerp(0,-maxLookout,Mathf.Abs( y_lookout ) - deadzoneY);
    }
    
    transform.position = temp;
 }
}
