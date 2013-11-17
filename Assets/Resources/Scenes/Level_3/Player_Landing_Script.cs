using UnityEngine;
using System.Collections;

public class Player_Landing_Script : MonoBehaviour {

    private bool hasPlayed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other)
    {
        if (!hasPlayed && other.gameObject.CompareTag("Player"))
        {
            audio.Play();
            hasPlayed = true;
        }
    }
}
