using UnityEngine;
using System.Collections;

public class Fireworks_Script : MonoBehaviour {

    public float timeIntervalToSpawnFirework = 1.0f;
    public Vector3 spawningArea = new Vector3(10, 10, 10);
    
    // x = small size
    // y = large size
    public Vector2 sizes = new Vector2(5, 10);

    public bool isEnabled = false;

    private float startTime = 0.0f;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - startTime > (timeIntervalToSpawnFirework))
        {
            if (Random.Range(0, 10) == 0 && isEnabled)
            {                
                SpawnFirework();
            }
        }
	}

    void SpawnFirework()
    {
        GameObject firework = (GameObject)Instantiate(Resources.Load("Prefabs/Level_4/Firework"));
        firework.transform.position = new Vector3(Random.Range(-spawningArea.x, spawningArea.x), Random.Range(-spawningArea.y, spawningArea.y), Random.Range(-spawningArea.z, spawningArea.z));
        firework.GetComponent<Detonator>().size = Random.Range(sizes.x, sizes.y);
        firework.GetComponent<Detonator>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
        firework.GetComponent<DetonatorFireball>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
        firework.GetComponent<DetonatorHeatwave>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
        firework.GetComponent<DetonatorShockwave>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
        firework.GetComponent<DetonatorSpray>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
        firework.GetComponent<DetonatorSparks>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
        firework.GetComponent<DetonatorLight>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
        firework.GetComponent<Detonator>().Explode();
        startTime = Time.time;
    }
}
