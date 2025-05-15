using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    public Player player;
    public GameObject particles;
    public GameObject fireball_explode_area;
    public Vector2 direction;

    void Start (){
        FindAnyObjectByType<AudioManager>().PlaySFX("Fireball_Cast", gameObject);
    }

    void Update(){
        transform.Translate(direction * 12 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.GetComponent<Slime>() || collision.gameObject.GetComponent<Zombie>() || collision.gameObject.GetComponent<SkeletonArcher>()){
            if (collision.gameObject.GetComponent<Slime>()){
                //collision.gameObject.GetComponent<Player>().TakeDamage(1, "Explosive");
            } else if (collision.gameObject.GetComponent<Zombie>()){
                collision.gameObject.GetComponent<Zombie>().TakeDamage(2, "Explosive");
            }  else if (collision.gameObject.GetComponent<SkeletonArcher>()){
                collision.gameObject.GetComponent<SkeletonArcher>().TakeDamage(2, "Explosive");
            }

            Explode(collision);
        }

        if (collision.gameObject.TryGetComponent<Ground>(out Ground ground)){
            Explode(collision);
        }
    }

    void Explode(Collider2D collision){
        var main = particles.GetComponent<ParticleSystem>().main;

        if (collision.gameObject.GetComponent<SpriteRenderer>().color == Color.white) {
            main.startColor = Color.white;
        } else {
            main.startColor = Color.black;
        }

        GameObject fireball_explode_area_ = Instantiate(fireball_explode_area);
        fireball_explode_area_.transform.position = this.transform.position;

        GameObject particles_ = Instantiate(particles);
        particles_.transform.position = this.transform.position;

        if (direction.x > 0){
            particles_.transform.position += new Vector3(-0.07f, 0f, 0f);
        } else {
            particles_.transform.position += new Vector3(0.07f, 0f, 0f);
        }

        Destroy(particles_, 5);

        Destroy(gameObject);

    }

}