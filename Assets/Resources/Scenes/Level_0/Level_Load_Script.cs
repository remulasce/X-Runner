using UnityEngine;
using System.Collections;

public class Level_Load_Script : MonoBehaviour {

    AudioSource[] audios;

    public GUIText time = null;
    public float timeTrailer = 15.85f + 24f;

    private float timeToSecond = 0.0f;

	// Use this for initialization
	void Start () {
        audios = this.gameObject.GetComponents<AudioSource>();
        GameObject stats = GameObject.FindGameObjectWithTag("Stats");
        StartCoroutine("StartMovie");
        if (stats)
        {
            Object.Destroy(stats);
        }        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Jump"))
        {
            audios[0].Play();
            StartCoroutine("FadeOutIntroSong");
            Application.LoadLevel("Level_1_Graybox");
        }
        if (time)
        {
            timeTrailer -= Time.deltaTime;
            timeToSecond += Time.deltaTime;
            time.text = "Time to Trailer: " + Mathf.Round(timeTrailer).ToString();
            if (timeToSecond >= 1.0f)
            {
                timeToSecond = 0.0f;
                audios[1].Play();
            }
        }
	}

    IEnumerator FadeOutIntroSong()
    {
        yield return new WaitForEndOfFrame();
        Music_Manager_Script musicManager = GameObject.FindGameObjectWithTag("AudioSourceManager").GetComponent<Music_Manager_Script>();
        musicManager.QuickFadeOuts(0, new int[] { 0, 1 });        
    }

    IEnumerator StartMovie()
    {
        yield return new WaitForSeconds(15.85f + 24f);
        Music_Manager_Script musicManager = GameObject.FindGameObjectWithTag("AudioSourceManager").GetComponent<Music_Manager_Script>();
        musicManager.QuickFadeOuts(0, new int[] { 0, 1 });        
        Application.LoadLevel("Title_Movie");
    }

    
}
