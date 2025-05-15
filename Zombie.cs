using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Zombie : MonoBehaviour {

    public Player player;
    public Animator anim;
    public GameObject death_particles;
    public SpriteRenderer sr;

    public bool isAlive;
    public bool isFacingRight;
    public bool isAttacking;
    public float time_last_attacked;
    public float time_to_attack = 0.8f;
    public float attack_cooldown = 2f;
    public float attack_cooldown_timer;

    public int hp = 3;

    void Start(){
        player = FindObjectOfType<Player>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        time_last_attacked = 0f;
        attack_cooldown_timer = attack_cooldown;
        isAlive = true;
        isAttacking = false;
        isFacingRight = true;
    }
    
    void Update(){
        anim.SetFloat("Speed", 0f);
 
        if (!isAlive){
            return;
        }

        if (attack_cooldown_timer < attack_cooldown){
            attack_cooldown_timer += 1f * Time.deltaTime;
        }

        if (isAttacking){
            if (time_last_attacked > time_to_attack){
                anim.SetBool("Attacking", false);
                isAttacking = false;
            }

            time_last_attacked += 1 * Time.deltaTime;
        } else {
            if (player.isAlive){
                if (Vector2.Distance(this.transform.position, player.transform.position) <= 8f && !isAttacking){
                    if (Vector2.Distance(this.transform.position, player.transform.position) <= 0.4f){
                        if (attack_cooldown_timer >= attack_cooldown){
                            Attack();
                        }
                    }

                    if (player.transform.position.x < this.transform.position.x){
                        if (isFacingRight){
                            this.transform.localScale *= new Vector2(-1, 1);
                            isFacingRight = false;
                        }

                        anim.SetFloat("Speed", 1f);
                        this.transform.Translate(new Vector2(-1.5f * Time.deltaTime, 0f));
                    } else {
                        if (!isFacingRight){
                            this.transform.localScale *= new Vector2(-1, 1);
                            isFacingRight = true;
                        }

                        anim.SetFloat("Speed", 1f);
                        this.transform.Translate(new Vector2(1.5f * Time.deltaTime, 0f));
                    }
                }
            }
        }
    }

    void Attack(){
        time_last_attacked = 0;
        attack_cooldown_timer = 0;
        isAttacking = true;
        anim.SetBool("Attacking", true);
        FindAnyObjectByType<AudioManager>().PlaySFX("Zombie_Attack", gameObject);
    }

    public void TakeDamage(int dmg, string damageType){
        hp -= dmg;

        if (hp <= 0){
            Die(damageType);

            return;
        }

        Vector2 direction = transform.position - player.transform.position;
        direction.Normalize();
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.GetComponent<Rigidbody2D>().AddForce(direction * 7f, ForceMode2D.Impulse);
    }

    public void Die(string damageType){
        player.AddScore(20);

        switch (damageType){
            case "Explosive":
                var main = death_particles.GetComponent<ParticleSystem>().main;

                if (!player.isInverted){
                    main.startColor = Color.white;
                } else {
                    main.startColor = Color.black;
                }

                GameObject death_particles_ = Instantiate(death_particles);
                death_particles_.transform.position = this.transform.position;
                Destroy(death_particles_, 3);
                Destroy(this.gameObject);
                break;
            case "Normal":
                Destroy(this.GetComponent<Rigidbody2D>());
                Destroy(this.GetComponent<CapsuleCollider2D>());
                isAlive = false;
                anim.SetBool("Dead", true);
                FindAnyObjectByType<AudioManager>().PlaySFX("Zombie_Death", gameObject);
                StartCoroutine(Despawn());
                break;
            default:
                break;
        }
    }

    IEnumerator Despawn(){
        yield return new WaitForSeconds(5f);

        /*for (int i = 255; i >= 0; i+= 8){
            sr.color = new Color(player.sr.color.r, player.sr.color.g, player.sr.color.b, i);

            yield return null;
        }*/

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (!isAlive || !player.isAlive){
            return;
        }

        if (collision.gameObject.name == "Player"){
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Rigidbody2D>().AddForce(direction * 7f, ForceMode2D.Impulse);

            if (!player.isGrounded) {
                if (player.transform.position.x < this.transform.position.x) {
                    player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3f, 0f), ForceMode2D.Impulse);
                } else {
                    player.GetComponent<Rigidbody2D>().AddForce(new Vector2(3f, 0f), ForceMode2D.Impulse);
                }
            }

            player.GetComponent<Player>().TakeDamage(1, "Normal");
        }
    }

}