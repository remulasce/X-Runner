using UnityEngine;
using System.Collections;



/** This is an enemy spawner! */
public class L4_Spawner : MonoBehaviour {
	
	public L2_Elite_Script elite;

    public Asteroid_Spawner_Script asteroidSpawner;

    public Music_Manager_Script musicManager;

	void Start () 
	{
		//Spawn fills the spawnList
		Spawn();
		//DoSpawning goes through the spawnList and does the spawning
		StartCoroutine(DoSpawning());

        musicManager = GameObject.FindGameObjectWithTag("AudioSourceManager").GetComponent<Music_Manager_Script>();
	}
	
	/** SpawnAPI:
	
	Wave:
	Type:
		-Horizontal (numguys, spacing)
	Entry:
		StartPos: (x,y)
		EndPos: (x,y)
	Behavior while Onscreen:
		-SitStill
		-SwoopOccasionally
		-GotoWaypoints (waypointlist)
		-TargetPlayer
	ExitTiming:
		-ExitImmediately
		-ExitAfter (numseconds)
		-DontExit
	ExitMethod:
		-GotoPoint: (x,y)
	AttackMethod:
		-None
		-LaserDrop
		-LaserTarget
		-Homing Missile
	NextWaveIn: (numseconds)
	
	
	-- Elite API later --
	*/
	
	
	
