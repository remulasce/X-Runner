using UnityEngine;
using System.Collections;

public class Ship_Scrape_Script : MonoBehaviour {

    public bool isPlayed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (!isPlayed && this.rigidbody.velocity.magnitude > 1.5f)
        {
            audio.Play();
            isPlayed = true;
        }
	}
}
