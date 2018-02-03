using UnityEngine;
/** Author: Taylor Ereio
 * File: CollisionDectection.cs
 * Date: 2/3/2015
 * Description: Detects if bricks are hit by ball within the wall
 * */
using System.Collections;

public class TelescopeController : MonoBehaviour {

	private Item item;

	private float lensDistance = 50f;
	private Camera zoomCam;
	private bool active;

	// Use this for initialization
	void Start () {
		zoomCam = GetComponentInChildren<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(active){
			// Show zoomed view
		}
	}

	public void SetToolEnabled(bool status){
			active = status;
	}

	public void AddPower(int increase){
		lensDistance += increase;
	}
}
