using UnityEngine;
/** Author: Taylor Ereio
 * File: CollisionDectection.cs
 * Date: 2/3/2015
 * Description: Detects if bricks are hit by ball within the wall
 * */
using System.Collections;

public class FlashLightController : MonoBehaviour {

	private Item item;

	private Light flashLight;
	private float power = 100f;
	private bool active;
	private float powerDecr;

	// Use this for initialization
	void Start () {
		flashLight = GetComponentInChildren<Light> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(active){
			if(powerDecr <= Time.time){
				power -= 1f;
				powerDecr = Time.time + 15f;
			} 

		}
	}

	public void SetToolEnabled(bool status){
		if(flashLight != null)
			flashLight.enabled = active = status;
	}

	public void AddPower(int increase){
		power += increase;
	}
}
