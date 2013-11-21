using UnityEngine;
using System.Collections;

public class Stat_Counter_Script : MonoBehaviour {

    public int numberOfDeaths = 0;
    public float secondsSinceStart = 0;

    public bool endGame = false;

    // From index 0 - 3 (stars 2 - 5), determine which stars will show for which ratings
    public int[] deathThresholds = new int[4];
    public float[] timeThresholds = new float[4];    

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
                if (t.name.Equals("EXIT")) // Special Case for controller for this text
                {
                    if (Input.GetJoystickNames().Length == 0)
                    {
                        t.text = "Press Space To Exit";
                    }
                    else
                    {
                        t.text = "Press 'A' To Exit";
                    }
                }
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
                if (s.name.Contains("1"))
                {                    
                    s.enabled = true;
                }

                else if (s.name.Contains("2"))
                {
                    if (s.name.Contains("Survival"))
                    {
                        if (this.numberOfDeaths < deathThresholds[0])
                        {                            
                            s.enabled = true;
                        }
                        else
                        {
                            s.enabled = false;
                        }
                    }
                    else
                    {
                        if (this.secondsSinceStart < timeThresholds[0])
                        {                            
                            s.enabled = true;
                        }
                        else
                        {
                            s.enabled = false;
                        }
                    }
                }

                else if (s.name.Contains("3"))
                {
                    if (s.name.Contains("Survival"))
                    {
                        if (this.numberOfDeaths < deathThresholds[1])
                        {
                            s.enabled = true;
                        }
                        else
                        {
                            s.enabled = false;
                        }
                    }
                    else
                    {
                        if (this.secondsSinceStart < timeThresholds[1])
                        {
                            s.enabled = true;
                        }
                        else
                        {
                            s.enabled = false;
                        }
                    }
                }

                else if (s.name.Contains("4"))
                {
                    if (s.name.Contains("Survival"))
                    {
                        if (this.numberOfDeaths < deathThresholds[2])
                        {
                            s.enabled = true;
                        }
                        else
                        {
                            s.enabled = false;
                        }
                    }
                    else
                    {
                        if (this.secondsSinceStart < timeThresholds[2])
                        {
                            s.enabled = true;
                        }
                        else
                        {
                            s.enabled = false;
                        }
                    }
                }

                else if (s.name.Contains("5"))
                {
                    if (s.name.Contains("Survival"))
                    {
                        if (this.numberOfDeaths < deathThresholds[3])
                        {
                            s.enabled = true;
                        }
                        else
                        {
                            s.enabled = false;
                        }
                    }
                    else
                    {
                        if (this.secondsSinceStart < timeThresholds[3])
                        {
                            s.enabled = true;
                        }
                        else
                        {
                            s.enabled = false;
                        }
                    }
                }
                    
            }
        }
	}
}
