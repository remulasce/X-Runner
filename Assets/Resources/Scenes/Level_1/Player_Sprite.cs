using UnityEngine;
using System.Collections;

public class Player_Sprite : MonoBehaviour {
	
	float framelength = .025f;
	float lastChange;
	int anState = 0;
	float charwidth = .0585f;//.115f;
	int numchars = 16;
	
	// Use this for initialization
	void Start () {
		lastChange = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Time.time > lastChange + framelength)
		{
			lastChange = Time.time;
			
			anState++;
			if (anState > numchars-1) { anState = 0; }
			
			this.renderer.material.SetTextureOffset("_MainTex", new Vector2(anState * charwidth, 0));
			
		}
		
	}
}
