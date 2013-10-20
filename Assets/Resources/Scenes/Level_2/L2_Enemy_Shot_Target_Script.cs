using UnityEngine;
using System.Collections;


public class L2_Enemy_Shot_Target_Script : L2_Enemy_Shot_Script
{
    protected GameObject player;

	// Use this for initialization
	protected void Start () {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        this.transform.LookAt(player.transform.position);
        this.rigidbody.velocity = this.transform.forward * speed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
