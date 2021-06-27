using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    [SerializeField] 
    float charge = 10;
    [SerializeField] 
    float max_charge = 10f; 

    [ReadOnly]
    HudManager hud_manager;

    [SerializeField] 
    float energy_decrease_per_second = 0.1f; 
    // Start is called before the first frame update
    void Start()
    {
        charge = max_charge;
        hud_manager = (HudManager) FindObjectOfType(typeof(HudManager));
    }

    // Update is called once per frame
    void Update()
    {
        charge -= energy_decrease_per_second * Time.deltaTime;
        hud_manager.SetEnergyValue(charge, max_charge);
    }

    public void OnCharge(float ammount){
        charge = Mathf.Min(charge + ammount, max_charge);
        // Debug.Log(ammount);
    }

    public float getEnergyPercentaje(){
        return charge/max_charge;
    }
}
