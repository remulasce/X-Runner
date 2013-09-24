using UnityEngine;
using System.Collections;


//Level 1 needs to know about the L0 camera position, L0 player,
// L0 ship, and what the ground/scenery is.
public class _To_L1 {
	//Previous camera position, for transition
	public Vector3 camera_pos;
	public Quaternion camera_rot;
	
	public GameObject player;
	public GameObject ship;
	
	public GameObject ground;
}
