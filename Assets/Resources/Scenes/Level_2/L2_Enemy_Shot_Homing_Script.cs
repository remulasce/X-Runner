using UnityEngine;
using System.Collections;

public class L2_Enemy_Shot_Homing_Script : L2_Enemy_Shot_Target_Script
{

	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.LookAt(player.transform.position);
        this.rigidbody.velocity = this.transform.forward * speed;
	}
}
