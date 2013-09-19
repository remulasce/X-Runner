using UnityEngine;
using System.Collections;

public class ShipCamera : MonoBehaviour {
	
	public Transform follow;
	
	// Use this for initialization
	void Start () {
		//Snap us to the previous camera, or wherever, so we can
		//	do transition
		
		if (LoadHandler.level0Camera != null)
		{
			this.transform.position = LoadHandler.level0Camera.transform.position;
			this.transform.rotation = LoadHandler.level0Camera.transform.rotation;
			
			//Destroy(LoadHandler.level0Camera);
			LoadHandler.level0Camera.enabled = false;
			
			print (LoadHandler.level0Camera.enabled);
			
			follow = LoadHandler.level0ship;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		//hack
		Quaternion curRot = this.transform.rotation;
		this.transform.LookAt(follow);
		Quaternion targetRot = this.transform.rotation;
		
		Vector3 targetPos = new Vector3(follow.position.x, follow.position.y, follow.position.z - 10);
		
		this.transform.position = this.transform.position + 4f * Time.deltaTime * (targetPos-this.transform.position);
		this.transform.rotation = Quaternion.Lerp(curRot, targetRot, 4f * Time.deltaTime);
		
		
	}
}
