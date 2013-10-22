using UnityEngine;
using System.Collections;

public class L2_Elite_Script : MonoBehaviour {
	
	Vector3 targetPos;
	Vector3 dir;
	float lastdirchange = -1;
	
	int health = 20;
	
	float hideTime;
	bool inplay = false;
	
	// Use this for initialization
	void Start () {
		targetPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Time.time > hideTime && inplay)
		{
			targetPos = new Vector3(-20, 10, 0);
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
	
	
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("L2_PlayerShot"))
		{
			health--;
			if (health <= 0)
			{
				Application.LoadLevel ("Level_1_Graybox");
			}
		}
	}
	
	/** Temporary(?) Elite control. It tries to follow what's in the wave. */
	public void DoWave(Wave w)
	{
		
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
	}
	
}
