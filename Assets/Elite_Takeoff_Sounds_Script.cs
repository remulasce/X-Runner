using UnityEngine;
using System.Collections;

public class Elite_Takeoff_Sounds_Script : MonoBehaviour {

    AudioSource[] audios;

	// Use this for initialization
	void Start () {
        audios = this.gameObject.GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void PlayStartUp()
    {
        audios[0].Play();
    }

    void PlayIdle()
    {
        audios[0].Stop();
        audios[1].Play();
    }

    void PlayTakeOff()
    {
        audios[1].Stop();
        audios[2].Play();
    }
}
