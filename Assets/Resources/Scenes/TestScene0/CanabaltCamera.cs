using UnityEngine;
using System.Collections;

public class CanabaltCamera : MonoBehaviour {
	
	public GameObject player;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject.activeSelf)
		{
			Vector3 tempVector = transform.position;
            tempVector.x = player.transform.position.x + 8;
            tempVector.y = player.transform.position.y;
            transform.position = tempVector;
		}
	}
}