	/** This doesn't actually do spawning-
	 * what it does is fill the waveList,
	 * whcih the Spawner will use to actually
	 * do spawning.
	 * 
	 * It's organized this way so there's less to type.
	 */
	void Spawn() {
		print("Making spawn list...");

        /*
         * Parameter Types
         * SpawnTDS.FormationTypes: ft_hl(number of ships) (Horizontal), 
         * SpawnTDS.EntryBehavior: nb_go(start, end) (go from start to end)
         * SpawnTDS.LoiterBehavior: lb_no() (Don't hang around), lb_lz(min, max, switchTime) (loiter between two points), lb_wp(float[] points, timeToSwitch) (Go through a waypointList)
         * SpawnTDS.AttackType: at_no() (no attack), at_ld(time) (fire down laser). at_lt() (laser fired towards player), at_hm() (homing missile)
         * SpawnTDS.ExitTrigger: xt_no() (no exit), xt_tm (time) (Delay leave), xt_im (immediate), 
         * SpawnTDS.ExitBehavior: xb_no() (never leave), xt_go() (leave IN A DIRECTION (slightly different))
         * timeTillNextWave: Seconds
         * 
         * There's also some sketchy hardcoded things for the Elite, which should be done
         * 	  with care.
         */

        /*
        // SUPER HACK ALERT -- this is done to block the elite coming in until all of the ships from the final blockade are destroyed
        W(ft_hl(1), nb_go(0, 2600, 0, 6), lb_no(), at_hm(15.0f), xt_no(), xb_no(), 1f);

        // Scout Diamond Wave
        W(ft_hl(1), nb_go(30, 2, 0, 2, 12.5f), lb_no(), at_lt(3.0f), xt_im(), xb_go(0, 1, 20), 0);
        W(ft_hl(1), nb_go(28, 0, -2, 0, 12.5f), lb_no(), at_lt(3.0f), xt_im(), xb_go(0, 1, 20), 0);
        W(ft_hl(1), nb_go(30, 0, 0, 0, 12.5f), lb_no(), at_hm(2.0f), xt_im(), xb_go(0, 1, 20), 0);
        W(ft_hl(1), nb_go(32, 0, 2, 0, 12.5f), lb_no(), at_lt(3.0f), xt_im(), xb_go(0, 1, 20), 0);
        W(ft_hl(1), nb_go(30, -2, 0, -2, 12.5f), lb_no(), at_lt(3.0f), xt_im(), xb_go(0, 1, 20), 3);

        const int numBoxWaves = 9;
        const float waveDelayOne = 1.525f;

        // Small Squads
        for (int i = 0; i < numBoxWaves; i++)
        {
            float yVal = Random.Range(-6f, 6f);

            W(ft_gd(4 + (i / 4), 4 + (i / 4)), nb_go(30, yVal - (i / 4), -30, yVal - (i / 4), 10.5f), lb_no(), at_lt(12.0f), xt_im(), xb_go(0, 1, 1000), waveDelayOne);
        }

        //----------------------------------------------------------------------------------------------
         
        // SUPER HACK ALERT -- this is done to block the elite coming in until all of the ships from the final blockade are destroyed
        W(ft_hl(1), nb_go(0, 2600, 0, 6), lb_no(), at_hm(15.0f), xt_no(), xb_no(), 3.5f);

        // Fade in the space song
        W(ft_space());

        //Look at capital ship
        W(ft_lookat_cap(.7f, 2, .45f, 1500), nb_go(0, 0, 0, 0, 0), lb_no(), at_no(), xt_no(), xb_no(), 4f);

        // Big Wave
        W(ft_gd(20, 14), nb_go(40f, 0, 10f, 0, 12), lb_no(), at_no(), xt_im(), xb_go(-1, 0, 18), 6f);

        // Come from behind waves

        const int numBehindWaves = 6;
        const float waveDelayTwo = 3.5f;

        for (int i = 0; i < numBehindWaves; i++)
        {
            float rowVal = Random.Range(0, 13);
            int negVal = 1;
            if (i % 2 == 0)
            {
                negVal *= -1;
            }

            for (int j = 0; j < 13; j++)
            {
                if (j != rowVal && j != rowVal + 1 && j != rowVal - 1 && (j != rowVal + (2 * Mathf.Sign(negVal)) || (i / 3 >= 1))) // Make a hole 2 - 4 rows big, then 2 - 3 rows big after 3 waves
                {
                    if (i != (numBehindWaves - 1))
                    {
                        W(ft_hl(2 + i), nb_go(-40, 12 - (2 * j), 14 - (2 * (i / 2)), 12 - (2 * j), 16.5f), lb_no(), at_hm(7.5f), xt_tm(waveDelayTwo / 6), xb_go(0, negVal, 20), 0);
                    }
                    else
                    {
                        W(ft_hl(2 + i), nb_go(-40, 12 - (2 * j), 14 - (2 * (i / 2)), 12 - (2 * j), 16.5f), lb_no(), at_hm(11.5f), xt_tm(waveDelayTwo / 6), xb_go(0, negVal, 20), 0);
                    }
                }
                
            }
            // SUPER HACK ALERT -- this is done to block the elite coming in until all of the ships from the final blockade are destroyed
            W(ft_hl(1), nb_go(0, 2600, 0, 6), lb_no(), at_hm(15.0f), xt_no(), xb_no(), waveDelayTwo);
        }
		 */
        // SUPER HACK ALERT -- this is done to block the elite coming in until all of the ships from the final blockade are destroyed
        W(ft_hl(1), nb_go(0, 2600, 0, 6), lb_no(), at_hm(15.0f), xt_no(), xb_no(), 2.5f);

        // Fade in the trench song
        W(ft_trench());

        //Glance at cap ship before entering
        //Wait for everything to be dead first, though.
        W(ft_lookat_cap(.5f, 1f, .5f, 750), nb_go(0, 0, 0, 0, 0), lb_no(), at_no(), xt_no(), xb_no(), 0f);

        // Have a bit of a delay before spawning ships again
        // SUPER HACK ALERT -- this is done to block the elite coming in until all of the ships from the final blockade are destroyed
        W(ft_hl(1), nb_go(0, 2600, 0, 6), lb_no(), at_hm(15.0f), xt_no(), xb_no(), 42.5f);

        // Fade in the space song
        W(ft_missileRun());

        // Spawn the Tie Bomber Wave
        W(ft_gd(60, 2), nb_go(40, 30, 40, 10, 20), lb_no(), at_ld(2.0f), xt_im(), xb_go(-1, 0, 17.5f), 15.25f);

        // Fade in the grand finale (everything)
        W(ft_finale());

        W(ft_hl(15), nb_go(0, 25, 0, 11, 20), lb_no(), at_ld(2.5f), xt_tm(1.75f), xb_go(0, 1, 17.5f), 2.25f);

        W(ft_hl(15), nb_go(0, 25, 0, 11, 20), lb_no(), at_ld(2.5f), xt_tm(1.75f), xb_go(0, 1, 17.5f), 3.75f);

        // SUPER HACK ALERT -- this is done to block the elite coming in until all of the ships from the final blockade are destroyed
        W(ft_hl(1), nb_go(0, 2600, 0, 6), lb_no(), at_hm(15.0f), xt_no(), xb_no(), 12f);

        // Fade out everything
        W(ft_endMusic());           
        
        // SUPER HACK ALERT -- this is done to block the elite coming in until all of the ships from the final blockade are destroyed
        W(ft_hl(1), nb_go(0, 2600, 0, 6), lb_no(), at_hm(15.0f), xt_no(), xb_no(), 1f);
		
    	print ("Done making spawn list");
        //*/
	}

	
	void StartTrench()
	{
		print("starting trench");
		GameObject.FindGameObjectWithTag("L4_Trench").GetComponent<L4_Background>().StartTrench();
		GameObject.FindGameObjectWithTag("L4_Trench").GetComponent<Transform>().position = new Vector3(780, 0, .9f);
	}
	
