using UnityEngine;
using System.Collections;

public class L2_Elite_Script : MonoBehaviour {
	
	enum EntryState { Spawned, Entering, Loitering, Exiting };
	EntryState entryState = EntryState.Entering;
	//Use the SetState() method so this always gets set right
	float lastStateChangeTime = 0;
	
	
	private SpawnTDS.Wave wave;
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

    // Explosion for when the elite gets shot down
    public GameObject endExplosion = null;

    public bool isShotDown = false;

    // Handle to the music manager to start the next transition, when it is in
    Music_Manager_Script musicManager;		
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<L2_Ship_Script>();
        currentHealth = totalHealth;

        musicManager = GameObject.FindGameObjectWithTag("AudioSourceManager").GetComponent<Music_Manager_Script>();
	}
	
	// Update is called once per frame
	void Update () {
		//We don't do anything without a Wave.
		if (wave == null) { return; }

        if (isShotDown) // If the elite is shot down, prevent it from doing anything.
        {
            return;
        }
		
		SetMaxSpeed();
		
		//Entry movement decides when it ends
		DoMovement();
		DoAttack();
		//Exit trigger switch decides when to exit
		CheckExitTrigger();

        // Send positon information to asteroids.
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("L2_Asteroid");
        foreach (GameObject g in asteroids)
        {
            g.GetComponent<L2_Asteroid_Script>().elitePosition = this.transform.position;
        }
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
			case SpawnTDS.ExitTrigger.T.ExitImmediately:
				SetState(EntryState.Exiting);
				break;
			case SpawnTDS.ExitTrigger.T.ExitAfter:
				if (Time.time > this.lastStateChangeTime + wave.xt.exitTime)
				{
					SetState(EntryState.Exiting);
				}
				break;
			case SpawnTDS.ExitTrigger.T.DontExit:
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
		case SpawnTDS.LoiterBehavior.T.LoiterZone:
			if (Time.time > nextLoiterChange)
			{
				nextLoiterChange = Time.time + Random.Range(wave.lb.timeEach*2/3, wave.lb.timeEach*4/3);
				Vector3 w1 = (Vector3)wave.lb.waypoints[0];
				Vector3 w12 =(Vector3)wave.lb.waypoints[1] - w1;
				
				target = w1 + Random.Range(0, 1f) * w12;
			}
			DoTargetMovement();	
			break;
		case SpawnTDS.LoiterBehavior.T.GotoWaypoints:
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
        if (isShotDown)
        {
            return;
        }
        
        switch (wave.at.type)
		{
		case (SpawnTDS.AttackType.T.None):
			print ("Wait, what?");
			break;
		case (SpawnTDS.AttackType.T.LaserDrop):
            //Make an enemy shot
            Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Drop"), this.transform.position - new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));            
			break;
		case (SpawnTDS.AttackType.T.LaserTarget):
			//Make an enemy shot
            Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Target"), this.transform.position - new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
			break;
		case (SpawnTDS.AttackType.T.HomingMissile):
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

    void DoTransition()
    {
        endExplosion = (GameObject)Instantiate(endExplosion);
        endExplosion.transform.position = this.transform.position;
        endExplosion.GetComponent<Detonator>().Explode();

        isShotDown = true;       

        Object.Destroy(this.transform.FindChild("Shield_Dome_Elite").gameObject);

        this.GetComponent<SphereCollider>().radius = 50; // Shot missiles at all of the asteroids in the area

        GameObject.FindGameObjectWithTag("L2_Asteroid_Spawner").GetComponent<Asteroid_Spawner_Script>().state = Asteroid_Spawner_Script.ENABLE_STATE.ON_CINEMATIC;

        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("L2_Asteroid");

        for (int i = asteroids.Length - 1; i >= 0; i--)
        {
            if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), asteroids[i].collider.bounds))
            {
                Object.Destroy(asteroids[i]);
            }
            else
            {
                GameObject gDetonator = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/Explosions/L2_Asteroid_Impact_Explosion"), asteroids[i].transform.position, Quaternion.Euler(0, 0, 0));
                gDetonator.GetComponent<Detonator>().size = 2 * asteroids[i].transform.localScale.x;
                Object.Destroy(asteroids[i]);
            }
        }

        // Do the animation here
        this.transform.parent.animation.Play();

        audio.Play();

        StartCoroutine("ShootBigMissile");

        musicManager.FadeOutSongs(2, new int[] { 4, 5 });
        musicManager.FadeInTransitions(2, new int[] { 6 }, 3, new int[] {}, 0.0f);
    }

    IEnumerator ShootBigMissile()
    {
        yield return new WaitForSeconds(5.0f);
        GameObject missile = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Homing_Cinematic"), this.transform.position, Quaternion.Euler(0, 0, 0));
        missile.transform.localScale *= 1.5f;
    }
	
	void OnCollisionEnter(Collision col)
	{        
        if (col.gameObject.CompareTag("L2_PlayerShot"))
		{
            GameObject gDetonator = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/Explosions/L2_Asteroid_Impact_Explosion"), col.contacts[0].point, Quaternion.Euler(0, 0, 0));
            currentHealth--;
            if (currentHealth <= 0 && !isShotDown)
			{
                DoTransition();
			}
		}

        if (col.gameObject.CompareTag("L2_Asteroid"))
        {
            currentHealth -= totalHealth;       
            GameObject gDetonator = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/Explosions/L2_Asteroid_Impact_Explosion"), col.contacts[0].point, Quaternion.Euler(0, 0, 0));
            gDetonator.GetComponent<Detonator>().size = 4;
            Object.Destroy(col.gameObject);
            if (currentHealth <= 0 && !isShotDown)
            {
                DoTransition();
            }
        }
	}

    const float eliteAwareness = 0.00f;//0.65f; // Used to determine if the elite will shoot back at an asteroid reflected back by the player.
    // The highetr the value, the lower the chace the asteroid will be shot back

    const int maxReflects = 8; // Will allow the player to have a shot if this number of reflects has passed.
	
	//Also don't allow Elite too fire too fast at asteroids in general
	const float reflectRate = 10f;
	float lastReflect;
	
    void OnTriggerStay(Collider other)
    {        
        if (other.gameObject.CompareTag("L2_Asteroid"))
        {
            if (other.gameObject.GetComponent<L2_Asteroid_Script>().targetedByEnemy) // Do no action if the asteroid has already been hit by the enemy
            {
                return;
            }
            
            GameObject laser;
            float detectionVal = Random.Range(0.0f, 1.0f);
            //Debug.Log(other.gameObject.GetComponent<L2_Asteroid_Script>().lastHit);
            //if (other.gameObject.GetComponent<L2_Asteroid_Script>().lastHit == L2_Asteroid_Script.LAST_HIT.PLAYER)
            //{
            //    Debug.LogWarning(detectionVal + " " + (eliteAwareness + Mathf.Clamp((float)(totalHealth - currentHealth) / (float)totalHealth, 0, 1.0f - eliteAwareness)));
            //    Debug.LogWarning(other.gameObject.GetComponent<L2_Asteroid_Script>().numberOfTimesHit < maxReflects /*Just Give the Player the shot when maxReflects have been done*/);
            //}

            if (((other.gameObject.GetComponent<L2_Asteroid_Script>().lastHit == L2_Asteroid_Script.LAST_HIT.PLAYER) 
                && (detectionVal > (eliteAwareness + Mathf.Clamp((float)(totalHealth - currentHealth)/(float)totalHealth, 0, 1.0f - eliteAwareness)))))
            {
                //Reflected too recently, nothing we can do.
                if (Time.time < lastReflect + 1 / reflectRate)
                {
                    return;
                }

                if (other.gameObject.GetComponent<L2_Asteroid_Script>().numberOfTimesHit < maxReflects /*Just Give the Player the shot when maxReflects have been done*/)
                {
                    laser = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Homing"), this.transform.position, Quaternion.Euler(0, 0, 0));
                    laser.GetComponent<L2_Enemy_Shot_Target_Script>().SetTarget(other.gameObject);
                    laser.GetComponent<L2_Enemy_Shot_Target_Script>().speed = 20;
                    other.gameObject.GetComponent<L2_Asteroid_Script>().targetedByEnemy = true;
					lastReflect = Time.time;
                }
            }
            else if (other.gameObject.GetComponent<L2_Asteroid_Script>().lastHit == L2_Asteroid_Script.LAST_HIT.NONE)
            {
                laser = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Homing"), this.transform.position, Quaternion.Euler(0, 0, 0));
                laser.GetComponent<L2_Enemy_Shot_Target_Script>().SetTarget(other.gameObject);
                laser.GetComponent<L2_Enemy_Shot_Target_Script>().speed = 20;
                other.gameObject.GetComponent<L2_Asteroid_Script>().targetedByEnemy = true;
				lastReflect = Time.time;
            }
        }
    }
	
	/** Temporary(?) Elite control. It tries to follow what's in the wave. */
	public void DoWave(SpawnTDS.Wave w)
	{
		this.wave = w;
		this.entryState = EntryState.Spawned;
	}

	

	
	public void DoBoss()
	{
		
	}
	
}
