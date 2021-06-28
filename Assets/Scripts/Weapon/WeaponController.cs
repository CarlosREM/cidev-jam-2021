using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    [SerializeField]
    GameObject[] weapons;

        //Player movement
    [SerializeField] 
    InputAction NextWeaponAction;

        //Player movement
    [SerializeField] 
    InputAction PreviousWeaponAction;

    bool next_weapon = false;
    bool prev_next_weapon= false; 

    bool prev_weapon = false;
    bool prev_prev_weapon = false;

    int current_weapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        NextWeaponAction.Enable();
        PreviousWeaponAction.Enable();

        NextWeaponAction.started += context => { next_weapon = true;};
        NextWeaponAction.canceled  += context => { next_weapon = false;};
        
        PreviousWeaponAction.started += context => { prev_weapon = true;};
        PreviousWeaponAction.canceled  += context => { prev_weapon = false;};
    }

    // Update is called once per frame
    void Update()
    {

        
        if(!prev_next_weapon && next_weapon){
            //clicked next weapon
            calculat_next_weapon();
        }

        if(!prev_prev_weapon && prev_weapon){
            //clicked prev weapon
            calculat_previous_weapon();
        }

        prev_next_weapon = next_weapon;
        prev_prev_weapon = prev_weapon;
        
    }

    void calculat_next_weapon(){
        weapons[current_weapon].GetComponent<Weapon>().active_weapon=false;
        current_weapon = (current_weapon+1)%weapons.Length;
        weapons[current_weapon].GetComponent<Weapon>().active_weapon=true;
        if(weapons[current_weapon].GetComponent<Weapon>().purchased == false){
            calculat_next_weapon();
        }
    }

    void calculat_previous_weapon(){
        weapons[current_weapon].GetComponent<Weapon>().active_weapon=false;
        current_weapon = (current_weapon-1)%weapons.Length;
        if(current_weapon < 0) current_weapon = weapons.Length - 1;
        weapons[current_weapon].GetComponent<Weapon>().active_weapon=true;
        // weapons[current_weapon].GetComponent<Weapon>().upgrade();
        if(weapons[current_weapon].GetComponent<Weapon>().purchased == false){
            calculat_previous_weapon();
        }
    }

    public int getPrice(int index){
        if(index < 0 || weapons.Length <= index ) return -1;
        return (int)weapons[index].GetComponent<Weapon>().get_upgrade_cost();
    }

    public bool upgrade(int index){
        if(index < 0 || weapons.Length <= index ) return false;
        weapons[index].GetComponent<Weapon>().upgrade();
        return true;
    }
}