	/** This is where I do things. Basic "define what you want and it's dealt with here */
	void W(SpawnTDS.FormationType f, SpawnTDS.EntryBehavior en, SpawnTDS.LoiterBehavior l, SpawnTDS.AttackType a, SpawnTDS.ExitTrigger ext, SpawnTDS.ExitBehavior exb, float timeTillNextWave)
	{
		//Start
		// This will do things.
		waveList.Add(new SpawnTDS.Wave(f, en, l, a, ext, exb, timeTillNextWave));
	
	}

    // This is specially for switching the asteroid spawner on
    void W(SpawnTDS.FormationType f)
    {        
        waveList.Add(new SpawnTDS.Wave(f));
    }
	
	
	/** Helpers so you don't have to W(new BlaType1(), new BlaType2() ....) */
	
	/** Look ahead at enemy capital ship */
	SpawnTDS.FormationType ft_lookat_cap(float transitionInTime, float ponderTime, float transitionOutTime, float distancefromplayer)
	{
		SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
		ft.args = new float[] { transitionInTime, ponderTime, transitionOutTime, distancefromplayer};
		ft.type = SpawnTDS.FormationType.T.L4_Lookat_Cap;
		return ft;
	}
	
	/** Start Trench formation */
	SpawnTDS.FormationType ft_st()
	{
		SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
		ft.type = SpawnTDS.FormationType.T.L4_Trench;
		return ft;
	}
	
	/** SpawnTDS.SpawnTDS.FormationType Horizontal Line */
	SpawnTDS.FormationType ft_hl(int num)
	{
		SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
		ft.type = SpawnTDS.FormationType.T.HorizontalLine;
		ft.width = num;
		return ft;
	}
	//Vertical Line
	SpawnTDS.FormationType ft_vl(int num)
	{
		SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
		ft.type = SpawnTDS.FormationType.T.VerticalLine;
		ft.height = num;
		return ft;
	}
	//Grid
	SpawnTDS.FormationType ft_gd(int width, int height)
	{
		SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
		ft.type = SpawnTDS.FormationType.T.Grid;
		ft.width = width; ft.height = height;
		return ft;
	}
	
	/** Hack: Formationtype elite pass*/
	SpawnTDS.FormationType ft_ep()
	{
		SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
		ft.type = SpawnTDS.FormationType.T.ElitePass;
		return ft;
	}
	/** Hack: Formationtype elite hide and shoot in back row*/
	SpawnTDS.FormationType ft_eb()
	{
		SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
		ft.type = SpawnTDS.FormationType.T.EliteStayBack;
		return ft;
	}
	/** Hack: Elite comes and stays for final battle */
	SpawnTDS.FormationType ft_ef()
	{
		SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
		ft.type = SpawnTDS.FormationType.T.EliteBattle;
		return ft;
	}

    /*Hacks to get the asteroid spawning on*/
    SpawnTDS.FormationType ft_ac()
    {
        SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
        ft.type = SpawnTDS.FormationType.T.AsteroidCinematic;
        return ft;
    }

    SpawnTDS.FormationType ft_ac2()
    {
        SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
        ft.type = SpawnTDS.FormationType.T.AsteroidCinematic2;
        return ft;
    }

