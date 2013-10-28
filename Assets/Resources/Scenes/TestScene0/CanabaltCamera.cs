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
    private float zoomLerpValue = 0.0f;

    private bool isShaking = false;
    private float shakeDuration = 1.0f;
    private float shakePower = 1.0f;
    private float shakeStartTime = 1.0f;

    // Special Upside Down Camera Lerping Case
    [Range(0.001f, 5.0f)]
    public float flipSpeed = 0.001f;
    private float flipLerpValue = 0.0f;
    private bool isFlipping = false;    

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
                //if (player.GetComponent<Player_Movement_Script>().isInAir && player.transform.position.y > 0.0f)
                //{
                    tempVector.y = player.transform.position.y;
                    if (!isFlipping)
                    {
                        tempVector.y += moveOffsetY;
                    }
	            //}
			}
			else
			{
                tempVector.x = player.transform.position.x;
				tempVector.y = player.transform.position.y;			
			}

            this.transform.position = tempVector;

            if (isFlipping)
            {
                LerpToNegativeOffset();
            }
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
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Mathf.Lerp(currentZPosition, endZoomPoint, Mathf.Clamp(zoomLerpValue, 0.0f, 1.0f)));
            zoomLerpValue += zoomSpeed * Time.deltaTime;

            if (zoomLerpValue > 1.0f)
            {
                zoomLerpValue = 0.0f;
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

    public void BeginFlipLerp()
    {
        isFlipping = true;
    }

    public void LerpToNegativeOffset()
    {
        if (isFlipping)
        {
            this.transform.position = Vector3.Lerp(
                new Vector3(this.transform.position.x, this.transform.position.y + moveOffsetY, this.transform.position.z), 
                new Vector3(this.transform.position.x, this.transform.position.y - moveOffsetY, this.transform.position.z), 
                Mathf.Clamp(flipLerpValue, 0, 1)
            );
            
            flipLerpValue += flipSpeed * Time.deltaTime;

            if (flipLerpValue >= 1)
            {
                isFlipping = false;
                moveOffsetY = -moveOffsetY;
                flipLerpValue = 0.0f;
                print(moveOffsetY);
            }
        }
    }
}