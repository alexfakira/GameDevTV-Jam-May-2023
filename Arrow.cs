using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public Rigidbody2D rb;

    public Vector2 direction;

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.GetComponent<Ground>()){
            Destroy(gameObject);
        } else if (collision.GetComponent<Player>()){
            Vector2 direction = collision.transform.position - transform.position;
            direction.Normalize();
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponent<Rigidbody2D>().AddForce(direction * 12f, ForceMode2D.Impulse);

            if (!collision.GetComponent<Player>().isGrounded) {
                if (collision.transform.position.x < this.transform.position.x) {
                    collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(-4f, 0f), ForceMode2D.Impulse);
                } else {
                    collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f, 0f), ForceMode2D.Impulse);
                }
            }

            collision.GetComponent<Player>().TakeDamage(1, "Normal");

            Destroy(gameObject);
        }
    }

}