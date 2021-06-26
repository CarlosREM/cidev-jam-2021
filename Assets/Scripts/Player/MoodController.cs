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

    // Start is called before the first frame update
    void Start()
    {
      controller = (GameController)FindObjectOfType(typeof(GameController));
      energy_controller = gameObject.GetComponent<EnergyController>();
      original_intensity = player_light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        player_light.intensity = Mathf.Lerp(original_intensity * min_brightness, original_intensity, mood);

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
    }

    

    void ResetMood(){
        mood = 1;
    }

}
