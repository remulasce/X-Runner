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

    [Range(0.001f, 5.0f)]
    public float zoomSpeed = 0.001f;

    private bool isZooming = false;
    private float currentZPosition = -9.0f;
    private float lerpValue = 0.0f;

    private bool isShaking = false;
    private float shakeDuration = 1.0f;
    private float shakePower = 1.0f;
    private float shakeStartTime = 1.0f;

	// Use this for initialization
	void Start () {
        this.transform.position = new Vector3(
            player.transform.position.x + moveOffsetX, 
            player.transform.position.y + moveOffsetY, 
            startingZPosition
        );
        currentZPosition = startingZPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject.activeSelf)
		{
			Vector3 tempVector = transform.position;
			
			//We're not necessarily following a player all the time
			if (player.CompareTag("Player"))
			{
                tempVector.x = player.transform.position.x + moveOffsetX - player.GetComponent<Player_Movement_Script>().horizontalMovement.playerOffset;
	            if (player.GetComponent<Player_Movement_Script>().isInAir && player.transform.position.y > 0.0f)
	            {
	                tempVector.y = player.transform.position.y + moveOffsetY;
	            }
			}
			else
			{
                tempVector.x = player.transform.position.x;
				tempVector.y = player.transform.position.y;			
			}
			
            transform.position = tempVector;
		}

        ZoomCamera();
        ShakeCamera();
	}

    public void ZoomToPosition(float zPos, float zSpeed)
    {
        endZoomPoint = zPos;
        zoomSpeed = zSpeed;
        isZooming = true;
    }

    private void ZoomCamera()
    {
        if (isZooming)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Mathf.Lerp(currentZPosition, endZoomPoint, Mathf.Clamp(lerpValue, 0.0f, 1.0f)));
            lerpValue += zoomSpeed * Time.deltaTime;

            if (lerpValue > 1.0f)
            {
                lerpValue = 0.0f;
                isZooming = false;
                currentZPosition = this.transform.position.z;
            }
        }
    }

    public void BeginShake(float sDuration, float sPower)
    {
        shakeDuration = sDuration;
        shakePower = sPower;
        isShaking = true;
        shakeStartTime = Time.time;
    }

    private void ShakeCamera()
    {
        if (isShaking)
        {
            this.transform.position += new Vector3(Random.Range(-shakePower, shakePower), Random.Range(-shakePower, shakePower), 0);

            if (Time.time - shakeStartTime > shakeDuration)
            {
                isShaking = false;
            }
        }
    }
}