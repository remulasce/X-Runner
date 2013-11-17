using UnityEngine;
using System.Collections;

public class Music_Manager_Script : MonoBehaviour {

    public AudioSource[] Level_0_Compositions;
    public AudioSource[] Level_1_Compositions;
    public AudioSource[] Level_2_Compositions;
    public AudioSource[] Level_3_Compositions;
    public AudioSource[] Level_4_Compositions;

	// Use this for initialization
	void Start () {
        Object.DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartSongs(int levelnumber, int[] values)
    {
        if (levelnumber == 0)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_0_Compositions[values[i]].Stop();
                Level_0_Compositions[values[i]].Play();         
            }
        }

        else if (levelnumber == 1)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_1_Compositions[values[i]].Stop();
                Level_1_Compositions[values[i]].Play();        
            }
        }

        else if (levelnumber == 2)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_2_Compositions[values[i]].Stop();
                Level_2_Compositions[values[i]].Play();             
            }
        }

        else if (levelnumber == 3)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_3_Compositions[values[i]].Stop();
                Level_3_Compositions[values[i]].Play();             
            }
        }

        else if (levelnumber == 4)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_4_Compositions[values[i]].Stop();
                Level_4_Compositions[values[i]].Play();              
            }
        }
    }

    //---------------------------------------------------------------

    public void QuickFadeInSongs(int levelnumber, int[] values)
    {
        if (levelnumber == 0)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_0_Compositions[values[i]].animation.Play("Quick_Fade_In");
            }
        }

        else if (levelnumber == 1)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_1_Compositions[values[i]].animation.Play("Quick_Fade_In");
            }
        }

        else if (levelnumber == 2)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_2_Compositions[values[i]].animation.Play("Quick_Fade_In");
            }
        }

        else if (levelnumber == 3)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_3_Compositions[values[i]].animation.Play("Quick_Fade_In");
            }
        }

        else if (levelnumber == 4)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_4_Compositions[values[i]].animation.Play("Quick_Fade_In");
            }
        }
    }

    //---------------------------------------------------------------

    public void FadeInSongs(int levelnumber, int[] values)
    {
        if (levelnumber == 0)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_0_Compositions[values[i]].animation.Play("Fade_In");
            }
        }

        else if (levelnumber == 1)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_1_Compositions[values[i]].animation.Play("Fade_In");
            }
        }

        else if (levelnumber == 2)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_2_Compositions[values[i]].animation.Play("Fade_In");
            }
        }

        else if (levelnumber == 3)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_3_Compositions[values[i]].animation.Play("Fade_In");
            }
        }

        else if (levelnumber == 4)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_4_Compositions[values[i]].animation.Play("Fade_In");
            }
        }
    }

    //---------------------------------------------------------------------

    public void FadeOutSongs(int levelnumber, int[] values)
    {
        if (levelnumber == 0)
        {
            for (int i = 0; i < values.Length; i++)
            {                
                Level_0_Compositions[values[i]].animation.Play("Fade_Out");
            }
        }

        else if (levelnumber == 1)
        {
            for (int i = 0; i < values.Length; i++)
            {                
                Level_1_Compositions[values[i]].animation.Play("Fade_Out");
            }
        }

        else if (levelnumber == 2)
        {
            for (int i = 0; i < values.Length; i++)
            {                
                Level_2_Compositions[values[i]].animation.Play("Fade_Out");
            }
        }

        else if (levelnumber == 3)
        {
            for (int i = 0; i < values.Length; i++)
            {                
                Level_3_Compositions[values[i]].animation.Play("Fade_Out");
            }
        }

        else if (levelnumber == 4)
        {
            for (int i = 0; i < values.Length; i++)
            {                
                Level_4_Compositions[values[i]].animation.Play("Fade_Out");
            }
        }
    }

    //---------------------------------------------------------------------

    // Coroutine for playing the delayed song
    IEnumerator PlayPostLevelTransitionSong(int postTransitionLevelChoice, int[] postTransitionMusicChoices, float postTransitionDelay)
    {
        yield return new WaitForSeconds(postTransitionDelay);
        if (postTransitionLevelChoice == 0)
        {
            for (int i = 0; i < postTransitionMusicChoices.Length; i++)
            {
                Level_0_Compositions[postTransitionMusicChoices[i]].Play();
                Level_0_Compositions[postTransitionMusicChoices[i]].animation.Play("Quick_Fade_In");                
            }

            // Run through the rest of the songs, if any of them need to loop, play them too
            for (int i = 0; i < Level_0_Compositions.Length; i++)
            {
                if (Level_0_Compositions[i].loop)
                {
                    Level_0_Compositions[i].Play();
                }                
            }
        }

        else if (postTransitionLevelChoice == 1)
        {
            for (int i = 0; i < postTransitionMusicChoices.Length; i++)
            {
                Level_1_Compositions[postTransitionMusicChoices[i]].Play();
                Level_1_Compositions[postTransitionMusicChoices[i]].animation.Play("Quick_Fade_In");                
            }

            // Run through the rest of the songs, if any of them need to loop, play them too
            for (int i = 0; i < Level_1_Compositions.Length; i++)
            {
                if (Level_1_Compositions[i].loop)
                {
                    Level_1_Compositions[i].Play();
                }
            }
        }

        else if (postTransitionLevelChoice == 2)
        {
            for (int i = 0; i < postTransitionMusicChoices.Length; i++)
            {
                Level_2_Compositions[postTransitionMusicChoices[i]].Play();
                Level_2_Compositions[postTransitionMusicChoices[i]].animation.Play("Quick_Fade_In");                
            }

            // Run through the rest of the songs, if any of them need to loop, play them too
            for (int i = 0; i < Level_2_Compositions.Length; i++)
            {
                if (Level_2_Compositions[i].loop)
                {
                    Level_2_Compositions[i].Play();
                }
            }
        }

        else if (postTransitionLevelChoice == 3)
        {
            for (int i = 0; i < postTransitionMusicChoices.Length; i++)
            {
                Level_3_Compositions[postTransitionMusicChoices[i]].Play();
                Level_3_Compositions[postTransitionMusicChoices[i]].animation.Play("Quick_Fade_In");                
            }

            // Run through the rest of the songs, if any of them need to loop, play them too
            for (int i = 0; i < Level_3_Compositions.Length; i++)
            {
                if (Level_3_Compositions[i].loop)
                {
                    Level_3_Compositions[i].Play();
                }
            }
        }

        else if (postTransitionLevelChoice == 4)
        {
            for (int i = 0; i < postTransitionMusicChoices.Length; i++)
            {
                Level_4_Compositions[postTransitionMusicChoices[i]].Play();
                Level_4_Compositions[postTransitionMusicChoices[i]].animation.Play("Quick_Fade_In");                
            }

            // Run through the rest of the songs, if any of them need to loop, play them too
            for (int i = 0; i < Level_4_Compositions.Length; i++)
            {
                if (Level_4_Compositions[i].loop)
                {
                    Level_4_Compositions[i].Play();
                }
            }
        }
    }

    //---------------------------------------------------------------------

    public void FadeInTransitions(int levelnumber, int[] values, int postTransitionLevelChoice, int[] postTransitionMusicChoices, float postTransitionDelay)
    {
        if (levelnumber == 0)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_0_Compositions[values[i]].Play();
                Level_0_Compositions[values[i]].animation.Play("Fade_In_Transition");
                StartCoroutine(PlayPostLevelTransitionSong(postTransitionLevelChoice, postTransitionMusicChoices, postTransitionDelay));
            }
        }

        else if (levelnumber == 1)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_1_Compositions[values[i]].Play();
                Level_1_Compositions[values[i]].animation.Play("Fade_In_Transition");
                StartCoroutine(PlayPostLevelTransitionSong(postTransitionLevelChoice, postTransitionMusicChoices, postTransitionDelay));
            }
        }

        else if (levelnumber == 2)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_2_Compositions[values[i]].Play();
                Level_2_Compositions[values[i]].animation.Play("Fade_In_Transition");
                StartCoroutine(PlayPostLevelTransitionSong(postTransitionLevelChoice, postTransitionMusicChoices, postTransitionDelay));
            }
        }

        else if (levelnumber == 3)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_3_Compositions[values[i]].Play();
                Level_3_Compositions[values[i]].animation.Play("Fade_In_Transition");
                StartCoroutine(PlayPostLevelTransitionSong(postTransitionLevelChoice, postTransitionMusicChoices, postTransitionDelay));
            }
        }

        else if (levelnumber == 4)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_4_Compositions[values[i]].Play();
                Level_4_Compositions[values[i]].animation.Play("Fade_In_Transition");
                StartCoroutine(PlayPostLevelTransitionSong(postTransitionLevelChoice, postTransitionMusicChoices, postTransitionDelay));
            }
        }
    }

    //---------------------------------------------------------------------

    public void QuickFadeOuts(int levelnumber, int[] values)
    {
        if (levelnumber == 0)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_0_Compositions[values[i]].animation.Play("Quick_Fade_Out");
            }
        }

        else if (levelnumber == 1)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_1_Compositions[values[i]].animation.Play("Quick_Fade_Out");
            }
        }

        else if (levelnumber == 2)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_2_Compositions[values[i]].animation.Play("Quick_Fade_Out");
            }
        }

        else if (levelnumber == 3)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_3_Compositions[values[i]].animation.Play("Quick_Fade_Out");
            }
        }

        else if (levelnumber == 4)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Level_4_Compositions[values[i]].animation.Play("Quick_Fade_Out");
            }
        }
    }
}
