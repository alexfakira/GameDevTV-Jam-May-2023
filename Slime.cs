using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {

    public Player player;
    public Animator anim;
    public GameObject slime_ball;

    public bool isAlive;
    public bool isFacingRight;
    public bool isAttacking;
    public float time_dead;
    public float death_time = 0.5f;
    public float time_last_attacked;
    public float time_to_attack = 0.5f;
    public float attack_cooldown = 3.0f;
    
    void Start(){
        player = FindObjectOfType<Player>();
        anim = GetComponent<Animator>();

        time_dead = 0;
        time_last_attacked = 0f;
        isAlive = true;
        isAttacking = false;
        isFacingRight = false;
    }

    void Update(){
        if (!isAlive){
            time_dead += 1 * Time.deltaTime;

            if (time_dead > death_time){
                this.gameObject.SetActive(false);
            }

            return;    
        }

        if (Vector2.Distance(this.transform.position, player.transform.position) <= 8f){
            if (player.transform.position.x < this.transform.position.x){
                if (isFacingRight){
                    this.transform.localScale *= new Vector2(-1, 1);

                    isFacingRight = false;
                }
            } else {
                if (!isFacingRight){
                    this.transform.localScale *= new Vector2(-1, 1);

                    isFacingRight = true;
                }
            }

            Attack();
        }

        if (isAttacking){
            if (time_last_attacked > time_to_attack){
                anim.SetBool("Attacking", false);
            }

            if (time_last_attacked > attack_cooldown){
                isAttacking = false;
                time_last_attacked = 0;

                return;
            }

            time_last_attacked += 1 * Time.deltaTime;
        }
    }

    void Attack(){
        if (isAttacking || !player.isAlive){
            return;
        }

        Vector2 direction = player.transform.position - this.transform.position;
        direction.Normalize();
        GameObject slime_ball_ = Instantiate(slime_ball);
        Vector3 offset = new Vector3(0f, 0.2f, 0f);
        slime_ball_.transform.position = this.transform.position;
        slime_ball_.transform.position += offset;
        slime_ball_.GetComponent<Slime_Ball>().direction = direction;

        if (player.isInverted){
            slime_ball_.GetComponent<SpriteRenderer>().color = Color.black;
        }

        isAttacking = true;
        anim.SetBool("Attacking", true);
    }

    public void Die(){
        isAlive = false;
        player.AddScore(10);
        anim.SetBool("Alive", false);
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (!isAlive){
            return;    
        }

        if (collision.gameObject.name == "Player"){
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Rigidbody2D>().AddForce(direction * 7f, ForceMode2D.Impulse);
        
            player.GetComponent<Player>().TakeDamage(1, "Normal");
        }
    }

}