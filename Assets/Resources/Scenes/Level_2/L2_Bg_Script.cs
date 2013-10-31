using UnityEngine;
using System.Collections;

public class L2_Bg_Script : MonoBehaviour {
	
	public Transform other;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (this.transform.position.y <= -60)
		{
			this.transform.position = other.position + new Vector3(0, 60, 0.1f);
		}
		
		this.transform.position = this.transform.position - new Vector3(0, 5*Time.deltaTime, 0);
		
	}
}
