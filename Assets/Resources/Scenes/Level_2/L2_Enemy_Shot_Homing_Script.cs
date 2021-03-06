﻿using UnityEngine;
using System.Collections;

public class L2_Enemy_Shot_Homing_Script : L2_Enemy_Shot_Target_Script
{
	public float homingRotationSpeed = 10;
	
    // Detonator
    private Detonator explosion;

    private IPlayer player;

    public bool isDestroyed = false;

	// Use this for initialization
	protected new void Start () {
        base.Start();
        explosion = this.GetComponentInChildren<Detonator>();
        player = (IPlayer)(GameObject.FindGameObjectWithTag("Player").GetComponents(typeof(IPlayer)))[0];
        StartCoroutine("StartEngineLoop");
	}

    IEnumerator StartEngineLoop()
    {
        yield return new WaitForSeconds(0.15f);
        AudioSource[] audios = this.gameObject.GetComponents<AudioSource>();
        audios[1].Play();
    }
	
	// Update is called once per frame
    protected new void Update()
    {
        base.Update();
        if (!target)
        {
            if (isTargetSet && !isDestroyed) // If asteroid or other target is destroyed, we do not want the missile to follow the player
            {
                isDestroyed = true;
                explosion.transform.parent = null;
                explosion.transform.position = this.transform.position;
                explosion.Explode();
                this.gameObject.transform.position = new Vector3(0, 0, -100);
                this.gameObject.audio.enabled = false;
                Object.Destroy(this.gameObject, 5.0f);
            }
            //if (!player.GetComponent<L2_Ship_Script>().isDead || !player.GetComponent<L4_Player_Script>().isDead) // Follow the player if it has not been destroyed yet
            else if (!player.IsDead())
            {
				//this.transform.LookAt(player.GetPosition());
				
				
				Quaternion pRot = this.transform.rotation;
				this.transform.LookAt(player.GetPosition());
				Quaternion finalRot = this.transform.rotation;
				
				this.transform.rotation = Quaternion.Lerp(pRot, finalRot, Mathf.Min(1,Time.deltaTime * homingRotationSpeed));
				
				
				
                this.rigidbody.velocity = this.transform.forward * speed;

                if (!isNotCinematic)
                {
                    this.speed += 0.05f;                
                }
            }

            //Stop following if he dies
            else
            {
                if (isNotCinematic && !isDestroyed)
                {
                    isDestroyed = true;
                    explosion.transform.parent = null;
                    explosion.transform.position = this.transform.position;
                    explosion.Explode();
                    this.gameObject.transform.position = new Vector3(0, 0, -100);
                    this.gameObject.audio.enabled = false;
                    Object.Destroy(this.gameObject, 5.0f);
                }
            }
        }
        else
        {
            this.transform.LookAt(target.transform.position);
            this.rigidbody.velocity = this.transform.forward * speed;
        }
	}

    protected void OnCollisionEnter(Collision col)
    {        
        if (col.gameObject.CompareTag("L2_PlayerShot"))
        {
            isDestroyed = true;
            explosion.transform.parent = null;
            explosion.transform.position = this.transform.position;
            explosion.Explode();
            Destroy(col.gameObject);
            if (isNotCinematic)
            {
                this.gameObject.transform.position = new Vector3(0, 0, -100);
                this.gameObject.audio.enabled = false;
                Object.Destroy(this.gameObject, 5.0f);
            }
        }

        if (col.gameObject.CompareTag("Player") && !isNotCinematic)
        {
            isDestroyed = true;
            explosion.transform.parent = null;
            explosion.transform.position = this.transform.position;
            explosion.Explode();
            this.gameObject.transform.position = new Vector3(0, 0, -1000);
            this.gameObject.audio.enabled = false;
            Object.Destroy(this.gameObject, 5.0f);
        }
        
    }

    public Detonator GetExplosion()
    {
        return explosion;
    }
}
