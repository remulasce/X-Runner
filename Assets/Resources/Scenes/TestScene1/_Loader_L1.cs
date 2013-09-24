using UnityEngine;
using System.Collections;

public class _Loader_L1 : MonoBehaviour {
	
	//Convenience
	public static _Loader_L1 loader;
	//We will fill this with useful things, based on what the previous level
	// gave us.
	//If we didn't get anything from _From_L0, we must be playing on our own,
	// so will create our own "dummy" objects from the previous scene.
	//That way, everything in this level can count on _Loader_L1.start to be
	// filled with sensible data.
	public static _To_L1 start;
	
	
	// Use this for initialization
	void Start () {
		//Setup convenience singletons
		loader = this;
		start  = new _To_L1();
		
		//We'll take stuff the previous level gives to us and put it in
		// a way this level can
		if (LoadHolder.transitionSet is _From_L0)
		{
			print ("Transitioning from L0");
			//Here we know what to do with everything.
			_From_L0 prev = (_From_L0)LoadHolder.transitionSet;
			
			//Disable the previous camera
			prev.camera.enabled = false;
			
			start.camera_pos = prev.camera.transform.position;
			start.camera_rot = prev.camera.transform.rotation;
			start.ground = prev.ground;
			start.player = prev.player;
			start.ship = prev.ship;
			
		}
		else if (LoadHolder.transitionSet is _From_L1)
		{
			//Why?? But, this is an example.
		}
		else
		{
			//We are loading from scratch. Make up our own _To_L1, because
			// everything else expects stuff to be there.
			start.camera_pos = new Vector3(0,0,0);
			start.camera_rot = Quaternion.identity;
			start.ground = new GameObject();
			start.player = new GameObject();
			
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
