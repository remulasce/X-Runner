using UnityEngine;
using System.Collections;

public class Level_Load_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Jump"))
        {
            Application.LoadLevel("Level_1_Graybox");
        }
	}

    
}