    SpawnTDS.FormationType ft_ag()
    {
        SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
        ft.type = SpawnTDS.FormationType.T.AsteroidGameplay;
        return ft;
    }


    /*Hacks for music*/
    SpawnTDS.FormationType ft_space()
    {
        SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
        ft.type = SpawnTDS.FormationType.T.L4_Space;
        return ft;
    }

    SpawnTDS.FormationType ft_trench()
    {
        SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
        ft.type = SpawnTDS.FormationType.T.L4_Trench;
        return ft;
    }

    SpawnTDS.FormationType ft_missileRun()
    {
        SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
        ft.type = SpawnTDS.FormationType.T.L4_MissileRun;
        return ft;
    }

    SpawnTDS.FormationType ft_finale()
    {
        SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
        ft.type = SpawnTDS.FormationType.T.L4_Finale;
        return ft;
    }

    SpawnTDS.FormationType ft_endMusic()
    {
        SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
        ft.type = SpawnTDS.FormationType.T.L4_Fade_Out;
        return ft;
    }
	
	
	//		=========		ENTRY BEHAVIOR		===========
	float nb_def_speed = 7;
	
	
	/** eNtry Behavior: GO from point offscreen to onscreen */
	SpawnTDS.EntryBehavior nb_go(float stx, float sty, float endx, float endy, float speed)
	{
		SpawnTDS.EntryBehavior nb = new SpawnTDS.EntryBehavior();
		nb.type = SpawnTDS.EntryBehavior.T.FlyIn;
		nb.startPos = new Vector2(stx, sty);
		nb.endPos = new Vector2(endx, endy);
		nb.speed = speed;
		return nb;
	}
	SpawnTDS.EntryBehavior nb_go(float stx, float sty, float endx, float endy)
	{
		return nb_go (stx, sty, endx, endy, nb_def_speed);
	}
	
	
	//		========		LOITER BEHAVIOR		===========
	float lb_def_speed = 7;
	/** SpawnTDS.LoiterBehavior do NOthing */
	SpawnTDS.LoiterBehavior lb_no()
	{
		SpawnTDS.LoiterBehavior lb = new SpawnTDS.LoiterBehavior();
		lb.type = SpawnTDS.LoiterBehavior.T.Nothing;
		return lb;
	}
	/** SpawnTDS.LoiterBehavior: LoiterZone
	 * Loiter to random positions between 2 points, finding a new point every pointTime */
	SpawnTDS.LoiterBehavior lb_lz(float x1, float y1, float x2, float y2, float pointTime, float speed)
	{
		SpawnTDS.LoiterBehavior lb = new SpawnTDS.LoiterBehavior();
		lb.type = SpawnTDS.LoiterBehavior.T.LoiterZone;
		lb.waypoints.Add(new Vector3(x1, y1, 0));
		lb.waypoints.Add(new Vector3(x2, y2, 0));
		lb.timeEach = pointTime;
		lb.speed = speed;
		
		return lb;
	}
	SpawnTDS.LoiterBehavior lb_lz(float x1, float y1, float x2, float y2, float pointTime)
	{
		return lb_lz(x1, y1, x2, y2, pointTime, lb_def_speed);
	}
	
	/** Goto Waypointns, lingering t time between them */
	SpawnTDS.LoiterBehavior lb_wp(float[] waypoints, float waitTime, float speed)
	{
		SpawnTDS.LoiterBehavior lb = new SpawnTDS.LoiterBehavior();
		lb.type = SpawnTDS.LoiterBehavior.T.GotoWaypoints;
		for (int i=0; i < waypoints.Length-1; i+=2)
		{
			lb.waypoints.Add(new Vector3(waypoints[i], waypoints[i+1], 0));
		}
		lb.timeEach = waitTime;
		lb.speed = speed;
		
		return lb;
	}
	SpawnTDS.LoiterBehavior lb_wp(float[] waypoints, float waitTime)
	{
		return  lb_wp(waypoints, waitTime, lb_def_speed);
	}
	
	
	
	//		=========		ATTACK TYPE		==========
	
