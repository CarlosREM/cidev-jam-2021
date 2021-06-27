using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    // variables visible to other scripts
    public int experience { get; private set; }
    public float difficulty { get; private set; }
    public int money{get; private set; } = 0;
    [ReadOnly]
    WaveController wave_controller;
    [ReadOnly]
    WeaponController weapon_controller;


    [SerializeField]
    public float difficulty_increase_per_wave = 1;
    [SerializeField]
    [Range(1.0F, 5.0F)]
    public float max_difficulty_multiplier = 2;
    [SerializeField]
    [Range(0.0F, 1.0F)]
    public float time_difficulty_increase_percent = 0.2f;
    [SerializeField]
    public float base_difficulty = 1;

    [SerializeField]
    [Range(1.0F, 5.0F)]
    public float time_increase_per_wave = 1;
    [SerializeField]
    float base_time;

    public int wave{get; private set;} = 0;
    private float difficulty_multiplier;
    //stores available time
    [ReadOnly]
    private float time;
    //stores wave duration
    [ReadOnly]
    private float wave_time;

    // Start is called before the first frame update
    void Start()
    {
        wave_controller = (WaveController)FindObjectOfType(typeof(WaveController));   
        RecalculateDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if(time <= 0 && wave_controller.active){
            EndWave();
        }else if(time <= 0){
            StartWave();
        }
        RecalculateDifficulty();
    }


    /**
    * -----------------------------------------------------
    * Game Loop
    * -----------------------------------------------------
    **/
    void StartWave(){
        time = base_time  + time_increase_per_wave * wave;
        wave_time = time;
        Debug.Log(time);
        //call wave controller
        wave_controller.StartWave();
    }

    void EndWave(){
        wave += 1;
        Debug.Log(wave);
        //call wave controller
        wave_controller.EndWave();
        // open menu
    }



    // Custom controller functions
    public void EndGame(){
        // TODO 

        // go to main menu
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    /**
    * -----------------------------------------------------
    * Difficulty Functions
    * -----------------------------------------------------
    **/
    void RecalculateDifficulty(){
        difficulty = base_difficulty + wave * difficulty_multiplier;
    }

    // mood is a value betwen 0 and 1
    public void SetMood(float mood){
        //sets difficulty multiplier betwen 1 and max_difficulty_multiplier
        float difficulty_by_time = Mathf.Lerp(0, time_difficulty_increase_percent, (time / wave_time));
        difficulty_multiplier = Mathf.Lerp(1, max_difficulty_multiplier, mood + difficulty_by_time);
    }

    // Listener for mood observer
    public void OnMoodChange(float mood){
        SetMood(mood);
    }

    /**
    * -----------------------------------------------------
    * Money Functions
    * -----------------------------------------------------
    **/
    // Listener for mood observer
    public void OnMoneyCollected(int new_money){
        money += new_money;
    }

    public int getPrice( int index){
        return weapon_controller.getPrice(index);
    }

    public bool CanPurchase( int index){
        int price = weapon_controller.getPrice(index);
        if(price <= money && price > 0){
            return true;
        }
        return false;
    }

    public bool Purchase( int index ){
        int price = weapon_controller.getPrice(index);
        if(price <= money && price > 0){
            money -= price;
            weapon_controller.upgrade(index);
            return true;
        }
        return false;
    }

    /**
    * -----------------------------------------------------
    * Exp functions
    * -----------------------------------------------------
    **/
    private void AddExp(int exp){
        experience += exp;
    }

    private bool ExpendExp(int ammount){
        if(experience - ammount >= 0){        
            experience -= ammount;
            return true;
        }
        return false;
    }


}
