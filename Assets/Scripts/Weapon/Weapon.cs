using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    // Upgrades    
    [SerializeField]
    float damage;
    [SerializeField]
    float damage_by_level;
    [SerializeField]
    float max_damage;
    [SerializeField]
    float spread = 0.1f;
    [SerializeField]
    float spread_by_level = 0.01f;
    [SerializeField]
    float min_spread = 0.0f;
    [SerializeField]
    float cadence_time = 1.0f;
    [SerializeField]
    float cadence_time_by_level = 0.01f;
    [SerializeField]
    float min_cadence = 0.2f;
    [SerializeField]
    float bullet_speed = 8f;
    [SerializeField]
    float bullet_speed_by_level= 0.2f;
    [SerializeField]
    float max_bullet_speed = 15f ;

    // Upgrades    
    [SerializeField]
    private float cost;
    [SerializeField]
    private float cost_by_level = 10f;
    [SerializeField]
    private float exp_cost_by_level = 0.01f;
    [SerializeField]
    float cooldown = 0f; 
    public int level{get; private set;}

    //Player movement
    [SerializeField] 
    InputAction ShootAction;
    
    [SerializeField] 
    GameObject skill_prefab;
    [SerializeField] 
    GameObject[] bullet_prefab;
    [SerializeField] 
    public bool active_weapon = false;
    public bool purchased = false;
    bool shooting = false;
    

    // private void bool m_pressed;
    // private void bool m_released;

    // Start is called before the first frame update
    void Start()
    {
        ShootAction.Enable();
        ShootAction.started += context => {shooting = true ;};
        ShootAction.canceled  += context => {shooting = false;};
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime ;
        if(cooldown <= 0 && shooting){
            Debug.Log("shooting");
            Shoot();
            cooldown = Mathf.Max(min_cadence, cadence_time - cadence_time_by_level*level);
        }
    }

    void Shoot(){
        if(!active_weapon) return;
        int random_index = Random.Range(0, bullet_prefab.Length);


        Vector3 difference = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        float new_spread = Mathf.Max(min_spread, spread + spread_by_level*level);
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg + Random.Range(-new_spread/2, new_spread/2);
        Quaternion target_rotation = Quaternion.Euler(0f, 0f, rotation_z);
        GameObject new_bullet = Instantiate(bullet_prefab[random_index], transform.position, target_rotation ) as GameObject;
        float new_speed = Mathf.Max(max_bullet_speed, bullet_speed + bullet_speed_by_level*level);
        new_bullet.GetComponent<Rigidbody2D>().AddForce(new_bullet.transform.right * new_speed);
        float new_damage = Mathf.Min(max_damage, damage + damage_by_level*level);
        new_bullet.GetComponent<Bullet>().damage = new_damage;
        
    }

    public void upgrade(){
        level++;
    }

    public float get_upgrade_cost(){
        return cost_by_level*level + cost + level*level + exp_cost_by_level; 
    }
}
