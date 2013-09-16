using UnityEngine;
using System.Collections;

public interface GameCameraBase {
	//Do camera updating in this fxn, not update
	//that way, we can take control for transitions
	void UpdateCamera();
	
	
}

//Wrapper class for position + rotation of where a
// camera should start
public class CameraPosition
{
	public Vector3 position;
	public Quaternion rotation;
}