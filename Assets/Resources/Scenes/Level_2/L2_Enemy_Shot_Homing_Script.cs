﻿using UnityEngine;
using System.Collections;

public class L2_Enemy_Shot_Homing_Script : L2_Enemy_Shot_Target_Script
{

    // Detonator
    private Detonator explosion;

	// Use this for initialization
	protected new void Start () {
        base.Start();
        explosion = this.GetComponentInChildren<Detonator>();
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
		//Stop following if he dies
		else
		{
            explosion.transform.parent = null;
            explosion.transform.position = this.transform.position;
            explosion.Explode();
            Destroy (this.gameObject);
		}
	}

    protected void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("L2_PlayerShot"))
        {
            explosion.transform.parent = null;
            explosion.transform.position = this.transform.position;
            explosion.Explode();
            Destroy(col.gameObject);
            Destroy(this.gameObject);
        }
    }

    public Detonator GetExplosion()
    {
        return explosion;
    }
}
