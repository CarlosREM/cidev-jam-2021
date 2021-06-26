using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [ReadOnly]
    GameController controller;
    public bool active{get;private set;} = false;

    // Spawn Config 
    [SerializeField]
    [Range(0.0F, 1.0F)]
    public float base_spawn_cooldown = 5f;
    [SerializeField]
    [Range(0.0F, 1.0F)]
    public float time_randomization = 3f;
    [SerializeField]
    [Range(0.0F, 1.0F)]
    public float spawn_time_decrease_by_wave = 0.25f;
    [SerializeField]
    [Range(0.0F, 1.0F)]
    public float spawn_time_decrease_by_difficulty = 0.5f;
    [SerializeField]
    public Spawner[] regular_spawners;


    // Task Config
    [SerializeField]
    [Range(0.0F, 1.0F)]
    public float base_task_cooldown = 20f;
    [SerializeField]
    [Range(0.0F, 1.0F)]
    public float task_time_randomization = 3f;
    [SerializeField]
    [Range(0.0F, 1.0F)]
    public float task_time_decrease_by_wave = 0.25f;
    [SerializeField]
    [Range(0.0F, 1.0F)]
    public float task_time_decrease_by_difficulty = 0.5f;
    [SerializeField]
    public Task[] regular_task;

    float task_cooldown  = 0f;
    public float spawn_cooldown = 0f;

    // Start is called before the first frame update
    
    void Start()
    {
      controller = (GameController)FindObjectOfType(typeof(GameController)); 
      spawn_cooldown = base_spawn_cooldown; 
      task_cooldown = base_spawn_cooldown; 
    }

    // Update is called once per frame
    void Update()
    {   
        spawn_cooldown -= Time.deltaTime;
        // float new_spawn_probability = spawn_probability + spawn_probability_wave_increase * controller.wave + spawn_prob_difficulty_increase;
        if(spawn_cooldown <= 0 ){
            spawn_cooldown = base_spawn_cooldown - Mathf.Log(Mathf.Max(1,controller.wave * spawn_time_decrease_by_wave)) + GetDifficulty() * spawn_time_decrease_by_difficulty; 
            int random_index = Random.Range(0, regular_spawners.Length);
            regular_spawners[random_index].Spawn();
        }

        if(task_cooldown <= 0 && regular_task.Length > 0){
            task_cooldown = base_task_cooldown - Mathf.Log(Mathf.Max(1,controller.wave * task_time_decrease_by_wave)) + GetDifficulty() * task_time_decrease_by_difficulty; 
            int random_index = Random.Range(0, regular_task.Length);
            regular_task[random_index].Activate();
            // Activate a random inactive task
        }
        
    }

    private void FixedUpdate() {
        
    }

    public void StartWave(){
        active = true;
        Debug.Log("Wave Started");
    }

    public void EndWave(){
        active = false;
        disableAllTasks();
        Debug.Log("Wave Ended");
    }

    void disableAllTasks(){
          for (int i = 0; i < regular_task.Length; i++)
        {
            regular_task[i].Deactivate();
        }
    }

    float GetDifficulty(){
        return controller.difficulty;
    }
}
