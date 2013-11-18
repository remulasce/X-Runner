using UnityEngine;
using System.Collections;

public class Destroy_Upon_Emit_Timed_Script : MonoBehaviour {

    public float destroyTime = 3.0f;

    private float startTime = 0.0f;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (this.GetComponent<ParticleEmitter>().emit && startTime == 0)
        {
            startTime = Time.time;
        }
        if (Time.time - startTime > destroyTime)
        {
            this.GetComponent<ParticleEmitter>().emit = false;
        }
	}
}
