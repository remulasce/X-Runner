using UnityEngine;
using System.Collections;

public class L2_Elite_Script : MonoBehaviour {
	
	enum EntryState { Spawned, Entering, Loitering, Exiting };
	EntryState entryState = EntryState.Entering;
	//Use the SetState() method so this always gets set right
	float lastStateChangeTime = 0;
	
	
	private L2_Enemy_Spawner.Wave wave;
	private L2_Ship_Script player;


    public int totalHealth = 100;
    public int currentHealth = 0;	
	
	//Use this to help with our movement:
	Vector3 target = new Vector3(0,0,0);
	//Set according to our state in SetMaxSpeed
	public float curMaxSpeed = 7f;
	
	//Help with AI loitering
	public float nextLoiterChange;
	public int curWaypoint = 0;
	
	
	float nextShot;
	
	
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<L2_Ship_Script>();
        currentHealth = totalHealth;
	}
	
	// Update is called once per frame
	void Update () {
		//We don't do anything without a Wave.
		if (wave == null) { return; }
		
		SetMaxSpeed();
		
		//Entry movement decides when it ends
		DoMovement();
		DoAttack();
		//Exit trigger switch decides when to exit
		CheckExitTrigger();
	}
	
	void SetState(EntryState s)
	{
		this.entryState = s;
		this.lastStateChangeTime = Time.time;
	}
	
	//Check whether it's time to exit
	void CheckExitTrigger()
	{
		if (entryState == EntryState.Loitering)
		{
			switch (wave.xt.type)
			{
			case L2_Enemy_Spawner.ExitTrigger.T.ExitImmediately:
				SetState(EntryState.Exiting);
				break;
			case L2_Enemy_Spawner.ExitTrigger.T.ExitAfter:
				if (Time.time > this.lastStateChangeTime + wave.xt.exitTime)
				{
					SetState(EntryState.Exiting);
				}
				break;
			case L2_Enemy_Spawner.ExitTrigger.T.DontExit:
				//nothing
				break;
			}
		}
	}
		
		
	/** "At" means "Close Enough", so manually move all the way if
	 * you need to be actually at the exact position.
	 * */
	bool AtTarget()
	{
		return ((this.target - this.transform.position).sqrMagnitude < .001f);
	}
	
	//Track towards the target.
	void DoTargetMovement()
	{
		//return;
		Vector3 delta = (this.target - this.transform.position);
		this.rigidbody.MovePosition(this.transform.position + Time.deltaTime*(delta.normalized * Mathf.Min(curMaxSpeed, 7.5f*delta.magnitude)));
	}
	
	
	/// <summary>
	/// Set up our initial state.
	/// </summary>
	void DoInitialSpawn()
	{
		//We don't have any other kinds of entry behavior.
		this.transform.position = wave.nb.startPos;
		
		//Meh. Hardcode it.
		this.entryState = EntryState.Entering;
		//Immediately do a loiter change when we switch into it.
		this.nextLoiterChange = 0;
		
		this.nextShot = Random.Range(0f, 1f) * wave.at.fireInterval + Time.time;
	}
	
	/// Move us, based on our entry/exit state/behavior */
	void DoMovement()
	{
		switch (entryState)
		{
		case EntryState.Spawned:
			DoInitialSpawn();
			break;
		case EntryState.Entering:
			DoEntryMovement();
			break;
		case EntryState.Loitering:
			DoLoiterMovement();
			break;
		case EntryState.Exiting:
			DoExitMovement();
			break;
		}
	}
	// Sets the "Max Speed" variable, based on our current state.
	void SetMaxSpeed()
	{
		switch (entryState)
		{
		case (EntryState.Entering):
			curMaxSpeed = wave.nb.speed;
			break;
		case (EntryState.Loitering):
			curMaxSpeed = wave.lb.speed;
			break;
		case (EntryState.Exiting):
			curMaxSpeed = wave.xb.speed;
			break;
		}
	}		
	void DoEntryMovement()
	{
		target = wave.nb.endPos;
		DoTargetMovement();
		if (AtTarget())
		{
			this.entryState = EntryState.Loitering;
		}
	}
	
	void DoLoiterMovement()
	{
		switch (wave.lb.type)
		{
		case L2_Enemy_Spawner.LoiterBehavior.T.LoiterZone:
			if (Time.time > nextLoiterChange)
			{
				nextLoiterChange = Time.time + Random.Range(wave.lb.timeEach*2/3, wave.lb.timeEach*4/3);
				Vector3 w1 = (Vector3)wave.lb.waypoints[0];
				Vector3 w12 =(Vector3)wave.lb.waypoints[1] - w1;
				
				target = w1 + Random.Range(0, 1f) * w12;
			}
			DoTargetMovement();	
			break;
		case L2_Enemy_Spawner.LoiterBehavior.T.GotoWaypoints:
			if (!AtTarget())
			{
				if (Time.time > nextLoiterChange)
				{
					DoTargetMovement();
				}
			}
			else
			{
				curWaypoint++;
				if (curWaypoint == wave.lb.waypoints.Count)
				{
					curWaypoint = 0;
				}
				target = (Vector3)wave.lb.waypoints[curWaypoint];
				nextLoiterChange = Time.time + wave.lb.timeEach;
				
			}
			break;
		}
		
	}
	
	void DoExitMovement()
	{
		target = this.transform.position + 1000 * wave.xb.dir;
		DoTargetMovement();
	}
	
	
	/// <summary>
	/// Figure out attacking stuff.
	/// </summary>
	void DoAttack()
	{
		if (Time.time > nextShot && !player.isDead)
		{
			FireOurWeapon();
			nextShot = Time.time + Random.Range(wave.at.fireInterval * 2/3, wave.at.fireInterval * 4/3);
		}
	}
	
	//Actually fire whatever thing we have
	void FireOurWeapon()
	{
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
	
	
	/*
	void DoShooting()
	{
		if (Time.time > lastShot + .75f)
		{
			GameObject.Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Homing"), this.transform.position, Quaternion.identity);
			lastShot = Time.time + Random.Range(-.1f, .1f);
				
		}
	}
	*/
	
	void OnCollisionEnter(Collision col)
	{        
        if (col.gameObject.CompareTag("L2_PlayerShot"))
		{
            GameObject gDetonator = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/L2_Asteroid_Impact_Explosion"), col.contacts[0].point, Quaternion.Euler(0, 0, 0));
            currentHealth--;
			if (currentHealth <= 0)
			{
				Application.LoadLevel ("Level_3_Graybox");
			}
		}

        if (col.gameObject.CompareTag("L2_Asteroid"))
        {
            currentHealth -= totalHealth/4;            
            GameObject gDetonator = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/L2_Asteroid_Impact_Explosion"), col.contacts[0].point, Quaternion.Euler(0, 0, 0));
            gDetonator.GetComponent<Detonator>().size = 5;
            Object.Destroy(col.gameObject);
            if (currentHealth <= 0)
            {
                Application.LoadLevel("Level_3_Graybox");
            }
        }
	}

    const float eliteAwareness = 0.25f; // Used to determine if the elite will shoot back at an asteroid reflected back by the player

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("L2_Asteroid"))
        {
            GameObject laser;
            float detectionVal = Random.Range(0.0f, 1.0f);
            //Debug.Log(other.gameObject.GetComponent<L2_Asteroid_Script>().lastHit);
            //print(detectionVal > eliteAwareness);
            if ((other.gameObject.GetComponent<L2_Asteroid_Script>().lastHit == L2_Asteroid_Script.LAST_HIT.PLAYER) && (detectionVal > (eliteAwareness + Mathf.Clamp((float)(totalHealth - currentHealth)/(float)totalHealth, 0, 1.0f - eliteAwareness))))
            {
                laser = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Homing"), this.transform.position, Quaternion.Euler(0, 0, 0));
                laser.GetComponent<L2_Enemy_Shot_Target_Script>().SetTarget(other.gameObject);
                laser.GetComponent<L2_Enemy_Shot_Target_Script>().speed = 15;
            }
            else if (other.gameObject.GetComponent<L2_Asteroid_Script>().lastHit == L2_Asteroid_Script.LAST_HIT.ENEMY ||
                other.gameObject.GetComponent<L2_Asteroid_Script>().lastHit == L2_Asteroid_Script.LAST_HIT.NONE)
            {
                laser = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Homing"), this.transform.position, Quaternion.Euler(0, 0, 0));
                laser.GetComponent<L2_Enemy_Shot_Target_Script>().SetTarget(other.gameObject);
                laser.GetComponent<L2_Enemy_Shot_Target_Script>().speed = 15;
            }
        }
    }
	
	/** Temporary(?) Elite control. It tries to follow what's in the wave. */
	public void DoWave(L2_Enemy_Spawner.Wave w)
	{
		this.wave = w;
		this.entryState = EntryState.Spawned;
	}

	

	
	public void DoBoss()
	{
		
	}
	
}
