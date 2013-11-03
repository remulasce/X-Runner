using UnityEngine;
using System.Collections;

public class Stat_Counter_Script : MonoBehaviour {

    public int numberOfDeaths = 0;
    public float secondsSinceStart = 0;

    GUIText[] texts;

	// Use this for initialization
	void Start () {
        texts = this.GetComponentsInChildren<GUIText>();
        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        secondsSinceStart += Time.deltaTime;

        foreach (GUIText t in texts)
        {
            if (t.name.Contains("Deaths"))
            {
                t.text = "Deaths: " + numberOfDeaths;
            }
            if (t.name.Contains("Time"))
            {
                // Make a better time tracking algorithm sometime
                t.text = "Time: " + secondsSinceStart;
            }
        }
	}
}
