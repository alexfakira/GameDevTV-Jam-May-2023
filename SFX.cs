using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour {
    
    AudioSource audioSource;

    void Start (){
        audioSource = GetComponent<AudioSource>();

        if (!audioSource.loop){
            Destroy(gameObject, audioSource.clip.length);
        }
    }

}