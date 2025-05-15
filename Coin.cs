using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.name == "Player"){
            collision.gameObject.GetComponent<Player>().AddScore(1);
            FindAnyObjectByType<AudioManager>().PlaySFX("Coin_Collect", collision.gameObject);
            Destroy(gameObject);
        }
    }

}