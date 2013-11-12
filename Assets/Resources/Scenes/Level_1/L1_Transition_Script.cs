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

        fakePlayer.renderer.enabled = true;        
		
		mainCamera.player = fakePlayer;
		
		player.SetActive(false);
	}
	
	public void onEnterShip()
	{
		mainCamera.player = fakeShip;
        fakePlayer.renderer.enabled = false;
	}
	
	public void onFinishTransition()
	{
		//Application.LoadLevel("Level_2_TDS");
		Application.LoadLevel("Level_4_Boss");
	}
	
}
