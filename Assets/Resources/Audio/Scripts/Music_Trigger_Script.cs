using UnityEngine;
using System.Collections;

public class Music_Trigger_Script : MonoBehaviour {

    public bool playOnStart = false; // If true, it will play the songs it is selected to on start

    public int levelChoice = 0;
    public int[] musicChoices; // Song indices you want to play in the AudioSourceManager

    /*ONLY use these values for a transition*/
    public int postTransitionLevelChoice = 0;
    public float postTransitionDelay = 0.0f;
    public int[] postTransitionMusicChoices; // Song indices you want to play in the AudioSourceManager


    public enum TRIGGER_TYPE { FADE_IN, FADE_OUT, TRANSITION_FADE_IN, START_PLAYING, QUICK_FADE_IN };
    public TRIGGER_TYPE triggerType = TRIGGER_TYPE.FADE_IN;

    public string tagToCompareFor = "Player";

    GameObject audioSourceManager;

    private bool isTriggered = false;

	// Use this for initialization
	void Start () {
        audioSourceManager = GameObject.FindGameObjectWithTag("AudioSourceManager");

        if (playOnStart)
        {
            if (triggerType == TRIGGER_TYPE.FADE_IN)
            {
                audioSourceManager.GetComponent<Music_Manager_Script>().FadeInSongs(levelChoice, musicChoices);
            }

            if (triggerType == TRIGGER_TYPE.FADE_OUT)
            {
                audioSourceManager.GetComponent<Music_Manager_Script>().FadeOutSongs(levelChoice, musicChoices);
            }

            if (triggerType == TRIGGER_TYPE.TRANSITION_FADE_IN)
            {
                audioSourceManager.GetComponent<Music_Manager_Script>().FadeInTransitions(levelChoice, musicChoices, postTransitionLevelChoice, postTransitionMusicChoices, postTransitionDelay);
            }
            
            if (triggerType == TRIGGER_TYPE.START_PLAYING)
            {
                audioSourceManager.GetComponent<Music_Manager_Script>().StartSongs(levelChoice, musicChoices);
            }

            if (triggerType == TRIGGER_TYPE.QUICK_FADE_IN)
            {
                audioSourceManager.GetComponent<Music_Manager_Script>().QuickFadeInSongs(levelChoice, musicChoices);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(tagToCompareFor))
        {
            if (!playOnStart && !isTriggered)
            {
                if (triggerType == TRIGGER_TYPE.FADE_IN)
                {
                    audioSourceManager.GetComponent<Music_Manager_Script>().FadeInSongs(levelChoice, musicChoices);
                    isTriggered = true;
                }

                if (triggerType == TRIGGER_TYPE.FADE_OUT)
                {
                    audioSourceManager.GetComponent<Music_Manager_Script>().FadeOutSongs(levelChoice, musicChoices);
                    isTriggered = true;
                }

                if (triggerType == TRIGGER_TYPE.TRANSITION_FADE_IN)
                {
                    audioSourceManager.GetComponent<Music_Manager_Script>().FadeInTransitions(levelChoice, musicChoices, postTransitionLevelChoice, postTransitionMusicChoices, postTransitionDelay);
                    isTriggered = true;
                }

                if (triggerType == TRIGGER_TYPE.START_PLAYING)
                {
                    audioSourceManager.GetComponent<Music_Manager_Script>().StartSongs(levelChoice, musicChoices);
                    isTriggered = true;
                }

                if (triggerType == TRIGGER_TYPE.QUICK_FADE_IN)
                {
                    audioSourceManager.GetComponent<Music_Manager_Script>().QuickFadeInSongs(levelChoice, musicChoices);
                    isTriggered = true;
                }
            }
        }
    }
}
