using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [ReadOnly]
    GameController controller;

    [SerializeField]
    public int value = 5;

    [SerializeField]
    public float money_by_luck = 0.5f;
    [SerializeField]
    float rot_speed=0.5f;


    // Start is called before the first frame update
    void Start()
    {
        controller = (GameController)FindObjectOfType(typeof(GameController));
    }

    void Update(){
        transform.Rotate(0, rot_speed, 0, Space.Self);

    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player"){
            controller.OnMoneyCollected(value);
            Destroy(gameObject);
        }
    }

    public void set_value(int min, int max){
        controller = (GameController)FindObjectOfType(typeof(GameController));
        float extra_by_luck = money_by_luck *controller.luck;
        value = Random.Range(min + (int)Mathf.Ceil(extra_by_luck), max + 1 + (int)Mathf.Ceil(extra_by_luck));
        transform.localScale = transform.localScale*Random.Range(0.7f, 1.0f);
    }
}
