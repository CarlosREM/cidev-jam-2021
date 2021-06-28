using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    float life_time = 5;
    [SerializeField]
    GameObject special_effect = null;
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
            end_life();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        end_life();
    }

    private void end_life(){
        if(special_effect != null){
            Instantiate(special_effect, transform.position,Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
