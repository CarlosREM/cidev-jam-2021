using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbMobAI : MonoBehaviour
{

    [SerializeField]
    float speed = 2f;

    [SerializeField]
    float health = 10f;

    [SerializeField]
    GameObject money_loot;
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
        if(health <= 0){
            GameObject new_money = Instantiate(money_loot, transform.position,Quaternion.identity );
            new_money.GetComponent<Money>().set_value(3,7);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "PlayerBullet"){
            Debug.Log("Collition with mob");
            health -= collision.gameObject.GetComponent<Bullet>().damage;
        }
    }
}
