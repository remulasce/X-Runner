using UnityEngine;
using System.Collections;

public class Missile_Trail_Script : MonoBehaviour {

    Vector3 lastPosition;

	// Use this for initialization
	void Start () {
        lastPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Vector3.SqrMagnitude(this.transform.position - lastPosition) < 0.0001f);
        if (Vector3.SqrMagnitude(this.transform.position - lastPosition) < 0.0001f)
        {
            this.particleEmitter.emit = false;
            if (this.particleEmitter.particleCount == 0)
            {
                Object.Destroy(this.gameObject);
            }
        }
        lastPosition = this.transform.position;
	}
}
