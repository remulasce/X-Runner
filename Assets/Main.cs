using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		//Check and handle previous level's remains
		print (LoadHandler.level0Camera);
		LoadHandler.level0Camera.GetComponent<AudioListener>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
