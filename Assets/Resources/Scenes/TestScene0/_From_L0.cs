using UnityEngine;
using System.Collections;


/** This defines the exiting objects from Level 0.
 * 
 * This will be accessed by level 1 to find what Level 1
 *  needs to perform transitions on
 * 
 * If a Transition_L0 is created, it is assumed that Level 0
 *  was played before L1, and that some objects from L1 were
 *  carried over, expecting to be handled.
 * 
 */
public class _From_L0 : TransitionSet{

	//Camera is kept in the base class (TransitionSet)
	public GameObject player;
	public GameObject ship;
	public GameObject ground;
}
