using UnityEngine;
using System.Collections;

public class Audio_SFX_Object_Script : MonoBehaviour {

    private bool isSoundPlayed = false;

    AudioSource sfx;

	// Use this for initialization
	void Start () {
        //this.sfx = this.gameObject.GetComponent<AudioSource>();        
	}
	
	// Update is called once per frame
	void Update () {
        if (isSoundPlayed && !this.gameObject.GetComponent<AudioSource>().isPlaying)
        {
            Object.Destroy(this.gameObject);
        }
	}

    public void StartSound(AudioSource s)
    {        
        this.sfx = this.gameObject.GetComponent<AudioSource>();
        sfx.clip = s.clip;
        sfx.pitch = s.pitch;
        sfx.volume = s.volume;
        sfx.maxDistance = s.maxDistance;
        sfx.rolloffMode = s.rolloffMode;
        sfx.minDistance = s.minDistance;
        sfx.priority = s.priority;
        sfx.spread = s.spread;
        sfx.loop = s.loop;
        sfx.Play();
        isSoundPlayed = true;             
    }
}
