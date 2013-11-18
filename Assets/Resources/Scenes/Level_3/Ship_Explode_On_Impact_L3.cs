using UnityEngine;
using System.Collections;

public class Ship_Explode_On_Impact_L3 : MonoBehaviour {

    AudioSource[] audios;

	// Use this for initialization
	void Start () {
        audios = this.gameObject.GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("L3_Ejection_Trigger"))
        {
            this.GetComponent<Detonator>().Explode();
            this.gameObject.renderer.enabled = false;
            audio.Stop();
        }        
    }

    void PlayEjection()
    {
        audios[1].Play();
    }
}
