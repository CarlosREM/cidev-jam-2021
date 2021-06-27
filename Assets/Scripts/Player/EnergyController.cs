using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    [SerializeField] 
    float charge = 10;
    [SerializeField] 
    float max_charge = 10f; 

    [SerializeField] 
    float energy_per_level = 0.5f; 

    [ReadOnly]
    HudManager hud_manager;
    [ReadOnly]
    GameController controller;

    [SerializeField] 
    float energy_decrease_per_second = 0.1f; 
    // Start is called before the first frame update
    void Start()
    {
        controller = (GameController)FindObjectOfType(typeof(GameController));
        charge = max_charge;
        hud_manager = (HudManager) FindObjectOfType(typeof(HudManager));
    }

    // Update is called once per frame
    void Update()
    {
        charge -= energy_decrease_per_second * Time.deltaTime;
        hud_manager.SetEnergyValue(charge, calculate_max_charge());
    }

    public void OnCharge(float ammount){
        charge = Mathf.Min(charge + ammount, calculate_max_charge());
        // Debug.Log(ammount);
    }

    public float getEnergyPercentaje(){
        return charge/calculate_max_charge();
    }

    private float calculate_max_charge(){
        return max_charge + energy_per_level * controller.battery_level;
    }

    public void ResetEnergy(){
        charge = calculate_max_charge();
    }
}
