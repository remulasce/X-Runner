using UnityEngine;
using System.Collections;

public class Advance_To_Title_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void LateUpdate()
    {
        //Application.LoadLevel("Level_4_Boss");
        Application.LoadLevel("Title_Screen");
    }
}
