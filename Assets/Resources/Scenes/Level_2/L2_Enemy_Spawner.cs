using UnityEngine;
using System.Collections;


/** This is an enemy spawner! */
public class L2_Enemy_Spawner : MonoBehaviour {
	
	public L2_Elite_Script elite;
	

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
         * FormationTypes: ft_hl(number of ships) (Horizontal), 
         * EntryBehavior: nb_go(start, end) (go from start to end)
         * LoiterBehavior: lb_no() (Don't hang around)
         * AttackType: at_no() (no attack), at_ld(time) (fire down laser). at_lt() (laser fired towards player), at_hm() (homing missile)
         * ExitTrigger: xt_no() (no exit), xt_tm (time) (Delay leave), xt_im (immediate), 
         * ExitBehavior: xb_no() (never leave), xt_go() (leave IN A DIRECTION (slightly different))
         * timeTillNextWave: Seconds
         * 
         * There's also some sketchy hardcoded things for the Elite, which should be done
         * 	  with care.
         */
		
		W(ft_hl(1), nb_go(0, 25, 0, 0), lb_no(), at_no(), xt_im(), xb_go(45, 0), 3f);
		//Elite makes a pass at you
		//E (EliteBehavior.HangBehind);
		E (EliteBehavior.Test);
        // Scout Ship
        W(ft_hl(1), nb_go(0, 25, 0, 0), lb_no(), at_no(), xt_im(), xb_go(45, 0), 15f);


		
        // Inital Fighter Wave
        W(ft_hl(5), nb_go(-10, 15, 0, 0), lb_no(), at_ld(7.0f), xt_im(), xb_go( 45, 0), 1f);
        W(ft_hl(5), nb_go( 10, 15, 0, 0), lb_no(), at_ld(7.0f), xt_im(), xb_go(-45, 0), 5f);

        // First Blockade
        W(ft_hl(15), nb_go(0, 30, 0, 10), lb_no(), at_lt(10.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(13), nb_go(0, 28, 0, 8), lb_no(), at_ld(10.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(11), nb_go(0, 26, 0, 6), lb_no(), at_ld(10.0f), xt_no(), xb_no(), 0f);
		E (EliteBehavior.HangBehind);
		
		
        // Second Fighter Wave
        W(ft_hl(3), nb_go(15, -15, 0, 5), lb_no(), at_lt(15.0f), xt_tm(3.0f), xb_go(45, 0), -1f);
        W(ft_hl(3), nb_go(-15, -15, 0, 7.5f), lb_no(), at_lt(15.0f), xt_tm(3.0f), xb_go(45, 0), 3f);

        W(ft_hl(3), nb_go(-15, 15, 0, -5), lb_no(), at_lt(15.0f), xt_tm(3.0f), xb_go(45, 0), 0f);
        W(ft_hl(3), nb_go(15, 15, 0, -7.5f), lb_no(), at_lt(15.0f), xt_tm(3.0f), xb_go(45, 0), 1.5f);
		E (EliteBehavior.QuickPass);
		
		
        // Third Fighter Wave
        W(ft_hl(3), nb_go( 25,  8,  0,  2), lb_no(), at_hm(6.0f), xt_tm(3.0f), xb_no(), 0f);
        W(ft_hl(3), nb_go( 25,  8,  0, -2), lb_no(), at_hm(6.0f), xt_tm(3.0f), xb_no(), 0f);
        W(ft_hl(3), nb_go(-25,  8,  5,  0), lb_no(), at_hm(6.0f), xt_tm(3.0f), xb_no(), 0f);
        W(ft_hl(3), nb_go(-25,  8, -5,  0), lb_no(), at_hm(6.0f), xt_tm(3.0f), xb_no(), 0f);

        // Second Blockade
        W(ft_hl(15), nb_go(0, 30, 0, 10), lb_no(), at_lt(7.0f), xt_no(), xb_no(), -.01f);
        W(ft_hl(15), nb_go(0, 28, 0, 8), lb_no(),  at_lt(7.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(13), nb_go(0, 26, 0, 6), lb_no(),  at_ld(7.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(11), nb_go(0, 24, 0, 4), lb_no(),  at_ld(7.0f), xt_no(), xb_no(), 0f);

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
		E (EliteBehavior.QuickPass);
		
        // Filler Wave 2
        W(ft_hl(3), nb_go(20, 8, 0, 2), lb_no(), at_hm(7.0f), xt_tm(3.0f), xb_no(), 0f);
        W(ft_hl(3), nb_go(20, 8, 0, -2), lb_no(), at_hm(7.0f), xt_tm(3.0f), xb_no(), 0f);
        W(ft_hl(3), nb_go(-20, 8, 5, 0), lb_no(), at_hm(7.0f), xt_tm(3.0f), xb_no(), 0f);
        W(ft_hl(3), nb_go(-20, 8, -5, 0), lb_no(), at_hm(7.0f), xt_tm(3.0f), xb_no(), 0f);
		
		//Elite comes and stays for real.
		E(EliteBehavior.FinalBattle);
		
        // Final Blockade Bottom
        W(ft_hl(11), nb_go(0, -30, 0, -6), lb_no(), at_hm(12.0f), xt_no(), xb_no(), -0.1f);
        W(ft_hl(9), nb_go(0, -28, 0, -4), lb_no(), at_hm(12.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(7), nb_go(0, -26, 0, -2), lb_no(), at_hm(12.0f), xt_no(), xb_no(), 1.5f);

        // Final Blockade Top
        W(ft_hl(15), nb_go(0, 30, 0, 10), lb_no(), at_lt(15.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(15), nb_go(0, 28, 0, 8), lb_no(), at_hm(15.0f), xt_no(), xb_no(), 0f);
        W(ft_hl(15), nb_go(0, 26, 0, 6), lb_no(), at_hm(15.0f), xt_no(), xb_no(), 0f);       
        

        // 1-on-1 Elite battle
		//Not any different for now
        
		print ("Done making spawn list");
	}
	
	/** This is where I do things. Basic "define what you want and it's dealt with here */
	void W(FormationType f, EntryBehavior en, LoiterBehavior l, AttackType a, ExitTrigger ext, ExitBehavior exb, float timeTillNextWave)
	{
		//Start
		// This will do things.
		waveList.Add(new Wave(f, en, l, a, ext, exb, timeTillNextWave));
	
	}
	
	enum EliteBehavior { QuickPass, HangBehind, FinalBattle, Test };
	/* Calls to the Elite. These really all get put into a Wave, which gets treated specially.
	 * It's really only coded to do the ~3 things I need the Elite to do right now.
	 */
	void E(EliteBehavior e)
	{
		switch (e)
		{
		case EliteBehavior.QuickPass:
			//W (FormationType.T.ElitePass, nb_go);
			W (ft_ep (), nb_go (-15, 14, 5, -4), lb_no(), at_la(0, -5, 1), xt_im(), xb_go (0, 1), 0f);
			break;
		case EliteBehavior.HangBehind:
			W (ft_eb(), nb_go (-15, 12, 0, 8), lb_lz(-14, 8, 14, 8, 2), at_lt(2), xt_tm (104), xb_go(0, 20), 0);
			break;
		case EliteBehavior.FinalBattle:
			W (ft_ef(), nb_go (0, 20, 0, 8), lb_no(), at_hm(4), xt_no (), xb_no (), 0);
			break;
		case EliteBehavior.Test:
			W (ft_eb(), nb_go (-15, 12, -12, 12), lb_wp (new float[] { -12, 10, 12, 10, -12, -2, 12, -2, -12, 10 }, 5f), 
				at_hm(3), xt_tm(100), xb_go(0, 1),5);
			break;
			
		}
	}
	
	
	/** Helpers so you don't have to W(new BlaType1(), new BlaType2() ....) */
	
	/** FormationType Horizontal Line */
	FormationType ft_hl(int num)
	{
		FormationType ft = new FormationType();
		ft.type = FormationType.T.HorizontalLine;
		ft.num = num;
		return ft;
	}
	
	/** Hack: Formationtype elite pass*/
	FormationType ft_ep()
	{
		FormationType ft = new FormationType();
		ft.type = FormationType.T.ElitePass;
		return ft;
	}
	/** Hack: Formationtype elite hide and shoot in back row*/
	FormationType ft_eb()
	{
		FormationType ft = new FormationType();
		ft.type = FormationType.T.EliteStayBack;
		return ft;
	}
	/** Hack: Elite comes and stays for final battle */
	FormationType ft_ef()
	{
		FormationType ft = new FormationType();
		ft.type = FormationType.T.EliteBattle;
		return ft;
	}
	
	
	
	
	/** eNtry Behavior: GO from point offscreen to onscreen */
	EntryBehavior nb_go(float stx, float sty, float endx, float endy)
	{
		EntryBehavior nb = new EntryBehavior();
		nb.type = EntryBehavior.T.FlyIn;
		nb.startPos = new Vector2(stx, sty);
		nb.endPos = new Vector2(endx, endy);
		return nb;
	}
	
	/** LoiterBehavior do NOthing */
	LoiterBehavior lb_no()
	{
		LoiterBehavior lb = new LoiterBehavior();
		lb.type = LoiterBehavior.T.Nothing;
		return lb;
	}
	/** LoiterBehavior: LoiterZone
	 * Loiter to random positions between 2 points, finding a new point every pointTime */
	LoiterBehavior lb_lz(float x1, float y1, float x2, float y2, float pointTime)
	{
		LoiterBehavior lb = new LoiterBehavior();
		lb.type = LoiterBehavior.T.LoiterZone;
		lb.waypoints.Add(new Vector3(x1, y1, 0));
		lb.waypoints.Add(new Vector3(x2, y2, 0));
		lb.timeEach = pointTime;
		
		return lb;
	}
	/** Goto Waypointns, lingering t time between them */
	LoiterBehavior lb_wp(float[] waypoints, float waitTime)
	{
		LoiterBehavior lb = new LoiterBehavior();
		lb.type = LoiterBehavior.T.GotoWaypoints;
		for (int i=0; i < waypoints.Length-1; i+=2)
		{
			lb.waypoints.Add(new Vector3(waypoints[i], waypoints[i+1], 0));
		}
		lb.timeEach = waitTime;
		
		return lb;
	}
	
	/** AttackType "NO attack" */
	AttackType at_no()
	{
		AttackType at = new AttackType();
		at.type = AttackType.T.None;
		return at;
	}
	/** AttackType LaserDrop (basic lasergoesdown) */
	AttackType at_ld(float fireInterval)
	{
		AttackType at = new AttackType();
		at.type = AttackType.T.LaserDrop;
		at.fireInterval = fireInterval;
		return at;
	}
	/* AttackType LaserAim (laser aimed at fixed location)
	 * only implemented for Elite for now.
	 */
	AttackType at_la(float tx, float ty, float fireInterval)
	{
		AttackType at = new AttackType();
        at.type = AttackType.T.LaserAim;
        at.fireInterval = fireInterval;
		at.target = new Vector3(tx, ty, 0);
        return at;
	}
	//Aim and Target should probably have switched names.
    /** AttackType LaserTarget (laser Fired Towards Player) */
    AttackType at_lt(float fireInterval)
    {
        AttackType at = new AttackType();
        at.type = AttackType.T.LaserTarget;
        at.fireInterval = fireInterval;
        return at;
    }

    /** AttackType HomingMissile (homing missile) */
    AttackType at_hm(float fireInterval)
    {
        AttackType at = new AttackType();
        at.type = AttackType.T.HomingMissile;
        at.fireInterval = fireInterval;
        return at;
    }
	
	/** eXit Trigger "NO exit" */
	ExitTrigger xt_no()
	{
		ExitTrigger xt = new ExitTrigger();
		xt.type = ExitTrigger.T.DontExit;
		return xt;
	}
	/** eXit Trigger TiMed */
	ExitTrigger xt_tm(float seconds)
	{
		ExitTrigger xt = new ExitTrigger();
		xt.type = ExitTrigger.T.ExitAfter;
		xt.exitTime = seconds;
		return xt;
	}
	/** eXit Trigger IMmediately */
	ExitTrigger xt_im()
	{
		ExitTrigger xt = new ExitTrigger();
		xt.type = ExitTrigger.T.ExitImmediately;
		return xt;
	}
	
	
	/** eXit Behavior "NOne" (shouldn't be used if we need to exit...) */
	ExitBehavior xb_no()
	{
		ExitBehavior xb = new ExitBehavior();
		xb.type = ExitBehavior.T.No;
		return xb;
	}
	/** eXit Behavior GO in a direction-
	 * This may be normalized to the enemy maxspeed.
	 * */
	ExitBehavior xb_go(float dx, float dy)
	{
		ExitBehavior xb = new ExitBehavior();
		xb.type = ExitBehavior.T.GoDir;
		xb.dir = new Vector3(dx, dy, 0);
		return xb;
	}
	
	
	/** Where we go through and actually make everything that was made
	 * via W(...)
	 */
	IList waveList = new ArrayList();
	IEnumerator DoSpawning()
	{
		foreach (Wave w in waveList)
		{
            if (!w.hasSpawned)
            {
                if (w.waveDuration >= 0)
                {
                    print("Spawning");
					//if it's just enemies, the Wave can make them
					if (w.ft.type == FormationType.T.HorizontalLine)
					{
                    	w.Spawn();
					}
					//Otherwise it's our temporary hardcoded Elite
					else
					{
						elite.DoWave(w);
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
	
	
	/** The overall wave, composed of the subtypes
	 * Also determines how long to wait before the
	 * 	following wave.
	 */
	public class Wave
	{
		public FormationType ft;
		public EntryBehavior nb;
		public LoiterBehavior lb;
		public AttackType at;
		public ExitTrigger xt;
		public ExitBehavior xb;
		public float waveDuration;

        public bool hasSpawned = false;
		
		IList enemies = new ArrayList();
		
		public Wave(FormationType ft, EntryBehavior nb, LoiterBehavior lb, AttackType at, ExitTrigger xt, ExitBehavior xb, float timeTillNextWave)
		{
			this.ft = ft; this.nb = nb; this.lb = lb; this.at = at; this.xt = xt; this.xb = xb; this.waveDuration = timeTillNextWave;
		}
		
		//Spawn creates all of our stuff
		//The Enemy itself will take care of doing its own things.
		public void Spawn()
		{
			
			//Make the thing.
			switch (ft.type)
			{
			//For now, all we have.
			case FormationType.T.HorizontalLine:
				for (int i=0; i < ft.num; i++)
				{
					Vector3 pos = new Vector3(nb.startPos.x, nb.startPos.y) - new Vector3((i - ft.num/2)*2, 0, 0);
					GameObject e = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Basic"), pos, Quaternion.identity);
					
					e.GetComponent<L2_Enemy_Script>().SetWaveAI(this, -new Vector3((i - ft.num/2)*2, 0, 0));
					this.enemies.Add(e);
				}
				break;
			}
			
			//Start our coroutine that will make it do later things.
		}
		
	}
	/* The composition of the wave, eg 10 enemies in line, etc.
	 *	Is defacto responsible for determining the "center" of the wave.
	 *  Includes sketchy hardcoded Elite behavior for now.
	 */
 	public class FormationType
	{
		public enum T { HorizontalLine, ElitePass, EliteStayBack, EliteBattle, WaypointTest };
		public T type;
		// Subclass maybe, but you should use the helper fxns and not touch
		// the classes themselves.
		public int num;
	}
	/* The way in which the wave enters, and where it goes.
	 * Basically is just "from where" "to where".
	 */
	public class EntryBehavior
	{
		public enum T { FlyIn };
		public T type;
		
		public Vector3 startPos;
		public Vector3 endPos;
	}
	/** What the wave does before it exits.
	 */
	public class LoiterBehavior
	{
		public enum T { Nothing, SwoopOccasionally, GotoWaypoints, TargetPlayer, Patrol, LoiterZone }
		public T type;
		
		//Used for both GotoWaypoints and as the boundaries for Patrol
		//Contains Vector3's
		public IList waypoints = new ArrayList();
		//Multipurpose field.
		//How long between patrol point switches/linger at each waypoint
		//(waypoints not actually implemented yet)
		public float timeEach;
	}
	/** What causes the wave to exit (thus executing
		the ExitBehavior)
		
		If set to exit immediately, then loiterbehavior
		won't be done at all.
	*/
	public class ExitTrigger
	{
		public enum T { ExitImmediately, ExitAfter, DontExit };
		public T type;
		
		public float exitTime;
		public IList waypoints;
	}
	
	/** How this exits, if it does. */
	public class ExitBehavior
	{
		public enum T { GoDir, No };
		public T type;
		
		public Vector3 dir;
	}	
	
	public class AttackType
	{
		//Target: Shoot at current player position
		public enum T { None, LaserDrop, LaserTarget, HomingMissile, LaserAim };
		public T type;
		
		public float fireInterval;
		public Vector3 target;
	}
	
	
}
