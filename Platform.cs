using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
 
    public Vector2 startPos;
    public bool movingRight;

    public float dist = 5f;

    void Start(){
        startPos = this.transform.position;
        movingRight = true;
    }

    void FixedUpdate(){
        if (movingRight){
            if (this.transform.position.x >= startPos.x + dist){
                movingRight = false;
                return;
            }

            this.transform.Translate(new Vector2(2f * Time.deltaTime, 0f));
        } else {
            if (this.transform.position.x <= startPos.x){
                movingRight = true;
                return;
            }

            this.transform.Translate(new Vector2(-2f * Time.deltaTime, 0f));
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        collision.gameObject.transform.parent = this.transform;
    }

    void OnCollisionExit2D(Collision2D collision) {
        collision.gameObject.transform.parent = null;
    }

}