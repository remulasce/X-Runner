using UnityEngine;
using System.Collections;

public class Ship_Scrape_Script : MonoBehaviour {

    public bool isPlayed = false;
    public float velocityMagnitude = 3.0f;

    private float timeToWait = 5.0f;
    private bool waitBool = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!isPlayed && !waitBool && this.rigidbody.velocity.magnitude > velocityMagnitude)
        {
            audio.Play();
            isPlayed = true;
        }
	}

    IEnumerator checkForSlide()
    {
        yield return new WaitForSeconds(timeToWait);
        waitBool = false;
    }
}
