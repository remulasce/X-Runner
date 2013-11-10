using UnityEngine;
using System.Collections;

public class Jetpack_powerUp_Script : MonoBehaviour {

    public float jetPackSpeed = 9.0f;
    public Color jetPackNewColor = Color.white;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player_Movement_Script>().isJetPackActive = true;
            other.gameObject.GetComponentInChildren<ParticleSystem>().startSpeed = jetPackSpeed * Mathf.Sign(other.gameObject.GetComponentInChildren<ParticleSystem>().startSpeed);
            other.gameObject.GetComponentInChildren<ParticleSystem>().startColor = jetPackNewColor; // Set the default color to something a bit more sci-fi ish            
            Object.Destroy(this.gameObject);
        }
    }
}
