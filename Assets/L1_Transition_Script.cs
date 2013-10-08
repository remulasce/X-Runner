using UnityEngine;
using System.Collections;

public class L1_Transition_Script : MonoBehaviour {
	
	
	public GameObject fakePlayer;
	public GameObject fakeShip;
	public CanabaltCamera mainCamera;
	
	public void onStart()
	{
		print ("l2trans onstart");
		//Take the actual player from the camera- it knows.
		GameObject player = mainCamera.player;
		
		mainCamera.player = fakePlayer;
		
		player.SetActive(false);
	}
	
	public void onEnterShip()
	{
		mainCamera.player = fakeShip;
	}
	
	public void onFinishTransition()
	{
		Application.LoadLevel("Level_2_TDS");
	}
	
}