	/** SpawnTDS.AttackType "NO attack" */
	SpawnTDS.AttackType at_no()
	{
		SpawnTDS.AttackType at = new SpawnTDS.AttackType();
		at.type = SpawnTDS.AttackType.T.None;
		return at;
	}
	/** SpawnTDS.AttackType LaserDrop (basic lasergoesdown) */
	SpawnTDS.AttackType at_ld(float fireInterval)
	{
		SpawnTDS.AttackType at = new SpawnTDS.AttackType();
		at.type = SpawnTDS.AttackType.T.LaserDrop;
		at.fireInterval = fireInterval;
		return at;
	}
	/* SpawnTDS.AttackType LaserAim (laser aimed at fixed location)
	 * only implemented for Elite for now.
	 */
	SpawnTDS.AttackType at_la(float tx, float ty, float fireInterval)
	{
		SpawnTDS.AttackType at = new SpawnTDS.AttackType();
        at.type = SpawnTDS.AttackType.T.LaserAim;
        at.fireInterval = fireInterval;
		at.target = new Vector3(tx, ty, 0);
        return at;
	}
	//Aim and Target should probably have switched names.
    /** SpawnTDS.AttackType LaserTarget (laser Fired Towards Player) */
    SpawnTDS.AttackType at_lt(float fireInterval)
    {
        SpawnTDS.AttackType at = new SpawnTDS.AttackType();
        at.type = SpawnTDS.AttackType.T.LaserTarget;
        at.fireInterval = fireInterval;
        return at;
    }

    /** SpawnTDS.AttackType HomingMissile (homing missile) */
    SpawnTDS.AttackType at_hm(float fireInterval)
    {
        SpawnTDS.AttackType at = new SpawnTDS.AttackType();
        at.type = SpawnTDS.AttackType.T.HomingMissile;
        at.fireInterval = fireInterval;
        return at;
    }
	
	
	//		========		EXIT TRIGGER		=========
	
	/** eXit Trigger "NO exit" */
	SpawnTDS.ExitTrigger xt_no()
	{
		SpawnTDS.ExitTrigger xt = new SpawnTDS.ExitTrigger();
		xt.type = SpawnTDS.ExitTrigger.T.DontExit;
		return xt;
	}
	/** eXit Trigger TiMed */
	SpawnTDS.ExitTrigger xt_tm(float seconds)
	{
		SpawnTDS.ExitTrigger xt = new SpawnTDS.ExitTrigger();
		xt.type = SpawnTDS.ExitTrigger.T.ExitAfter;
		xt.exitTime = seconds;
		return xt;
	}
	/** eXit Trigger IMmediately */
	SpawnTDS.ExitTrigger xt_im()
	{
		SpawnTDS.ExitTrigger xt = new SpawnTDS.ExitTrigger();
		xt.type = SpawnTDS.ExitTrigger.T.ExitImmediately;
		return xt;
	}
	
	
	//		=======		EXIT BEHAVIOR		========
	float xb_def_speed = 7;
	
	/** eXit Behavior "NOne" (shouldn't be used if we need to exit...) */
	SpawnTDS.ExitBehavior xb_no()
	{
		SpawnTDS.ExitBehavior xb = new SpawnTDS.ExitBehavior();
		xb.type = SpawnTDS.ExitBehavior.T.No;
		return xb;
	}
	/** eXit Behavior GO in a direction-
	 * This may be normalized to the enemy maxspeed.
	 * */
	SpawnTDS.ExitBehavior xb_go(float dx, float dy, float speed)
	{
		SpawnTDS.ExitBehavior xb = new SpawnTDS.ExitBehavior();
		xb.type = SpawnTDS.ExitBehavior.T.GoDir;
		xb.dir = new Vector3(dx, dy, 0);
		xb.speed = speed;
		return xb;
	}
	SpawnTDS.ExitBehavior xb_go(float dx, float dy)
	{
		return xb_go(dx, dy, xb_def_speed);
	}
	
