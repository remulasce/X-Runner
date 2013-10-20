using UnityEngine;
using System.Collections;

public class L2_Enemy_Shot_Homing_Script : L2_Enemy_Shot_Target_Script
{

	// Use this for initialization
	protected new void Start () {
        base.Start();
	}
	
	// Update is called once per frame
    protected new void Update()
    {
        base.Update();
        if (!player.GetComponent<L2_Ship_Script>().isDead) // Follow the player if it has not been destroyed yet
        {
            this.transform.LookAt(player.transform.position);
            this.rigidbody.velocity = this.transform.forward * speed;
        }
	}
}
