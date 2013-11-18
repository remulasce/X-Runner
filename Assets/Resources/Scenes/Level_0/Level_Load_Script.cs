using UnityEngine;
using System.Collections;

public class Level_Load_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject stats = GameObject.FindGameObjectWithTag("Stats");
        if (stats)
        {
            Object.Destroy(stats);
        }        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Jump"))
        {
            StartCoroutine("FadeOutIntroSong");
            Application.LoadLevel("Level_1_Graybox");
        }
	}

    IEnumerator FadeOutIntroSong()
    {
        yield return new WaitForEndOfFrame();
        Music_Manager_Script musicManager = GameObject.FindGameObjectWithTag("AudioSourceManager").GetComponent<Music_Manager_Script>();
        musicManager.QuickFadeOuts(0, new int[] { 0, 1 });
    }

    
}
