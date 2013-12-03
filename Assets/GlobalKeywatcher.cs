using UnityEngine;
using System.Collections;

public class GlobalKeywatcher : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			//Application.LoadLevel(Application.loadedLevelName);
		}
		
	}
}
