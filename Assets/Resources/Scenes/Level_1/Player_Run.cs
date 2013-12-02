using UnityEngine;
using System.Collections;

public class Player_Run : MonoBehaviour {
	
	int framenum = 0;
	float framewidth = .059f;
	public float frametime = .03f;
	int maxframe = 15;
	
	float nextframe;
	
	float airYpos = .0f;
	
	bool inAir = false;
	bool falling = false;
	bool wallReady = false;
	
	// Use this for initialization
	void Start () {
		nextframe = Time.time + frametime;
	}
	
	// Update is called once per frame
	void Update () {
		if (!inAir && !wallReady)
		{
			if (Time.time > nextframe)
			{
				framenum++;
				nextframe += frametime;
				if (framenum > maxframe) { framenum = 0; }
				this.renderer.material.SetTextureOffset("_MainTex",  new Vector2(framenum * framewidth, -.5f));
			}
		}
	}
	
	public void InAir() 
	{
		inAir = true;
		this.renderer.material.SetTextureOffset("_MainTex", new Vector2(0, airYpos));
	}
	
	public void Falling()
	{
		falling = true;
		this.renderer.material.SetTextureOffset("_MainTex", new Vector2(framewidth*2, airYpos));
	}
	
	public void WallJumpReady()
	{
		print ("WallJumpReady");
		wallReady = true;
		this.renderer.material.SetTextureOffset("_MainTex", new Vector2(framewidth, airYpos));
	}
	
	public void OnGround()
	{
		inAir = false;
		wallReady = false;
	}
}
