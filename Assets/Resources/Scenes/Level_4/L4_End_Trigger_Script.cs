using UnityEngine;
using System.Collections;

public class L4_End_Trigger_Script : MonoBehaviour {

    public Music_Manager_Script musicManager;

	// Use this for initialization
	void Start () {
        musicManager = GameObject.FindGameObjectWithTag("AudioSourceManager").GetComponent<Music_Manager_Script>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Object.Destroy(GameObject.FindGameObjectWithTag("Stats"));

            musicManager.QuickFadeOuts(4, new int[] { 5, 6 });            
            musicManager.StopAllSongs();
            musicManager.StartSongs(0, new int[] { 0 });

            Application.LoadLevel("Title_Screen");
        }
    }
}
