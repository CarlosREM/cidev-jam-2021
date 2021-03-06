using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{

    [SerializeField]
    Rigidbody2D connector_tip;

    [SerializeField]
    float speed = 5;
    GameObject player = null;

    [SerializeField]
    SpriteRenderer[] chain_renderers;
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        if(player != null){
            float step =  speed * Time.deltaTime; // calculate distance to move
            Vector3 new_position = Vector3.MoveTowards(connector_tip.transform.position, player.transform.position, step);
            connector_tip.MovePosition( new_position );
        }else{
            float step =  speed * Time.deltaTime; // calculate distance to move
            Vector3 new_position = Vector3.MoveTowards(connector_tip.transform.position, transform.position, step);
            connector_tip.MovePosition( new_position );
        }
    }

    void chain_visibility(bool new_value){
        for(int i = 0; i < chain_renderers.Length; i++ ){
            chain_renderers[i].enabled = new_value;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player"){
            Debug.Log("Connected to player ...");
            player = collider.gameObject;
            chain_visibility(true);
        }
        // Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        // spriteMove = -0.1f;
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if(collider.tag == "Player"){
            player = null;
            chain_visibility(false);
        }
    }
}
