using UnityEngine;
using System.Collections;


/** Some base classes for both L2 and L4
 * 
 * Separating out the Spawner from the Wave infrastructure.
 */

public class SpawnTDS : MonoBehaviour {

	
	/** The overall wave, composed of the subtypes
	 * Also determines how long to wait before the
	 * 	following wave.
	 */
	public class Wave
	{
		//Hacky for L4 direct manipulation
		public string enemyPrefabPath = "Prefabs/Level_2/L2_Enemy_Basic";
		public Quaternion defRot = Quaternion.identity;
		
		public FormationType ft;
		public EntryBehavior nb;
		public LoiterBehavior lb;
		public AttackType at;
		public ExitTrigger xt;
		public ExitBehavior xb;
		public float waveDuration;
		
		
        public bool hasSpawned = false;
        public float waveSpeed = 7.0f;
		
		IList enemies = new ArrayList();
		
		public Wave(FormationType ft, EntryBehavior nb, LoiterBehavior lb, AttackType at, ExitTrigger xt, ExitBehavior xb, float timeTillNextWave)
		{
			this.ft = ft; this.nb = nb; this.lb = lb; this.at = at; this.xt = xt; this.xb = xb; this.waveDuration = timeTillNextWave;
		}

        public Wave(FormationType ft)
        {
            this.ft = ft;
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
					GameObject e = (GameObject)GameObject.Instantiate(Resources.Load(enemyPrefabPath), pos, defRot);
					
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
		public enum T { HorizontalLine, ElitePass, EliteStayBack, EliteBattle, WaypointTest, AsteroidCinematic, AsteroidGameplay };
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
		
		public float speed;
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
		public float speed;
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
		public float speed;
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
