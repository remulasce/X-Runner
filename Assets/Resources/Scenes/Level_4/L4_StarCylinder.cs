using UnityEngine;
using System.Collections;

public class L4_StarCylinder : MonoBehaviour {
	
	public float rotSpeedPerMin = 60;
	public float transSpeed = 2000;
	public bool started = true;
	
	private bool headOn = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (started)
		{
			if (!headOn)
			{
				this.transform.Rotate(0, 0, -rotSpeedPerMin / 60 * Time.deltaTime);
			}
			else
			{
				this.transform.Translate(-transSpeed * Time.deltaTime, 0, 0);
			}
		}
	}
	
	/** LookHeadOn switches to translation in X axis. */
	void LookHeadOn()
	{
		this.transform.position = new Vector3(0,0,0);
		headOn = true;
	}
	
	/** LookSide rotates the skycube. */
	void LookSide()
	{
		this.transform.position = new Vector3(0,0,0);
		headOn = false;
	}
}
