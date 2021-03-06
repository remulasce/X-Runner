﻿using UnityEngine;
using System.Collections;

public class Window_Ship_Script : MonoBehaviour {

    public GameObject objectToInteractWith; // Will be used to link a destructible platform to it ONLY, since this animation will be unique

    private bool shipCrashed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!animation.isPlaying && !shipCrashed)
        {
            objectToInteractWith.GetComponent<Destructive_Platform_Script>().ApplyStagedForce();
            this.transform.position = Vector3.zero;
            shipCrashed = true;
        }
	}
}
