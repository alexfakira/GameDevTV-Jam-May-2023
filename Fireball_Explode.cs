using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_Explode : MonoBehaviour {

    public Player player;

    private void Start(){
        FindAnyObjectByType<AudioManager>().PlaySFX("Fireball_Hit", gameObject);
        StartCoroutine(EndExplosion());
    }

    IEnumerator EndExplosion(){
        yield return new WaitForSeconds(0.2f);

        this.GetComponent<CircleCollider2D>().enabled = false;

        yield return new WaitForSeconds(1.0f);

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision){
        Vector2 direction = collision.gameObject.transform.position - transform.position;
        direction.Normalize();
        collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (collision.gameObject.GetComponent<Arrow>()){
            Destroy(collision.gameObject);
        }
           
        if (!player.isInverted){
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0f, 3f, 0f), ForceMode2D.Impulse);    
        } else {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0f, -3f, 0f), ForceMode2D.Impulse);
        }

        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * 7f, ForceMode2D.Impulse);

        if (collision.gameObject.GetComponent<Zombie>()){
            collision.gameObject.GetComponent<Zombie>().TakeDamage(5, "Explosive");
        }

        if (collision.gameObject.GetComponent<SkeletonArcher>()){
            collision.gameObject.GetComponent<SkeletonArcher>().TakeDamage(5, "Explosive");
        }

        if (collision.gameObject.GetComponent<Player>()){
            collision.gameObject.GetComponent<Player>().TakeDamage(2, "Explosive");
        }
    }

}