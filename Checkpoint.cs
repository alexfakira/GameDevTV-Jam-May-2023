using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.name == "Player"){
            collision.gameObject.GetComponent<Player>().SetRespawnPoint(gameObject.transform.position);
            FindAnyObjectByType<AudioManager>().PlaySFX("Checkpoint", collision.gameObject);
            Destroy(gameObject);
        }
    }

}