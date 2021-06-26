using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] mobs;
    float spawn_period = -1;
    float spawn_cooldown;
    [SerializeField]
    Vector2 spawn_area;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawn_cooldown -= Time.deltaTime;
        if(spawn_cooldown <=0 && spawn_period > 0){
            spawn_cooldown = spawn_period;
            CreateRandomMob();
        }

    }

    public void Spawn(){
            Debug.Log("Creating Mob in spawner ...");
            CreateRandomMob();
    }

    private void CreateRandomMob(){
        int random_index = Random.Range(0, mobs.Length);
        // Add a random offset to the mob
        float randomX = Random.Range(- spawn_area.x , spawn_area.x);
        float randomY = Random.Range(- spawn_area.y , spawn_area.y);
        GameObject new_mob = Instantiate(mobs[random_index], transform.position + new Vector3(randomX, randomY, 0),Quaternion.identity ) as GameObject;
    }
}
