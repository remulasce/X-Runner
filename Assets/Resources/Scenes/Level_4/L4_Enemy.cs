using UnityEngine;
using System.Collections;

public class L4_Enemy : MonoBehaviour {

	//Used to know how we should behave.
	L2_Enemy_Spawner.Wave wave;
	Vector3 offset = new Vector3(0,0,0);
	
	//Where we are in our expected lifespan
	enum LifeState { Entry, Loiter, Exit };
	LifeState state = LifeState.Entry;
	float lastStateTime = 0;
	
	//Use this to help with our movement:
	Vector3 target = new Vector3(0,0,0);
	//This will be set in limitSpeed, then used by the movement fxns.
	public float curMaxSpeed = 7f;
	
	//Shooting
	float nextFire = 0;

    // Reference to player
    L4_Player_Script player;    

    // Detonator
    private Detonator explosion;
	
	// Use this for initialization
	void Start () {
		//This gets called after SetWaveAI, because Unity.
		lastStateTime = Time.time;
		nextFire = Time.time + Random.Range(0f, 1f);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<L4_Player_Script>();
        explosion = this.GetComponentInChildren<Detonator>();
	}
	
	//Set how we're going to behave.
	//For convenience, we're just going to assume we're in a Wave
	//	and acta lie it.
	//Offset is a little offset from the targetPositions, so we don't have
	//	waves all over each other.
	public void SetWaveAI(L2_Enemy_Spawner.Wave parent, Vector3 offset)
	{
		//This actually gets called before Start, apparently.
		SetLifeState(LifeState.Entry);
		this.wave = parent;
		this.offset = offset;
		this.nextFire = Time.time + Random.Range(0f, this.wave.at.fireInterval);
	}
	
	
	void killIfOutBounds()
	{
		if (Mathf.Abs(this.transform.position.magnitude) > 60)
		{
			Destroy(this.gameObject);
		}
	}
	
	/** "At" means "Close Enough", so manually move all the way if
	 * you need to be actually at the exact position.
	 * */
	bool AtTarget()
	{
		return ((this.target - this.transform.position).sqrMagnitude < .001f);
	}
	void DoEntry()
	{
		switch (wave.nb.type)
		{
		case L2_Enemy_Spawner.EntryBehavior.T.FlyIn:
			target = new Vector3(wave.nb.endPos.x, wave.nb.endPos.y) + offset;
			if (AtTarget())
			{
				//AtTarget() returns if "close enough", so make sure we're perfect.
				this.transform.position = target;
				SetLifeState(LifeState.Loiter);
			}
			break;
		}
	}
	
	void DoLoiter()
	{
		switch (wave.xt.type)
		{
		case L2_Enemy_Spawner.ExitTrigger.T.ExitImmediately:
			SetLifeState (LifeState.Exit);
			break;
		case L2_Enemy_Spawner.ExitTrigger.T.ExitAfter:
			if ((Time.time - this.lastStateTime) > wave.xt.exitTime)
			{
				SetLifeState(LifeState.Exit);
			}
			break;
		}
		//No need to check for "Dont Exit" case.
	}
	
	//This will not check if we're already in the state
	void SetLifeState(LifeState state)
	{
		print ("Changing State: "+state);
		this.lastStateTime = Time.time;
		this.state = state;	
	}
			
	void DoExit()
	{
		//Hacky event-thingie.
		//Also, assume we're doing that one exit that we know how to do.
		if (Time.time < this.lastStateTime+.5f)
		{
			this.target = this.transform.position + this.offset
				+ (this.wave.xb.dir * 10000);
			
		}
	}
	
	//Actually fire whatever thing we have
	void FireOurWeapon()
	{
        if (player.isDead) // Do not fire a laser when the player is dead
        {
            return;
        }

        
        switch (wave.at.type)
		{
		case (L2_Enemy_Spawner.AttackType.T.None):
			print ("Wait, what?");
			break;
		case (L2_Enemy_Spawner.AttackType.T.LaserDrop):
            //Make an enemy shot
            Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Drop"), this.transform.position - new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));            
			break;
		case (L2_Enemy_Spawner.AttackType.T.LaserTarget):
			//Make an enemy shot
            Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Target"), this.transform.position - new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
			break;
		case (L2_Enemy_Spawner.AttackType.T.HomingMissile):
			//Make an enemy shot
            Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Homing"), this.transform.position - new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
			break;
		}
	}
	
		
	//See if we can shoot somethings.
	//There is some randomness added, so not everything fires
	// in waves.
	void DoShooting()
	{
		if (wave.at.type == L2_Enemy_Spawner.AttackType.T.None) { return; }
		
		if (Time.time > nextFire)
		{
			FireOurWeapon();
			nextFire = Time.time + Random.Range(wave.at.fireInterval*1/2f,  wave.at.fireInterval*3/2f);
		}
	}
	
	//Track towards the target.
	//Uses the curMaxSpeed field.
	void DoTargetMovement()
	{
		//return;
		Vector3 delta = (this.target - this.transform.position);
		this.rigidbody.MovePosition(this.transform.position + Time.deltaTime*(delta.normalized * Mathf.Min(curMaxSpeed, 7.5f*delta.magnitude)));
	}
	// Sets the "Max Speed" variable, based on our current state.
	void SetMaxSpeed()
	{
		switch (state)
		{
		case (LifeState.Entry):
			curMaxSpeed = wave.nb.speed;
			break;
		case (LifeState.Loiter):
			curMaxSpeed = wave.lb.speed;
			break;
		case (LifeState.Exit):
			curMaxSpeed = wave.xb.speed;
			break;
		}
	}		
	
	// Update is called once per frame
	void Update () {
		//so we can assume we have a Wave
		if (wave == null) {return;}
		
		SetMaxSpeed();
		DoTargetMovement();
		
		switch (state)
		{
		case (LifeState.Entry):
			DoEntry();
			break;
		case (LifeState.Loiter):
			DoLoiter();
			break;
		case (LifeState.Exit):
			DoExit();
			break;
		}
		
		DoShooting();
		
		killIfOutBounds();
		
	}
	
	//When we get hit by a player shot, we should die.
	//Leave killing the shot to the shot itself, in case
	//	we make stuff that can go through things
    void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("L2_PlayerShot"))
		{
            explosion.transform.parent = null;
            explosion.transform.position = this.transform.position;
            explosion.Explode();
            Destroy(this.gameObject);
		}			
	}

    public Detonator GetExplosion()
    {
        return explosion;
    }
}
