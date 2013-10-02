using UnityEngine;
using System.Collections;

public class CanabaltCamera : MonoBehaviour {
	
	public GameObject player;

    public float moveOffsetX = 8.0f;
    public float moveOffsetY = 1.0f;

    // Used for camera zooming
    [Range(-5.0f, -100.0f)]
    public float startingZPosition = -9.0f;

    [Range(-5.0f, -100.0f)]
    public float endZoomPoint = -20.0f;

    [Range(0.001f, 1.0f)]
    public float zoomSpeed = 0.001f;

	// Use this for initialization
	void Start () {
        this.transform.position = new Vector3(player.transform.position.x + moveOffsetX, player.transform.position.y + moveOffsetY, startingZPosition);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject.activeSelf)
		{
			Vector3 tempVector = transform.position;
            tempVector.x = player.transform.position.x + moveOffsetX;
            if (player.GetComponent<Player_Movement_Script>().isInAir)
            {
                tempVector.y = player.transform.position.y + moveOffsetY;
            }
            transform.position = tempVector;
		}
	}
}