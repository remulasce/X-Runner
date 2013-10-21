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
         * ExitBehavior: xb_no() (never leave), xt_go() (leave towards a position)
         * timeTillNextWave: Seconds
         */
		//Fake wave for timing purposes
		W (ft_hl(0), nb_go(0,0,0,0), lb_no (), at_no(), xt_no(), xb_no(), 3);
        /** should be 3 seconds */
		W(ft_hl(10), nb_go(-30, 8, 0, 8), lb_no(), at_lt(4f), xt_tm(3), xb_go(1, 1), 3);
        W(ft_hl(20), nb_go(20, 20, 0, 10), lb_no(), at_hm(4f), xt_im(), xb_go(-1, 0), 3);
        W(ft_hl(4), nb_go(-20, 20, 0, 8), lb_no(), at_ld(4f), xt_no(), xb_go(11, 1), 4);
		print ("Done making spawn list");
	}
	
	/** This is where I do things. Basic "define what you want and it's dealt with here */
	void W(FormationType f, EntryBehavior en, LoiterBehavior l, AttackType a, ExitTrigger ext, ExitBehavior exb, float timeTillNextWave)
	{
		//Start
		// This will do things.
		waveList.Add(new Wave(f, en, l, a, ext, exb, timeTillNextWave));
	
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
			print ("Spawning");
			w.Spawn();
			yield return new WaitForSeconds(w.waveDuration);
		}
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
	 */
	public class FormationType
	{
		public enum T { HorizontalLine };
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
		public enum T { Nothing, SwoopOccasionally, GotoWaypoints, TargetPlayer }
		public T type;
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
		public enum T { None, LaserDrop, LaserTarget, HomingMissile };
		public T type;
		
		public float fireInterval;
	}
	
	
}
