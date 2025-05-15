using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

    public bool can_respawn = true;

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.name == "Player"){
            collision.gameObject.GetComponent<Player>().SwitchGravity();
            FindAnyObjectByType<AudioManager>().PlaySFX("Orb_Collect", collision.gameObject);
            gameObject.SetActive(false);
        }
    }

}