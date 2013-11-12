using UnityEngine;
using System.Collections;

public class Stat_Counter_Script : MonoBehaviour {

    public int numberOfDeaths = 0;
    public float secondsSinceStart = 0;

    public bool endGame = false;

    // Rating can be from 1 to 5
    private int rating = 5;

    GUIText[] texts;
    GUITexture[] stars;

	// Use this for initialization
	void Start () {
		//Don't make 2 stat counters
		if (GameObject.FindGameObjectsWithTag("Stats").Length == 2)
		{
			Destroy(this);
		}
		
        texts = this.GetComponentsInChildren<GUIText>();
        stars = this.GetComponentsInChildren<GUITexture>();
        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {        

        if (!endGame)
        {
            secondsSinceStart += Time.deltaTime; // Update the time
            foreach (GUIText t in texts)
            {
                if (t.name.Contains("Deaths"))
                {
                    t.text = "Deaths: " + numberOfDeaths;
                }
                else if (t.name.Contains("Time"))
                {
                    // Make a better time tracking algorithm sometime
                    t.text = "Time: " + secondsSinceStart;
                }
                else
                {
                    t.enabled = false;
                }
            }
            foreach (GUITexture s in stars)
            {
                s.enabled = false;
            }
        }
        else
        {
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
                else
                {
                    t.enabled = true;
                }
            }
            foreach (GUITexture s in stars)
            {
                s.enabled = true;
            }
        }
	}
}
