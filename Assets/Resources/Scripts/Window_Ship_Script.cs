using UnityEngine;
using System.Collections;

public class Window_Ship_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!animation.isPlaying)
        {
            Object.Destroy(this.gameObject);
        }
	}
}
