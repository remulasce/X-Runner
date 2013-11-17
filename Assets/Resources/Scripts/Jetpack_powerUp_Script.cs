using UnityEngine;
using System.Collections;

public class Jetpack_powerUp_Script : MonoBehaviour {

    public float jetPackSpeed = 9.0f;
    public Color jetPackNewColor = Color.white;

    private bool soundHasPlayed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (soundHasPlayed && !audio.isPlaying)
        {
            Object.Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !soundHasPlayed)
        {
            audio.Play();
            other.gameObject.GetComponent<Player_Movement_Script>().isJetPackActive = true;
            other.gameObject.GetComponentInChildren<ParticleSystem>().startSpeed = jetPackSpeed * Mathf.Sign(other.gameObject.GetComponentInChildren<ParticleSystem>().startSpeed);
            other.gameObject.GetComponentInChildren<ParticleSystem>().startColor = jetPackNewColor; // Set the default color to something a bit more sci-fi ish              
            soundHasPlayed = true;
            this.renderer.enabled = false;
        }
    }
}
