using UnityEngine;
using System.Collections;

public class Crashing_Ship_Sound_Script : MonoBehaviour {

    AudioSource[] audios;

	// Use this for initialization
	void Start () {
        audios = this.gameObject.GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void PlayIdle()
    {
        audios[0].Play();
    }

    void PlayCrash()
    {
        audios[0].Stop();
        audios[1].Play();
    }
}
