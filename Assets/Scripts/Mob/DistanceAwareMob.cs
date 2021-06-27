using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class DistanceAwareMob : MonoBehaviour
{
    [SerializeField]
    float max_health = 8f;
    [SerializeField]
    float healt_by_difficulty = 2f;
    [SerializeField]
    float healt_regen = 0.5f;
    float health = 8f;

    [SerializeField]
    float health_mult_by_diff = 1.2f;
    
    [SerializeField]
    float health_by_wave = 2; 

    [SerializeField]
    float speed = 2f; 
    [SerializeField]
    float next_waypoint_distance = 0.5F; 

    [ReadOnly]
    GameController controller;
    
    [ReadOnly]
    GameObject target;
    Path path;
    Seeker seeker;

    [SerializeField]
    int current_waypoint = 3;
    bool reached_end_of_path = false;

    [SerializeField]
    GameObject money_loot;

    
    // 0 is iddle 1 is angry
    [SerializeField]
    int state = 0;
    float angry_timer = 0f;
    [SerializeField]
    float default_angry_duration = 3f;
    [SerializeField]
    float sensitivity_distance = 5f;
    // Start is called before the first frame update

    Vector3 scale;
    void Start()
    {
        controller = (GameController)FindObjectOfType(typeof(GameController));
        seeker = gameObject.GetComponent<Seeker>();
        target = GameObject.FindWithTag("Player");

        InvokeRepeating("UpdatePath", 0f, .5f);

        health = calculate_max_health();
        scale = transform.localScale;
    }

    float calculate_max_health(){
        return healt_by_difficulty * controller.difficulty + max_health;
    }

    void UpdatePath(){
        if(seeker.IsDone()){
            seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance_aware();
        switch (state)
      {
        case 0:
            idle();
            break;
        case 1:
            angry();  
            break;
        default:
            state = 0;
            break;
      }    

      if(health <= 0){
            GameObject new_money = Instantiate(money_loot, transform.position,Quaternion.identity );
            new_money.GetComponent<Money>().set_value(3,7);
            Destroy(gameObject);
        }
    }


    //state executed when iddle
    void idle(){
        health = Mathf.Min(calculate_max_health(), health + healt_regen * Time.deltaTime);
        transform.localScale = new Vector3(Mathf.Sin(Time.deltaTime * 0.1f),Mathf.Sin(Time.deltaTime * 0.1f),0 ) + scale;
        
    }

    void distance_aware(){
        float target_dist = Vector2.Distance((Vector2)transform.position, (Vector2)target.transform.position);
        // Debug.Log(target_dist);
        if(target_dist <= sensitivity_distance){
            become_angry();
        }
    }

    //state executed when angry
    void angry(){
        angry_timer -= Time.deltaTime;
        if(path != null && current_waypoint < path.vectorPath.Count){
            reached_end_of_path = false;  
            // Vector2 target_post = new Vector2(target.transform.position.x, target.transform.position.y);
            Vector2 target_path_point =  path.vectorPath[current_waypoint];
            // Debug.Log(direction);

            float step =  speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target_path_point.x,target_path_point.y,0), step);

            float distance = Vector2.Distance((Vector2)transform.position, (Vector2) path.vectorPath[current_waypoint] );
            if(distance <= next_waypoint_distance){
                // Debug.Log(distance);
                // Debug.Log(path.vectorPath.Count);
                current_waypoint++;
            }

        } else if(path != null && current_waypoint >= path.vectorPath.Count){
            reached_end_of_path = true;  
        }

        if(angry_timer <= 0){
            state = 0;
        }
    }

    void become_angry(){
        state = 1;
        angry_timer = default_angry_duration;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "PlayerBullet"){
            become_angry();
            Debug.Log("Collition with mob");
            float damage = collision.gameObject.GetComponent<Bullet>().damage;
            health -= damage;
            transform.position = Vector3.MoveTowards(transform.position, collision.gameObject.transform.position, Mathf.Max(-1f, -0.05f * damage));
        }
    }


    // ------------------------------------------------
    // Movement
    // ------------------------------------------------
    void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            current_waypoint = 0;
        }
    }
}
