using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(EnergyController))]
public class MoodController : MonoBehaviour
{

    [SerializeField]
    float mood = 1;
    [ReadOnly]
    GameController controller;

    [ReadOnly]
    EnergyController energy_controller;

    [ReadOnly]
    HudManager hud_manager;

    [SerializeField]
    Light2D player_light;

    [SerializeField]
    [Range(0f,1f)]
    float min_brightness = 0.2f;
    float original_intensity = 0f;

    [SerializeField]
    float max_mood_loss = 0.05f;
    [SerializeField]
    float max_mood_gain = 0.1f;

    [SerializeField]
    float mob_contact_damage= 0.05f;
    [SerializeField]
    float damage_tick = 0.5f;
    [SerializeField]
    float damage_cooldown = 0.5f;
    
    [SerializeField]
    float def_by_level = 0.03f;
    [SerializeField]
    float max_def = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
      controller = (GameController)FindObjectOfType(typeof(GameController));
      hud_manager = (HudManager) FindObjectOfType(typeof(HudManager));
      energy_controller = gameObject.GetComponent<EnergyController>();
      original_intensity = player_light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        player_light.intensity = Mathf.Lerp(original_intensity * min_brightness, original_intensity, mood);
        hud_manager.SetSanityValue(mood,1);
        float energy_percent = energy_controller.getEnergyPercentaje();
        if(energy_percent < 0.9){
            mood -= Mathf.Lerp(0, max_mood_loss, 1 - energy_percent) * Time.deltaTime;
        }else{
            mood += Mathf.Lerp(0, max_mood_gain,  ( energy_percent - 0.9f) * 10f ) * Time.deltaTime;
        }

        mood = Mathf.Clamp(mood, 0f, 1f);
        if(mood <= 0.0001){
            //Die
            controller.EndGame();
        }

        //damage
        damage_cooldown -= Time.deltaTime;
    }

    public void ResetMood(){
        mood = 1;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Mob"){
            ContactDamage();
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.tag == "Mob"){
            ContactDamage();
        }
    }

    private void ContactDamage(){
        if(damage_cooldown <= 0){
            Debug.Log("Contact damage");
            damage_cooldown = damage_tick;
            mood -= mob_contact_damage + mob_contact_damage * Mathf.Min(max_def, def_by_level *controller.bravery_level);
        }
    }
}
