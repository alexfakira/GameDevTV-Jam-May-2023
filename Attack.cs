using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.GetComponent<Slime>()){
            collision.gameObject.GetComponent<Slime>().Die();
        } else if (collision.GetComponent<Zombie>()){
            collision.gameObject.GetComponent<Zombie>().TakeDamage(3, "Normal");
        } else if (collision.GetComponent<SkeletonArcher>()){
            collision.gameObject.GetComponent<SkeletonArcher>().TakeDamage(3, "Normal");
        }
    }

}