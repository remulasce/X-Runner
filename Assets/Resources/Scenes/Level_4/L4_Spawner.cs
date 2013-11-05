using UnityEngine;
using System.Collections;


/* Copy-Paste job from L2:
 * Removed all Elite references
 * 
 * Changed the other thing
 */

public class L4_Spawner : MonoBehaviour {

	//public L2_Elite_Script elite;
	// We don't have these for now.
    //public Asteroid_Spawner_Script asteroidSpawner;

	void Start () 
	{
		//Spawn fills the spawnList
		Spawn();
		//DoSpawning goes through the spawnList and does the spawning
		StartCoroutine(DoSpawning());
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

        //E(EliteBehavior.Test);
        // Scout Ship
        W(ft_hl(1), nb_go(0, 25, 0, 0, 4), lb_no(), at_no(), xt_im(), xb_go(45, 0, 10), 6f);

        //Elite makes a pass at you
        E(EliteBehavior.QuickPass);

        // Inital Fighter Wave
        W(ft_hl(5), nb_go(-10, 15, 0, 0), lb_no(), at_ld(7.0f), xt_im(), xb_go(45, 0), 1f);
        W(ft_hl(5), nb_go(10, 15, 0, 0), lb_no(), at_ld(7.0f), xt_im(), xb_go(-45, 0), 6.5f);

        // First Blockade
        W(ft_hl(15), nb_go(0, 30, 0, 10), lb_no(), at_lt(10.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(13), nb_go(0, 28, 0, 8), lb_no(), at_ld(10.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(11), nb_go(0, 26, 0, 6), lb_no(), at_ld(10.0f), xt_no(), xb_no(), 0f);
        E(EliteBehavior.HangBehind);


        // Second Fighter Wave
        W(ft_hl(3), nb_go(15, -15, 0, 5), lb_no(), at_lt(15.0f), xt_tm(3.0f), xb_go(45, 0), -1f);
        W(ft_hl(3), nb_go(-15, -15, 0, 7.5f), lb_no(), at_lt(15.0f), xt_tm(3.0f), xb_go(45, 0), 3f);

        W(ft_hl(3), nb_go(-15, 15, 0, -5), lb_no(), at_lt(15.0f), xt_tm(3.0f), xb_go(45, 0), 0f);
        W(ft_hl(3), nb_go(15, 15, 0, -7.5f), lb_no(), at_lt(15.0f), xt_tm(3.0f), xb_go(45, 0), 1.5f);
        E(EliteBehavior.QuickPass);


        // Third Fighter Wave
        W(ft_hl(3), nb_go(25, 8, 0, 2), lb_no(), at_hm(6.0f), xt_tm(3.0f), xb_no(), 0f);
        W(ft_hl(3), nb_go(25, 8, 0, -2), lb_no(), at_hm(6.0f), xt_tm(3.0f), xb_no(), 0f);
        W(ft_hl(3), nb_go(-25, 8, 5, 0), lb_no(), at_hm(6.0f), xt_tm(3.0f), xb_no(), 0f);
        W(ft_hl(3), nb_go(-25, 8, -5, 0), lb_no(), at_hm(6.0f), xt_tm(3.0f), xb_no(), 0f);

        // Second Blockade
        W(ft_hl(15), nb_go(0, 30, 0, 10), lb_no(), at_lt(7.0f), xt_no(), xb_no(), -.01f);
        W(ft_hl(15), nb_go(0, 28, 0, 8), lb_no(), at_lt(7.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(13), nb_go(0, 26, 0, 6), lb_no(), at_ld(7.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(11), nb_go(0, 24, 0, 4), lb_no(), at_ld(7.0f), xt_no(), xb_no(), 0f);

        // Begin spawning the cinematic asteroids (no physics, background stuff).
        W(ft_ac());

        // Giant Vertical Wave
        W(ft_hl(15), nb_go(0, 40, 0, -60), lb_no(), at_no(), xt_im(), xb_go(0, -60), -.01f);
        W(ft_hl(15), nb_go(0, 38, 0, -62), lb_no(), at_no(), xt_im(), xb_go(0, -60), 0f);
        W(ft_hl(15), nb_go(0, 36, 0, -64), lb_no(), at_no(), xt_im(), xb_go(0, -60), 0f);
        W(ft_hl(15), nb_go(0, 34, 0, -66), lb_no(), at_no(), xt_im(), xb_go(0, -60), 0f);
        W(ft_hl(15), nb_go(0, 32, 0, -68), lb_no(), at_no(), xt_im(), xb_go(0, -60), 0f);
        W(ft_hl(15), nb_go(0, 30, 0, -70), lb_no(), at_no(), xt_im(), xb_go(0, -60), 0f);
        W(ft_hl(15), nb_go(0, 28, 0, -72), lb_no(), at_no(), xt_im(), xb_go(0, -60), 0f);
        W(ft_hl(15), nb_go(0, 26, 0, -74), lb_no(), at_no(), xt_im(), xb_go(0, -60), 0f);
        W(ft_hl(15), nb_go(0, 24, 0, -76), lb_no(), at_no(), xt_im(), xb_go(0, -60), 0f);
        W(ft_hl(15), nb_go(0, 22, 0, -78), lb_no(), at_no(), xt_im(), xb_go(0, -60), 0f);
        W(ft_hl(15), nb_go(0, 20, 0, -80), lb_no(), at_no(), xt_im(), xb_go(0, -60), 0f);
        W(ft_hl(15), nb_go(0, 18, 0, -82), lb_no(), at_no(), xt_im(), xb_go(0, -60), 0f);
        W(ft_hl(15), nb_go(0, 16, 0, -84), lb_no(), at_no(), xt_im(), xb_go(0, -60), 0f);
        W(ft_hl(15), nb_go(0, 14, 0, -86), lb_no(), at_no(), xt_im(), xb_go(0, -60), 8f);

        // Filler Wave 1
        W(ft_hl(4), nb_go(15, 15, 0, 5), lb_no(), at_lt(15.0f), xt_tm(3.0f), xb_no(), 0f);
        W(ft_hl(4), nb_go(-15, 15, 0, 7.5f), lb_no(), at_lt(15.0f), xt_tm(3.0f), xb_no(), 1.5f);

        //Elite quick visit
        E(EliteBehavior.PreFinalBattle);        

        // Filler Wave 2
        W(ft_hl(3), nb_go(20, 8, 0, 2), lb_no(), at_hm(7.0f), xt_tm(3.0f), xb_no(), 0f);
        W(ft_hl(3), nb_go(20, 8, 0, -2), lb_no(), at_hm(7.0f), xt_tm(3.0f), xb_no(), 0f);
        W(ft_hl(3), nb_go(-20, 8, 5, 0), lb_no(), at_hm(7.0f), xt_tm(3.0f), xb_no(), 0f);
        W(ft_hl(3), nb_go(-20, 8, -5, 0), lb_no(), at_hm(7.0f), xt_tm(3.0f), xb_no(), 0f);

        // Final Blockade Bottom
        W(ft_hl(11), nb_go(0, -30, 0, -6), lb_no(), at_hm(12.0f), xt_no(), xb_no(), -0.1f);
        W(ft_hl(9), nb_go(0, -28, 0, -4), lb_no(), at_hm(12.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(7), nb_go(0, -26, 0, -2), lb_no(), at_hm(12.0f), xt_no(), xb_no(), 1.5f);

        // Final Blockade Top
        W(ft_hl(15), nb_go(0, 30, 0, 10), lb_no(), at_lt(15.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(15), nb_go(0, 28, 0, 8), lb_no(), at_hm(15.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(15), nb_go(0, 26, 0, 6), lb_no(), at_hm(15.0f), xt_no(), xb_no(), 0f);

        // SUPER HACK ALERT -- this is done to block the elite coming in until all of the ships from the final blockade are destroyed
        W(ft_hl(1), nb_go(0, 2600, 0, 6), lb_no(), at_hm(15.0f), xt_no(), xb_no(), -0.1f);

        //Elite comes and stays for real.
        E(EliteBehavior.FinalBattle);

        // Game Asteroid Spawning
        W(ft_ag());

        // 1-on-1 Elite battle
		//Not any different for now
        
		print ("Done making spawn list");
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

    enum EliteBehavior { Test, QuickPass, HangBehind, FinalBattle, PreFinalBattle};
	/* Calls to the Elite. These really all get put into a Wave, which gets treated specially.
	 * It's really only coded to do the ~3 things I need the Elite to do right now.
	 */
	void E(EliteBehavior e)
	{
		switch (e)
		{
		case EliteBehavior.QuickPass:
			//W (SpawnTDS.FormationType.T.ElitePass, nb_go);
			W (ft_ep (), nb_go (-15, 14, 5, -4), lb_no(), at_la(0, -5, 1), xt_im(), xb_go (0, 1, 14), 0f);
			break;
		case EliteBehavior.HangBehind:
			W (ft_eb(), nb_go (-15, 12, 0, 8), lb_lz(-14, 8, 14, 8, 2), at_lt(2), xt_tm (10), xb_go(0, 20, 14), 0);
			break;
        case EliteBehavior.PreFinalBattle:
            W(ft_ef(), nb_go(-20, 2, 20, 2), lb_no(), at_ld(0.35f), xt_no(), xb_no(), 0);            
            break;
		case EliteBehavior.FinalBattle:
			W (ft_ef(), nb_go (0, 20, 0, 4), lb_lz(-13, 4, 13, 4, 2.0f)/*lb_no()*/, at_hm(4), xt_no (), xb_no (), 0);            
			break;
		case EliteBehavior.Test:
			W (ft_eb(), nb_go (-15, 12, -12, 12), lb_wp (new float[] { -12, 10, 12, 10, -12, -2, 12, -2, -12, 10 }, 5f), 
				at_hm(3), xt_tm(100), xb_go(0, 1),5);
			break;
			
		}
	}
	
	
	/** Helpers so you don't have to W(new BlaType1(), new BlaType2() ....) */
	
	/** SpawnTDS.FormationType Horizontal Line */
	SpawnTDS.FormationType ft_hl(int num)
	{
		SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
		ft.type = SpawnTDS.FormationType.T.HorizontalLine;
		ft.width = num;
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

    SpawnTDS.FormationType ft_ag()
    {
        SpawnTDS.FormationType ft = new SpawnTDS.FormationType();
        ft.type = SpawnTDS.FormationType.T.AsteroidGameplay;
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
			//Set us sideways
			//Actually not needed because the Klingon model is sideways to begin with.
			//w.defRot = Quaternion.Euler(0, 0, -90);
			//And Klingons.
			w.enemyPrefabPath = "Prefabs/Level_4/L4_Klingon";
            w.defRot = Quaternion.Euler(new Vector3(0, 0, 180));
			
            if (!w.hasSpawned)
            {
                if (w.waveDuration >= 0)
                {
                    print("Spawning");
					//if it's just enemies, the Wave can make them
					if (w.ft.type == SpawnTDS.FormationType.T.HorizontalLine)
					{
                    	w.Spawn();
					}

                    // Asteroid Spawner Special Cases
                    else if (w.ft.type == SpawnTDS.FormationType.T.AsteroidCinematic)
                    {
                        //asteroidSpawner.state = Asteroid_Spawner_Script.ENABLE_STATE.ON_CINEMATIC;
                        //print("Here 1");
                    }

                    else if (w.ft.type == SpawnTDS.FormationType.T.AsteroidGameplay)
                    {
                        //asteroidSpawner.state = Asteroid_Spawner_Script.ENABLE_STATE.ON_GAMEPLAY;
                        //print("Here 2");
                    }

                    //Otherwise it's our temporary hardcoded Elite
                    else
                    {
                        //elite.DoWave(w);
                    }
                    w.hasSpawned = true;
                    yield return new WaitForSeconds(w.waveDuration);
                }
                else
                {
                    GameObject[] ships = GameObject.FindGameObjectsWithTag("L2_Enemy");

                    if (ships.Length > 1) // Have to count the Elite as somthing separate
                    {
                        
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
