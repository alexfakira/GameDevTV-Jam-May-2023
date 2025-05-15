using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Ball : MonoBehaviour {

    public GameObject particles;
    public Vector2 direction;

    void Update(){
        transform.Translate(direction * 4 * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.name == "Player"){
            Vector2 direction = collision.gameObject.transform.position - transform.position;
            direction.Normalize();
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            
            if (!collision.gameObject.GetComponent<Player>().isInverted){
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0f, 3f, 0f), ForceMode2D.Impulse);    
            } else {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0f, -3f, 0f), ForceMode2D.Impulse);
            }

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * 7f, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<Player>().TakeDamage(1, "Explosive");
            explode(collision);
        }

        if (collision.gameObject.TryGetComponent<Ground>(out Ground ground)){
            explode(collision);
        }
    }

    void explode(Collision2D collision){
        var main = particles.GetComponent<ParticleSystem>().main;
            
        if (collision.gameObject.GetComponent<SpriteRenderer>().color == Color.white){ 
            main.startColor = Color.white;
        } else {
            main.startColor = Color.black;
        }
            
        GameObject particles_ = Instantiate(particles);
        particles_.transform.position = this.transform.position;
        Destroy(particles_, 5);
        Destroy(gameObject);
    }

}