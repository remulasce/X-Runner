﻿using UnityEngine;
using System.Collections;

public class L1_WinTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			//Application.LoadLevel("Level_2_TDS");
			this.transform.parent.animation.Play();
		}
	}
	
}
