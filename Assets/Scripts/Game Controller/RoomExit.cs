using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    [SerializeField] GameController gameController;

    void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Player")) {
            Vector3 roomExitPos = new Vector3(-0.5f, 60, 0);
            other.transform.position = roomExitPos;

            gameController.StartWave();
        }

    }
}
