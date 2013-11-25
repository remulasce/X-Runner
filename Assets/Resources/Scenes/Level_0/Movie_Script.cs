using UnityEngine;
using System.Collections;

public class Movie_Script : MonoBehaviour {

    public MovieTexture texture = new MovieTexture();

    Music_Manager_Script musicManager;

	// Use this for initialization
	void Start () {
        StartCoroutine("SwitchToTitle");
        musicManager = GameObject.FindGameObjectWithTag("AudioSourceManager").GetComponent<Music_Manager_Script>();
	}

    void OnGUI()
    {
        if (!texture.isPlaying)
        {
            texture.Play();
        }
        if (!audio.isPlaying)
        {
            audio.Play();
        }
    }

    IEnumerator SwitchToTitle()
    {
        yield return new WaitForSeconds(60);
        Application.LoadLevel("Title_Screen");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {            
            Application.LoadLevel("Title_Screen");
        }
	}
}
