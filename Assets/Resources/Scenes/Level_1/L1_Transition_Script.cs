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
		Application.LoadLevel("Level_2_TDS");
		//Application.LoadLevel("Level_4_Boss");
	}
	public void onStartCloud()
	{
		StartCoroutine("CloudSpawn");
	}
	IEnumerator CloudSpawn()
	{
		Instantiate(Resources.Load("Prefabs/Level_1/Cloud"), fakeShip.transform.position-new Vector3(0,-40,-10), Quaternion.AngleAxis(270, new Vector3(1,0,0)));
		yield return new WaitForSeconds(1f);
		Instantiate(Resources.Load("Prefabs/Level_1/Cloud"), fakeShip.transform.position-new Vector3(0,-40,1), Quaternion.AngleAxis(270, new Vector3(1,0,0)));
		yield return new WaitForSeconds(.5f);
		Instantiate(Resources.Load("Prefabs/Level_1/Cloud"), fakeShip.transform.position-new Vector3(0,-40,10), Quaternion.AngleAxis(270, new Vector3(1,0,0)));
		yield return new WaitForSeconds(.5f);
		Instantiate(Resources.Load("Prefabs/Level_1/Cloud"), fakeShip.transform.position-new Vector3(0,-40,1), Quaternion.AngleAxis(270, new Vector3(1,0,0)));
		yield return new WaitForSeconds(.5f);
		Instantiate(Resources.Load("Prefabs/Level_1/Cloud"), fakeShip.transform.position-new Vector3(0,-40,4), Quaternion.AngleAxis(270, new Vector3(1,0,0)));
		yield return new WaitForSeconds(.25f);
		Instantiate(Resources.Load("Prefabs/Level_1/Cloud"), fakeShip.transform.position-new Vector3(0,-40,15), Quaternion.AngleAxis(270, new Vector3(1,0,0)));
		yield return new WaitForSeconds(.25f);
		Instantiate(Resources.Load("Prefabs/Level_1/Cloud"), fakeShip.transform.position-new Vector3(0,-40,4), Quaternion.AngleAxis(270, new Vector3(1,0,0)));
		yield return new WaitForSeconds(.25f);
		Instantiate(Resources.Load("Prefabs/Level_1/Cloud"), fakeShip.transform.position-new Vector3(0,-40,15), Quaternion.AngleAxis(270, new Vector3(1,0,0)));
		yield return new WaitForSeconds(.25f);
		Instantiate(Resources.Load("Prefabs/Level_1/Cloud"), fakeShip.transform.position-new Vector3(0,-40,10), Quaternion.AngleAxis(270, new Vector3(1,0,0)));
		
		
		
		
		yield break;
	}
	
}
