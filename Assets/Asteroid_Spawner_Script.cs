﻿using UnityEngine;
using System.Collections;

public class Asteroid_Spawner_Script : MonoBehaviour {

    public enum ENABLE_STATE { OFF, ON_CINEMATIC, ON_GAMEPLAY };
    public ENABLE_STATE state = ENABLE_STATE.OFF;

    public float maxTimeToSpawnAsteroid = 4f;
    public float lastSpawnedAsteroidTime = 0.0f;

    public float screenLength = 30;
    public float screenWidth = 20;

    private float timeToSpawnAsteroid = 0.0f;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - lastSpawnedAsteroidTime > timeToSpawnAsteroid && state != ENABLE_STATE.OFF)
        {
            if (state == ENABLE_STATE.ON_CINEMATIC)
            {
                SpawnCinematicAsteroid();
            }
            else if (state == ENABLE_STATE.ON_GAMEPLAY)
            {
                SpawnGameAsteroid();
            }
        }
	}

    void SpawnCinematicAsteroid()
    {        
        lastSpawnedAsteroidTime = Time.time;
        timeToSpawnAsteroid = Random.Range(maxTimeToSpawnAsteroid/2, maxTimeToSpawnAsteroid);
        GameObject asteroid = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/L2_Asteroid_Cinematic"), this.transform.position, Quaternion.Euler(0, 0, 0));
        asteroid.transform.localScale *= Random.Range(0.5f, 1.5f); // Make the asteroid bigger so it looks like it is in the foreground
        asteroid.transform.position = new Vector3((asteroid.transform.localScale.x / 2.0f) + Random.Range(0, screenLength - (asteroid.transform.localScale.x * 1.5f)), 50f, 0);
        asteroid.transform.position += new Vector3(-screenLength / 2.0f, 0, 0);
        asteroid.rigidbody.mass = asteroid.rigidbody.mass * asteroid.transform.localScale.x;
        asteroid.rigidbody.AddForce(new Vector3(0, -1, 0) * Random.Range(200, 1000));
    }

    void SpawnGameAsteroid()
    {
        lastSpawnedAsteroidTime = Time.time;
        timeToSpawnAsteroid = Random.Range(maxTimeToSpawnAsteroid / 2, maxTimeToSpawnAsteroid);
        GameObject asteroid = (GameObject)Instantiate(Resources.Load("Prefabs/Level_2/L2_Asteroid"), this.transform.position, Quaternion.Euler(0, 0, 0));
        asteroid.transform.localScale *= Random.Range(1.0f, 2.5f); // Make the asteroid bigger so it looks like it is in the foreground
        asteroid.transform.position = new Vector3((asteroid.transform.localScale.x / 2.0f) + Random.Range(0, screenLength - (asteroid.transform.localScale.x * 1.5f)), 50f, 0);
        asteroid.transform.position += new Vector3(-screenLength / 2.0f, 0, 0);
        asteroid.rigidbody.mass = asteroid.rigidbody.mass * asteroid.transform.localScale.x;
        asteroid.rigidbody.AddForce(new Vector3(0, -1, 0) * Random.Range(1500, 5000));
    }
}