	/** Where we go through and actually make everything that was made
	 * via W(...)
	 */
	IList waveList = new ArrayList();
	IEnumerator DoSpawning()
	{
		foreach (SpawnTDS.Wave w in waveList)
		{
            w.enemyPrefabPath = "Prefabs/Level_4/L4_Klingon";
            w.defRot = Quaternion.Euler(0, 0, 180);
            
            if (!w.hasSpawned)
            {
                if (w.waveDuration >= 0)
                {
                    print("Spawning");
					//if it's just enemies, the Wave can make them
					if (w.ft.type == SpawnTDS.FormationType.T.HorizontalLine
						|| w.ft.type == SpawnTDS.FormationType.T.Grid
						|| w.ft.type == SpawnTDS.FormationType.T.VerticalLine)
					{
                    	w.Spawn();
					}

                    // Asteroid Spawner Special Cases
                    else if (w.ft.type == SpawnTDS.FormationType.T.AsteroidCinematic)
                    {
                        asteroidSpawner.state = Asteroid_Spawner_Script.ENABLE_STATE.ON_CINEMATIC;
                        if (musicManager)
                        {
                            musicManager.FadeInSongs(2, new int[] { 1 });
                        }
                        //print("Here 1");
                    }

                    else if (w.ft.type == SpawnTDS.FormationType.T.AsteroidGameplay)
                    {
                        asteroidSpawner.state = Asteroid_Spawner_Script.ENABLE_STATE.ON_GAMEPLAY;
                        if (musicManager)
                        {
                            musicManager.FadeOutSongs(2, new int[] { 0, 1 });
                            musicManager.FadeInTransitions(2, new int[] { 2 }, 2, new int[] { 3 }, 6.76f);
                        }
                        //print("Here 2");
                    }                    
					else if (w.ft.type == SpawnTDS.FormationType.T.L4_Lookat_Cap)
					{
						GameObject.FindGameObjectWithTag("L4_Trench").GetComponent<Transform>().position = new Vector3(w.ft.args[3], 0, .9f);
						GameObject.FindGameObjectWithTag("MainCamera").GetComponent<L4_Camera>()
							.LookAtCapShip(w.ft.args[0],w.ft.args[1],w.ft.args[2]);
					}

                    else if (w.ft.type == SpawnTDS.FormationType.T.L4_Space)
                    {
                        if (musicManager)
                        {
                            musicManager.FadeInSongs(4, new int[] { 1 });
                        }
                    }

                    else if (w.ft.type == SpawnTDS.FormationType.T.L4_Trench)
                    {
                        if (musicManager)
                        {
                            musicManager.FadeOutSongs(4, new int[] { 1 });
                            musicManager.FadeInSongs(4, new int[] { 2 });
                        }
                        StartTrench(); // Make sure to start the trench scene
                    }

                    else if (w.ft.type == SpawnTDS.FormationType.T.L4_MissileRun)
                    {
                        if (musicManager)
                        {
                            musicManager.FadeOutSongs(4, new int[] { 2 });
                            musicManager.FadeInSongs(4, new int[] { 3 });
                        }
                    }

                    else if (w.ft.type == SpawnTDS.FormationType.T.L4_Finale)
                    {
                        if (musicManager)
                        {
                            musicManager.FadeInSongs(4, new int[] { 1, 2 });
                        }
                    }

                    else if (w.ft.type == SpawnTDS.FormationType.T.L4_Fade_Out)
                    {
                        if (musicManager)
                        {
                            musicManager.FadeOutSongs(4, new int[] { 0, 1, 2, 3 });
                            musicManager.FadeInTransitions(4, new int[] { 4 }, 4, new int[] {5, 6}, 6.0f);
                        }
                    } 

                    w.hasSpawned = true;
                    yield return new WaitForSeconds(w.waveDuration);
                }
                else
                {
                    GameObject[] ships = GameObject.FindGameObjectsWithTag("L2_Enemy");

                    if (ships.Length > 1) // Have to count the Elite as somthing separate
                    {
                        print(ships.Length);
                        StartCoroutine("RestartSpawning");
                        break;
                    }
                    else // Once there are no regular ships left, spawning continues after the delay from destroying the current wave expires
                    {
                        yield return new WaitForSeconds(-w.waveDuration);
                        print("Spawning");
                        w.Spawn();
                        w.hasSpawned = true;                  
                    }
                }
            }
		}
	}


    IEnumerator RestartSpawning()
    {
        yield return new WaitForSeconds(0.5f);
        print("Attempting to spawn again...");
        StartCoroutine("DoSpawning");
    }
	
	
	
	
}
