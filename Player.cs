using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public Rigidbody2D rb;
    public PolygonCollider2D attack_area;
    public Animator anim;
    public SpriteRenderer sr;
    public CapsuleCollider2D cc;

    public GameObject death_particles;
    public GameObject cam_holder;
    public Camera cam;
    public GameObject[] orbs;
    public TMP_Text score_text;
    public TMP_Text death_text;
    public TMP_Text lives_remaining_text;
    public Image hp_1, hp_2, hp_3, hp_4, hp_5;
    public Image flame_1, flame_2, flame_3;
    public GameObject fireball;

    public Vector2 start_pos;
    public Vector2 respawn_point;

    public bool isAlive = true;
    public bool isInverted = false;
    public bool isFacingRight = true;
    public bool isGrounded = false;
    public bool isAttacking = false;
    public bool isSpedup = false;

    public bool movingLeft = false;
    public bool movingRight = false;
    public bool jumping = false;
    public bool airMovingLeft = false;
    public bool airMovingRight = false;

    public float speedMultiplier = 1f;
    public float speeduptime = 3f;
    public float time_since_spedup = 0f;
    public float time_to_attack = 0.7f;
    public float time_last_attacked = 0f;

    public int score = 0;
    public int lives = 3;
    public int hp = 5;
    public int firepower = 0;

    void Start(){
        cam = cam_holder.GetComponentInChildren<Camera>();

        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        attack_area = this.GetComponentInChildren<PolygonCollider2D>();

        orbs = GameObject.FindGameObjectsWithTag("Orb");

        start_pos = new Vector3(-9.01f, -1.98f, -4.4f);
        respawn_point = start_pos;
    }

    void Update(){
        cam_holder.transform.position = this.transform.position;
        cam_holder.transform.position += new Vector3(0f, 0f, -8.0f);

        GetInput();
    }

    void GetInput(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("Menu");
        }

        anim.SetFloat("Speed", 0f);
        anim.ResetTrigger("Jumped");

        if (isAttacking){
            if (time_last_attacked > time_to_attack){
                isAttacking = false;
                attack_area.enabled = false;
                anim.SetBool("Attacking", false);
                anim.SetBool("Casting", false);
                time_last_attacked = 0;

                return;
            }

            time_last_attacked += 1 * Time.deltaTime;

            return;
        }

        if (isSpedup){
            if (time_since_spedup > speeduptime){
                isSpedup = false;
                speedMultiplier = 1f;
                time_since_spedup = 0;
            } else {
                time_since_spedup += 1 * Time.deltaTime;
            }
        }

        if (!isAlive){
            return;
        }

        if (isGrounded){
            if (Input.GetKey(KeyCode.A)){
                movingLeft = true;
                anim.SetFloat("Speed", 1f);

                if (isFacingRight){
                    this.transform.localScale *= new Vector2(-1, 1);

                    isFacingRight = false;
                }
            } else if (Input.GetKey(KeyCode.D)){
                movingRight = true;
                anim.SetFloat("Speed", 1f);

                if (!isFacingRight){
                    this.transform.localScale *= new Vector2(-1, 1);

                    isFacingRight = true;
                }
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)){
                ReduceXVelocity(5);
            }

            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 7 * speedMultiplier);

            if (Input.GetKeyDown(KeyCode.Space)){
                jumping = true;

                if (isSpedup){
                    FindAnyObjectByType<AudioManager>().PlaySFX("Boosted_Jump", gameObject);
                }
            }

            if (Input.GetKeyDown(KeyCode.F)){
                ReduceXVelocity(1.1f);
                isAttacking = true;
                attack_area.enabled = true;
                anim.SetBool("Attacking", true);
                FindAnyObjectByType<AudioManager>().PlaySFX("Katana_Slice", gameObject);
            }

            if (Input.GetKeyDown(KeyCode.Q) && firepower > 0){
                ReduceXVelocity(3.0f);
                isAttacking = true;
                anim.SetBool("Casting", true);
                firepower--;

                StartCoroutine(Cast());
            }
        } else {
            if (Input.GetKey(KeyCode.A)){
                airMovingLeft = true;
            } else if (Input.GetKey(KeyCode.D)){
                airMovingRight = true;
            }
        }
    }

    void FixedUpdate(){
        if (movingLeft){ 
            rb.AddForce(new Vector2(-25f * speedMultiplier, 0f));

            movingLeft = false;
        } else if (movingRight){
            rb.AddForce(new Vector2(25f * speedMultiplier, 0f));

            movingRight = false;
        }

        if (jumping){
            if (!isInverted){
                rb.AddForce(new Vector2(0f, 7.0f), ForceMode2D.Impulse);
            } else {
                rb.AddForce(new Vector2(0f, -7.0f), ForceMode2D.Impulse);
            }

            anim.SetTrigger("Jumped");

            jumping = false;
        }

        if (airMovingLeft){
            rb.AddForce(new Vector2(-0.8f * speedMultiplier, 0f));

            airMovingLeft = false;
        } else if (airMovingRight){
            rb.AddForce(new Vector2(0.8f * speedMultiplier, 0f));

            airMovingRight = false;
        }
    }

    public void GiveFirePower(){
        firepower = 3;

        Color opaque = new Color(1f, 1f, 1f, 1f);

        flame_1.color = opaque;
        flame_2.color = opaque;
        flame_3.color = opaque;
    }

    void ReduceXVelocity(float amt){
        rb.velocity = new Vector2(rb.velocity.x / amt, rb.velocity.y);
    }

    public void AddScore(int score){
        this.score += score;

        score_text.text = "Score: " + this.score;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.TryGetComponent<Ground>(out Ground ground)){
            float playerPosY = transform.position.y;
            float groundPosY;

            if (!isInverted){
                groundPosY = ground.transform.position.y + ground.transform.localScale.y / 2 - 0.1f;

                if (playerPosY < groundPosY) {
                    return;
                }
            } else {
                groundPosY = ground.transform.position.y - ground.transform.localScale.y / 2 + 0.1f;

                if (playerPosY > groundPosY) {
                    return;
                }
            }

            isGrounded = true;
            anim.SetBool("Grounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.TryGetComponent<Ground>(out Ground ground)){
            float playerPosY = transform.position.y;
            float groundPosY;
            
            if (!isInverted){
                groundPosY = ground.transform.position.y + ground.transform.localScale.y / 2;
            
                if (playerPosY < groundPosY){
                    return;    
                }
            } else {
                groundPosY = ground.transform.position.y - ground.transform.localScale.y / 2;
            
                if (playerPosY > groundPosY){
                    return;
                }
            }

            isGrounded = false;
            anim.SetBool("Grounded", false);
        }
    }

    public void SwitchGravity(){
        Physics2D.gravity *= -1;
        isGrounded = false;
        anim.SetBool("Grounded", false);

        this.transform.localScale *= new Vector2(1, -1);

        if (isInverted){
            isInverted = false;
            transform.Translate(new Vector2(0f, -1.35f));
        } else {
            isInverted = true;
            transform.Translate(new Vector2(0f, 1.35f));
        }

        if (cam.backgroundColor == Color.white) {
            cam.backgroundColor = Color.black;
        } else {
            cam.backgroundColor = Color.white;
        }

        GameObject[] inverts = GameObject.FindGameObjectsWithTag("Invert");

        /*ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();

        foreach (ParticleSystem particleSystem in particleSystems){
            Destroy(particleSystem);
        }*/

        for (int i = 0; i < inverts.Length; i++){
            SpriteRenderer srw = inverts[i].GetComponent<SpriteRenderer>();

            if (srw.color == Color.white) {
                srw.color = Color.black;
            } else {
                srw.color = Color.white;
            }

            if (srw.gameObject.GetComponent<Zombie>() || srw.gameObject.GetComponent<SkeletonArcher>() || srw.gameObject.GetComponent<Slime>()){
                if (srw.GetComponent<Zombie>()){
                    if (!srw.GetComponent<Zombie>().isAlive){
                        Destroy(srw.gameObject);
                    }
                } else if (srw.GetComponent<SkeletonArcher>()){
                    if (!srw.GetComponent<SkeletonArcher>().isAlive){
                        Destroy(srw.gameObject);
                    }
                }
                
                srw.gameObject.transform.localScale *= new Vector2(1, -1);
            }
        }

        if (sr.color == Color.white){
            sr.color = Color.black;
        } else {
            sr.color = Color.white;
        }
    }

    IEnumerator Die(string damageType){
        lives--;

        isAlive = false;
        rb.velocity = new Vector2(0f, rb.velocity.y);

        Color transparent = new Color(0f, 0f, 0f, 0f);
        Color opaque = new Color(1f, 1f, 1f, 1f);

        death_text.color = opaque;
        lives_remaining_text.color = opaque;

        flame_1.color = transparent;
        flame_2.color = transparent;
        flame_3.color = transparent;

        lives_remaining_text.text = lives + " lives remaining";

        switch (damageType){
            case "Explosive":
                sr.enabled = false;
                cc.enabled = false;
                rb.bodyType = RigidbodyType2D.Static;

                var main = death_particles.GetComponent<ParticleSystem>().main;

                if (!isInverted){
                    main.startColor = Color.white;
                } else {
                    main.startColor = Color.black;
                }

                GameObject death_particles_ = Instantiate(death_particles);
                death_particles_.transform.position = this.transform.position;
                Destroy(death_particles_, 3);
                break;
            case "Normal":
                anim.SetBool("Dead", true);
                FindAnyObjectByType<AudioManager>().PlaySFX("Player_Death", gameObject);
                break;
            default:
                break;
        }
           
        yield return new WaitForSeconds(2f);

        if (lives > 0){
            if (score > 0){
                float score_ = score;
                score_ /= 3;
                Math.Ceiling(score_);
                score = (int)score_;
                score_text.text = "Score: " + this.score;
            }

            ResetHP();

            foreach (GameObject orb in orbs){
                if (orb.GetComponent<Gravity>()){
                    if (!orb.GetComponent<Gravity>().can_respawn){
                        continue;
                    }
                }

                if (!orb.activeSelf){
                    orb.SetActive(true);
                }
            }

            if (isInverted){
                SwitchGravity();
            } else {
                sr.color = Color.white;
            }

            this.transform.position = respawn_point;

            sr.enabled = true;
            cc.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = Vector2.zero;
            isAlive = true;
            isGrounded = false;
            firepower = 0;
            anim.SetBool("Dead", false);
            anim.SetBool("Grounded", false);
            death_text.color = transparent;
            lives_remaining_text.color = transparent;
        } else {
            SceneManager.LoadScene("Menu");
        }
    }

    IEnumerator Cast(){
        yield return new WaitForSeconds(0.7f);

        GameObject fireball_ = Instantiate(fireball);

        Vector2 direction;
        Vector3 offset;

        if (isFacingRight){ 
            offset = new Vector3(0.7f, 0f, 0f);
            direction = Vector2.right;
        } else {
            fireball_.transform.localScale *= new Vector2(-1, 1);
            offset = new Vector3(-0.7f, 0f, 0f);
            direction = Vector2.left;
        }
                
        fireball_.transform.position = this.transform.position;
        fireball_.transform.position += offset;
        fireball_.GetComponent<Fireball>().direction = direction;

        if (isInverted){
            fireball_.GetComponent<SpriteRenderer>().color = Color.black;
            offset = new Vector3(0f, -0.6f, 0f);
        } else {
            fireball_.GetComponent<SpriteRenderer>().color = Color.white;
            offset = new Vector3(0f, 0.6f, 0f);
        }

        fireball_.transform.position += offset;

        direction.Normalize();

        Color transparent = new Color(1f, 1f, 1f, 0f);
        Color opaque = new Color(1f, 1f, 1f, 1f);

        flame_1.color = transparent;
        flame_2.color = transparent;
        flame_3.color = transparent;

        switch (firepower){
            case 3:
                flame_3.color = opaque;
                goto case 2;
            case 2:
                flame_2.color = opaque;
                goto case 1;
            case 1:
                flame_1.color = opaque;
                break;
            default:
                break;
        }
    }
    
    public void TakeDamage(int dmg, string damageType){
        if (!isAlive){
            return;
        }

        hp -= dmg;

        speedMultiplier = 1;

        Color transparent = new Color(1f, 1f, 1f, 0f);
        Color opaque = new Color(1f, 1f, 1f, 1f);

        hp_1.color = transparent;
        hp_2.color = transparent;
        hp_3.color = transparent;
        hp_4.color = transparent;
        hp_5.color = transparent;

        switch (hp){
            case 5:
                hp_5.color = opaque;
                goto case 4;
            case 4:
                hp_4.color = opaque;
                goto case 3;
            case 3:
                hp_3.color = opaque;
                goto case 2;
            case 2:
                hp_2.color = opaque;
                goto case 1;
            case 1:
                hp_1.color = opaque;
                FindAnyObjectByType<AudioManager>().PlaySFX("Player_Hurt", gameObject);
                StartCoroutine(CameraShake());
                break;
            default:
                StartCoroutine(Die(damageType));
                break;
        }
    }

    public void SetRespawnPoint(Vector2 respawn_point){
        this.respawn_point = respawn_point;
    }

    public void ResetHP(){
        hp = 5;

        Color opaque = new Color(1f, 1f, 1f, 1f);

        hp_1.color = opaque;
        hp_2.color = opaque;
        hp_3.color = opaque;
        hp_4.color = opaque;
        hp_5.color = opaque;
    }

    public void Speedup(){
        isSpedup = true;
        speedMultiplier = 2f;
    }

    IEnumerator CameraShake(){
        Vector3 originalPos = cam.transform.localPosition;

        float elapsed = 0f;
        float duration = 0.1f;
        float magnitute = 0.1f;

        while (elapsed < duration){
            float x = UnityEngine.Random.Range(-1, 1) * magnitute;
            float y = UnityEngine.Random.Range(-1, 1) * magnitute;
            float z = cam.transform.position.z;

            cam.transform.localPosition = new Vector3(x, y, z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cam.transform.localPosition = originalPos;
    }

}