using UnityEngine;
using System.Collections;

/** handles looking at the capital ship */
public class L4_Camera : MonoBehaviour {
	
	private int lookatState = 0;
	private Quaternion finalRot;
	
	//these aren't defaults, they get immediately overwritten
	// by LookAtCapShip(). Set them in the spawner.
	float startlooktime = .75f;
	float lingertime = 4f;
	float lookbacktime = .5f;
	
	public float statestarttime = 0;
	
	// Use this for initialization
	void Start () {
		finalRot = Quaternion.AngleAxis(75,new Vector3(0,1,0));
		//Quaternion.LookRotation(
	}
	
	// Update is called once per frame
	void Update () {
	
		switch (lookatState)
		{
		// 
		case 0:
			this.transform.rotation = Quaternion.identity;
			break;
		case 1:
			this.transform.rotation = Quaternion.Lerp(Quaternion.identity, finalRot, (Time.time-statestarttime)/startlooktime);
			if (Time.time > statestarttime + startlooktime) { lookatState = 2; statestarttime = Time.time; }
			break;
		case 2:
			if (Time.time > statestarttime + lingertime) { lookatState = 3; statestarttime = Time.time; }
			break;
		case 3:
			this.transform.rotation = Quaternion.Lerp(finalRot, Quaternion.identity, (Time.time-statestarttime)/lookbacktime);
			if (Time.time > statestarttime + lookbacktime) { lookatState = 0; statestarttime = Time.time; }
			break;
		}
		
	}
	
	
	public void LookAtCapShip(float transitionInTime, float ponderTime, float)
	{
		lookatState = 1;
		statestarttime = Time.time;
	}
	
	
}
