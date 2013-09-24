using UnityEngine;
using System.Collections;


/** TransitionSet is the base holder for levels to provide
 * info about their current state, so that the next level
 * can transition properly.
 * 
 *  Each level will have its own derivation of TransitionSet,
 * containing the objects that it thinks are important for
 * the next level to smoothly transition out of.
 * 
 *  So each TransitionSet is objects /leaving/ a level.
 * 
 *  TransitionSet provides some default information that all
 * levels need to provide, so if the next level doesn't know
 * how to do a proper transition, they can at least fall back
 * and clean up after the previous level properly.
 * 
 *  Eg. All persistent objects need to be kept in a list, so
 * they can all be Delete'd if the level doesn't know how to find
 * every object carried over.
 * 
 *  Also, since Cameras are hard to destroy, they are kept
 * in the base class, so any level can deactivate them.
 */
public class TransitionSet {
	//We know this will be part of every scene, and every level
	// will have to use it to transition, so keep here.
	public Camera camera;
	
	//Note: This should be assumed to be an array of GameObjects
	//Also: This will never be null, only empty.
	//public ArrayList allObjects = new ArrayList();
	//Don't bother, this is annoying to set, and we're not going to use it anyway
}
