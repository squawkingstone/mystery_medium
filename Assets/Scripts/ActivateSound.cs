using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSound : MonoBehaviour {

    AudioSource music;
	// Use this for initialization
	void Start () {
        music = this.GetComponent<AudioSource>();
	}


    public void activate(int value)
    {
        if(value == 0)
        {
            if (music.isPlaying)
            {
                music.Pause();
            }
        }
        else
        {
            if (!music.isPlaying)
            {
                music.Play();
            }
        }
    }
}
