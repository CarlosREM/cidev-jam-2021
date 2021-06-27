using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbMobAI : MonoBehaviour
{

    [SerializeField]
    float speed = 2f;

    [SerializeField]
    float speed_by_difficulty = 0.2f;

    [SerializeField]
    float max_health = 10f;
    float health = 10f;

    [SerializeField]
    float healt_by_difficulty = 1f;

    [SerializeField]
    GameObject money_loot;

    [ReadOnly]
    GameController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = (GameController)FindObjectOfType(typeof(GameController));
        health = calculate_max_health();
    }

    float calculate_max_health(){
        return healt_by_difficulty * controller.difficulty + max_health;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target_pos = GameObject.FindWithTag("Player").transform.position;
        float step =  (speed + speed_by_difficulty * controller.difficulty) * Time.deltaTime; // calculate distance to move
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
            float damage = collision.gameObject.GetComponent<Bullet>().damage;
            health -= damage;
            transform.position = Vector3.MoveTowards(transform.position, collision.gameObject.transform.position, Mathf.Max(-1f, -0.05f * damage));
        }
    }
}
