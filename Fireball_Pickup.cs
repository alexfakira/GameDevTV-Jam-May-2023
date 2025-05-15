using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_Pickup : MonoBehaviour {

    public bool can_respawn = true;

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.name == "Player"){
            collision.gameObject.GetComponent<Player>().GiveFirePower();
            FindAnyObjectByType<AudioManager>().PlaySFX("Orb_Collect", collision.gameObject);
            gameObject.SetActive(false);
        }
    }

}