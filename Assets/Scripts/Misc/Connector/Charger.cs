using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{
    [SerializeField]
    float charge_speed = 0.7f;

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player"){
            // Debug.Log("Connected to player ...");
            GameObject player = collider.gameObject;
            player.GetComponent<EnergyController>().OnCharge(charge_speed * Time.deltaTime);
        }
    }
}
