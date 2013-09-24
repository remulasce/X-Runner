using UnityEngine;
using System.Collections;

public class LoadHolder {
	
	//Each level will check for a TransitionSet upon load.
	//The TransitionSet should contain everything that can
	//	be seen by the player in the previous level, so
	//	the level currently starting can transition from
	//	that into itself.
	//If it is null, the level will assume that there is no
	//	previous level, eg. it is being tested, and thus
	//	will either skip the transition or come up with
	//	its own dummy values.
	public static TransitionSet transitionSet;
	

}
