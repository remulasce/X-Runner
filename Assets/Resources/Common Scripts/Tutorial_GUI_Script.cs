using UnityEngine;
using System.Collections;

public class Tutorial_GUI_Script : MonoBehaviour {

    public float timeToDisappear = 0.0f;

    private float startTime = 0.0f;

    public bool setOnWithStart = false;

	// Use this for initialization
	void Start () {
        if (!setOnWithStart)
        {
            this.gameObject.GetComponent<GUITexture>().enabled = false;
        }
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (timeToDisappear == 0.0f)
        {
            return;
        }

        if (Time.time - startTime > timeToDisappear && this.enabled)
        {
            this.gameObject.GetComponent<GUITexture>().enabled = false;
        }
	}

    public void setOn()
    {
        this.gameObject.GetComponent<GUITexture>().enabled = true;
        startTime = Time.time;
    }
}
