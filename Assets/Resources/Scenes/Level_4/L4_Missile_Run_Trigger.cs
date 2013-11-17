using UnityEngine;
using System.Collections;

public class L4_Missile_Run_Trigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider col)
	{
		print ("Missile Run Start");
	}
}
