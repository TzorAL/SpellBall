using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Object.DontDestroyOnLoad example.
//
// This script example manages the playing audio. The GameObject with the
// "music" tag is the BackgroundMusic GameObject. The AudioSource has the
// audio attached to the AudioClip.

public class DontDestroy : MonoBehaviour
{
    public GameObject audio;
    static bool created = false;

    void Start(){
        audio = this.gameObject;

        audio.GetComponent<AudioSource>().Play();
 
    }
    void Awake() {
    
        if (!created) {
            DontDestroyOnLoad (audio);
            created = true;
        } 
        else {
            Destroy (audio);
        }
    }
}
