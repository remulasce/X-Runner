using UnityEngine;
using System.Collections;

public class L2_Elite_Script : MonoBehaviour {
	
	Vector3 targetPos;
	Vector3 dir;
	float lastdirchange = -1;
	
	int health = 20;
	
	float hideTime;
	bool inplay = false;
	bool boss = false;	//if we're in boss mode
	
	// Use this for initialization
	void Start () {
		targetPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (inplay)
		{
			DoShooting();
		}
		
		if (Time.time > hideTime && inplay)
		{
			targetPos = new Vector3(-20, 8, 0);
			inplay = false;
		}
		
		Vector3 d = targetPos - this.transform.position;
		if (d.magnitude > 4)
		{
			dir = d.normalized * 10;
		}
		
		else if (Time.time > lastdirchange + 1)
		{
			dir = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
			lastdirchange = Time.time;
		}
		
		
		
		this.transform.Translate(dir * Time.deltaTime);
		
	}
	
	private float lastShot = 0;
	void DoShooting()
	{
		if (Time.time > lastShot + .75f)
		{
			GameObject.Instantiate(Resources.Load("Prefabs/Level_2/L2_Enemy_Shot_Homing"), this.transform.position, Quaternion.identity);
			lastShot = Time.time + Random.Range(-.1f, .1f);
				
		}
	}
	
	
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("L2_PlayerShot"))
		{
			//Don't die too early!
			if (!boss && health <= 5)
			{
				return;
			}
			health--;
			if (health <= 0)
			{
				Application.LoadLevel ("Level_3_Graybox");
			}
		}
	}
	
	/** Temporary(?) Elite control. It tries to follow what's in the wave. */
	public void DoWave(L2_Enemy_Spawner.Wave w)
	{
		switch (w.ft.type)
		{
		case L2_Enemy_Spawner.FormationType.T.ElitePass:
			AppearFor(3);
			break;
		case L2_Enemy_Spawner.FormationType.T.EliteStayBack:
			AppearFor(6);
			break;
		case L2_Enemy_Spawner.FormationType.T.EliteBattle:
			DoBoss();
			break;
		}
	}
	
	public void Hide()
	{
		
	}
	
	public void AppearFor(float seconds)
	{
		inplay = true;
		targetPos = new Vector3(Random.Range(-10f, 10f), Random.Range(4f, 12f), 0);
		hideTime = Time.time + seconds;
	}
	
	public void DoBoss()
	{
		targetPos = new Vector3(Random.Range(-10f, 10f), Random.Range(4f, 12f), 0);
		hideTime = Time.time + 1000;
		boss = true;
	}
	
}
