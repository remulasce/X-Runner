using UnityEngine;
using System.Collections;

public class L2_Enemy_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	
	void killIfOutBounds()
	{
		if (Mathf.Abs(this.transform.position.magnitude) > 100)
		{
			Destroy(this);
		}
	}
	
	// Update is called once per frame
	void Update () {
		killIfOutBounds();
	}
	
	//When we get hit by a player shot, we should die.
	//Leave killing the shot to the shot itself, in case
	//	we make stuff that can go through things
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("L2_PlayerShot"))
		{
			Destroy(this.gameObject);
		}
			
	}
}
