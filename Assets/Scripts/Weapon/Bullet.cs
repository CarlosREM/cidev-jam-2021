using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    float life_time = 5;
    // Start is called before the first frame update
    public float damage{get;set;} = 1f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        life_time -= Time.deltaTime;
        if(life_time<=0){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}
