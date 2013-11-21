using UnityEngine;
using System.Collections;

public class Animation_TakeOff_Script_L4 : MonoBehaviour {

    private bool isStopped = false;

    public Music_Manager_Script musicManager;

    // Use this for initialization
    void Start()
    {
        musicManager = GameObject.FindGameObjectWithTag("AudioSourceManager").GetComponent<Music_Manager_Script>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isStopped)
        {
            Object.Destroy(GameObject.FindGameObjectWithTag("Stats"));

            if (musicManager)
            {
                musicManager.QuickFadeOuts(4, new int[] { 5, 6 });
                musicManager.StopAllSongs();
                musicManager.StartSongs(0, new int[] { 0 });
            }

            Application.LoadLevel("Title_Screen");
        }
	}

    void PlayTakeOff()
    {
        audio.Play();
    }

    void LoadL0()
    {
        isStopped = true;
    }
}
