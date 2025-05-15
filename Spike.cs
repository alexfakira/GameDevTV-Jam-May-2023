using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.name == "Player"){
            collision.gameObject.GetComponent<Player>().TakeDamage(99, "Normal");
            FindAnyObjectByType<AudioManager>().PlaySFX("Spike_Death", gameObject);
        } else if (collision.gameObject.GetComponent<Zombie>()){
            collision.gameObject.GetComponent<Zombie>().TakeDamage(99, "Normal");
            FindAnyObjectByType<AudioManager>().PlaySFX("Spike_Death", gameObject);
        } else if (collision.gameObject.GetComponent<SkeletonArcher>()){
            collision.gameObject.GetComponent<SkeletonArcher>().TakeDamage(99, "Normal");
            FindAnyObjectByType<AudioManager>().PlaySFX("Spike_Death", gameObject);
        }
    }

}