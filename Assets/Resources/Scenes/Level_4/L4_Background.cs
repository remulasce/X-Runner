﻿using UnityEngine;
using System.Collections;


/** This should be attached to a Background empty GameObject, which will
 * be the parent of the entire background.
 * 
 * We're going to keep the player ship in place and move the entire scenery
 * around the player, so we can copy-paste from L2 and not deal with everything
 * moving.
 * 
 * */
public class L4_Background : MonoBehaviour {
	
	public float speed = 4;
	public bool started = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// We move the base Background object to move everything that is a child of it.
	void Update () {
		if (started)
		{
			this.transform.Translate(-speed * Time.deltaTime, 0, 0);
		}
	}
	
	public void StartTrench()
	{
		started = true;
		//this.transform.position = 
	}
}
