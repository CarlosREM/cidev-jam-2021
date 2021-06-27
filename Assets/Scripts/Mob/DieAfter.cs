using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAfter : MonoBehaviour
{

    [SerializeField]
    float life_time = 2;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        life_time -= Time.deltaTime;
        if(life_time<=0){
            Destroy(gameObject);
        }
    }
}
