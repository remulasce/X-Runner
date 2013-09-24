using UnityEngine;
using System.Collections;


//Loaders deal with the unpacking and repacking of
// objects at the start and end of levels.
//They unpack the previous level, assign defaults
// as necessary, and at the end of level, pack
// necessary objects into a TransitionSet
//
//They should always be set to run first, via 
//	Edit->Project Settings->Script Execution Order

public class _Loader_L0 : MonoBehaviour {
	
	//Convenience
	public static _Loader_L0 loader;
	//Nothing in here because we create everything in this level.
	public static _To_L0 start;
	
	//Put all the loadie-type stuff here, so we know what
	//	actually gets loaded.
	//Unless it's dynamically tagged or doesn't exist
	//	in the editor yet.
	public Camera theCamera;
	public GameObject player;
	public GameObject ship;
	public GameObject ground;
	
	public void Start()
	{
		//Set ourself as the static loader, for convenience.
		loader = this;
		
		//No transition or loading. It's the first level.
	}
	
	//Pack everything up for Level 1, and then load L1.
	public void EndLevel()
	{
		print ("Ending Level 0");
		
		//Set each object to not die when the next level loads
		DontDestroyOnLoad(theCamera);
		DontDestroyOnLoad(ground);
		//Don't forget to not kill ourselves
		DontDestroyOnLoad(this);
		
		//This is what we pack our level into.
		_From_L0 pack = new _From_L0();
		

		pack.ship = ship;
		pack.player = player;
		pack.camera = theCamera;
		pack.ground = ground;
		
		LoadHolder.transitionSet = pack;
		Application.LoadLevel("Test_Scene_1");
	}
	
}
