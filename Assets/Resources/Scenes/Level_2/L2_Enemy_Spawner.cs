using UnityEngine;
using System.Collections;


/** This is an enemy spawner! */
public class L2_Enemy_Spawner : MonoBehaviour {
	
	public L2_Elite_Script elite;
	
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(SpawnStuff());

	}
	
	enum WaveType { Side, Line, /*Loop,*/ EliteVisit, EliteBoss };
	WaveType[] roster = { 
		WaveType.Side, 
		WaveType.EliteVisit,
		WaveType.Side,
		WaveType.Line,
		WaveType.Line,
		WaveType.Side,
		WaveType.Side,
		WaveType.Line,
		WaveType.Line,
		WaveType.Side,
		//WaveType.Loop,
		WaveType.EliteVisit,
		WaveType.Line,
		WaveType.Side,
		WaveType.Line,
		WaveType.Side,
		WaveType.Line,
		WaveType.Line,
		WaveType.Side,
		WaveType.EliteVisit,
		//WaveType.Loop,
		WaveType.Line,
		WaveType.Side,
		WaveType.Line,
		WaveType.Line,
		WaveType.Side,
		WaveType.Line,
		WaveType.Side,
		WaveType.EliteBoss };
		
	
	
    IEnumerator SpawnStuff() {
        print ("Spawn routine started");
		
		foreach (WaveType w in roster)
		{
			switch (w)
			{
			case WaveType.EliteBoss:
				MakeEliteBoss();
				yield return new WaitForSeconds(2);
				break;
			case WaveType.EliteVisit:
				MakeEliteComeHang(5);
				yield return new WaitForSeconds(2);
				break;
			case WaveType.Line:
				SpawnHorizLine(8);
				yield return new WaitForSeconds(2);
				break;
				/*
			case WaveType.Loop:
				SpawnLoop(4);
				yield return new WaitForSeconds(2);
				break; (*/
			case WaveType.Side:
				SpawnSideSweep(8);
				yield return new WaitForSeconds(2);
				break;
			
			}
		}
		
		yield break;
	}
	
	/** Convenience methods for pseudo-scripting */
	
	//Spawns a bunch of enemies to the side of screen, which
	//	all run across that screen.
	void SpawnSideSweep(int numenemies)
	{
		for (int i=0; i < numenemies; i++)
		{
			Rigidbody b = ((GameObject)Instantiate(Resources.Load ("Prefabs/Level_2/L2_Enemy_Basic"))).rigidbody;
			b.position = new Vector3(16 + i*2f, 13, 0);
			b.rigidbody.velocity = new Vector3(-8, 0, 0);
		}
	}
	
	//Spawn a horizontal line of enemies from the top of the screen,
	// which all go down so as to try to crush the player.
	void SpawnHorizLine(int numenemies)
	{
		for (int i = 0; i < numenemies; i++)
		{
			Rigidbody b = ((GameObject)Instantiate(Resources.Load ("Prefabs/Level_2/L2_Enemy_Basic"))).rigidbody;
			b.position = new Vector3((i - numenemies/2)*2, 20, 0);
			b.rigidbody.velocity = new Vector3(0, -6, 0);
		}
	}
	
	//Spawn some enemies which come in from the side, and do a loopdeeloop.
	void SpawnLoop(int num)
	{
		///Actually, don't do this.
	}
	
	//Makes the Elite come and hang around for num seconds, then leave.
	void MakeEliteComeHang(float seconds)
	{
		//Temp: Just do something else
		//SpawnHorizLine(6);
		elite.AppearFor(seconds);
	}
	
	//Make the elite come and do a final boss fight.
	void MakeEliteBoss()
	{
		//SpawnHorizLine(10);
		elite.DoBoss();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
