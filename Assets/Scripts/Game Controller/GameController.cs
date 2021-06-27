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
    [ReadOnly]
    EnergyController energy_controller;
    [ReadOnly]
    MoodController mood_controller;


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

    [ReadOnly]
    HudManager hud_manager;

    [SerializeField]
    public int bravery_level = 0;
    [SerializeField]
    public int battery_level = 0;
    [SerializeField]
    public int luck = 0;


    [Header("UI Overlays")]
    [SerializeField] CanvasGroup GameOverCanvas;

    // Start is called before the first frame update
    void Start()
    {
        wave_controller = (WaveController)FindObjectOfType(typeof(WaveController));
        energy_controller = (EnergyController)FindObjectOfType(typeof(EnergyController));
        mood_controller = (MoodController)FindObjectOfType(typeof(MoodController));
        hud_manager = (HudManager) FindObjectOfType(typeof(HudManager));   
        RecalculateDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        hud_manager.SetNightProgress(wave_time - time, wave_time);
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
        //reset player stats
        mood_controller.ResetMood();
        energy_controller.ResetEnergy();
    }

    void EndWave(){
        wave += 1;
        Debug.Log(wave);
        //call wave controller
        wave_controller.EndWave();
        AddExp(1);


        // open menu

    }



    // Custom controller functions
    public void EndGame(){
        // TODO 
        Time.timeScale = 0;

        // go to main menu
        StartCoroutine(EndGameCoroutine());
    }
    IEnumerator EndGameCoroutine() {

        float step = 0.01f;
        while(GameOverCanvas.alpha < 1) {
            GameOverCanvas.alpha += step*Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }

        yield return new WaitForSecondsRealtime(3f);

        GameObject.FindWithTag("Scene Manager").GetComponent<TransitionManager>().ChangeScene("MainMenu");
        yield break;
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
        hud_manager.SetBits(money);
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
            hud_manager.SetBits(money);
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

    public bool increase_bravery(){
        bool succesfull = ExpendExp(1);
        if(succesfull){
            bravery_level++;
        }
        return succesfull;
    }

    public bool increase_lucky(){
        bool succesfull = ExpendExp(1);
        if(succesfull){
            luck++;
        }
        return succesfull;
    }
    public bool increase_battery(){
        bool succesfull = ExpendExp(1);
        if(succesfull){
            battery_level++;
        }
        return succesfull;
    }



}
