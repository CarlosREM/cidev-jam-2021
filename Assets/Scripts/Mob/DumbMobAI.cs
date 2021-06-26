using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbMobAI : MonoBehaviour
{

    [SerializeField]
    float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target_pos = GameObject.FindWithTag("Player").transform.position;
        float step =  speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target_pos, step);
    }
}