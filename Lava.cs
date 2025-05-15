using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.name == "Player"){
            collision.GetComponent<Player>().TakeDamage(99, "Explosive");
            FindAnyObjectByType<AudioManager>().PlaySFX("Fireball_Hit", gameObject);
        } else if (collision.gameObject.GetComponent<Zombie>()){
            collision.GetComponent<Zombie>().TakeDamage(99, "Explosive");
            FindAnyObjectByType<AudioManager>().PlaySFX("Fireball_Hit", gameObject);
        } else if (collision.gameObject.GetComponent<SkeletonArcher>()){
            collision.GetComponent<SkeletonArcher>().TakeDamage(99, "Explosive");
            FindAnyObjectByType<AudioManager>().PlaySFX("Fireball_Hit", gameObject);
        }
    }

}