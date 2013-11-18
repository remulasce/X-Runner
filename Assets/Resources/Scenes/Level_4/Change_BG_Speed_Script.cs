using UnityEngine;
using System.Collections;

public class Change_BG_Speed_Script : MonoBehaviour {

    public GameObject background = null;
    public float speed;

    public bool endGame = false;

    public Fireworks_Script fireworksScript = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            background.GetComponent<L4_Background>().speed = this.speed;
            if (endGame)
            {                
                if (fireworksScript)
                {
                    fireworksScript.isEnabled = true;
                }
                GameObject.FindGameObjectWithTag("Stats").GetComponent<Stat_Counter_Script>().endGame = true;
            }
        }
    }
}
