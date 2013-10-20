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
		print("Spawning");
		
		
		W (ft_hl(10), nb_st(-100,0,0,0), lb_no(), at_no(), xt_no(), xb_no(), 1);
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
	
	/** eNtry Behavior STandard: Go from point offscreen to onscreen */
	EntryBehavior nb_st(float stx, float sty, float endx, float endy)
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
	
	
	/** eXit Trigger "NO exit" */
	ExitTrigger xt_no()
	{
		ExitTrigger xt = new ExitTrigger();
		xt.type = ExitTrigger.T.DontExit;
		return xt;
	}
	
	/** eXit Behavior "NOne" (shouldn't be used if we need to exit...) */
	ExitBehavior xb_no()
	{
		ExitBehavior xb = new ExitBehavior();
		xb.type = ExitBehavior.T.GotoDst;
		return new ExitBehavior();
	}
	
	
	/** Where we go through and actually make everything that was made
	 * via W(...)
	 */
	IList waveList = new ArrayList();
	IEnumerator DoSpawning()
	{
		foreach (Wave w in waveList)
		{
			
			
			
			// w.Spawn() or something
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
					Vector3 pos = new Vector3(nb.startPos.x, nb.startPos.y) - new Vector3((i/ft.num-1/2f)*3, 0, 0);
					GameObject e = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Level2/Enemy"));
					
					e.GetComponent<L2_Enemy_Script>().SetWaveAI(this, -new Vector3((i/ft.num-1/2f)*3, 0, 0));
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
		
		public Vector2 startPos;
		public Vector2 endPos;
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
		public enum T { GotoDst };
		public T type;
		
		public Vector2 dst;
	}	
	
	public class AttackType
	{
		public enum T { None, LaserDrop, LaserTarget, HomingMissile };
		public T type;
	}
	
	
}
