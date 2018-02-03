/** Author: Taylor Ereio
 * File: CollisionDectection.cs
 * Date: 2/3/2015
 * Description: Manages the current state of the player character
//https://www.youtube.com/watch?v=D5WEwG4L5HQ
 * */
using UnityEngine;
using System.Collections;

// Used in UNITY TUTORIALS ON STEALTH AT 
//http://unity3d.com/learn/tutorials/projects/stealth/player-health

public class PlayerStatus : MonoBehaviour
	{
		public float health = 100f;                         // How much health the player has left.
		public float resetAfterDeathTime = 5f;              // How much time from the player dying to the level reseting.
		public AudioClip deathClip;                         // The sound effect of the player dying.
		
		
		private Animator anim;                              // Reference to the animator component.
		private RigidbodyFirstPersonController movementController;              // Reference to the player movement script.
		private float timer;                                // A timer for counting to the reset of the level once the player is dead.
		private bool playerDead;                            // A bool to show if the player is dead or not.
		
		
		void Awake ()
		{
			// Setting up the references.
			anim = GetComponent<Animator>();
			movementController = GetComponent<RigidbodyFirstPersonController>();
		}
		
		
		void Update ()
		{
			// If health is less than or equal to 0...
			if(health <= 0f)
			{
				// ... and if the player is not yet dead...
				if(!playerDead)
					// ... call the PlayerDying function.
					PlayerDying();
				else
				{
					// Otherwise, if the player is dead, call the PlayerDead and LevelReset functions.
					PlayerDead();
					LevelReset();
				}
			}
		}
		
		
		void PlayerDying ()
		{
			// The player is now dead.
			playerDead = true;
			
			// Set the animator's dead parameter to true also.
			//anim.SetBool(hash.deadBool, playerDead);
			
			// Play the dying sound effect at the player's location.
			AudioSource.PlayClipAtPoint(deathClip, transform.position);
		}
		
		
		void PlayerDead ()
		{
			/* If the player is in the dying state then reset the dead parameter.
			if(anim.GetCurrentAnimatorStateInfo(0).nameHash == hash.dyingState)
				anim.SetBool(hash.deadBool, false);
			*/
			// Disable the movement.
			movementController.isEnabled = false;
		}
		
		
		void LevelReset ()
		{
			// Increment the timer.
			timer += Time.deltaTime;
			
			//If the timer is greater than or equal to the time before the level resets...
			if(timer >= resetAfterDeathTime)
				Application.LoadLevel("IntroGUI");
		}
		
		
		public void TakeDamage (float amount)
		{
			// Decrement the player's health by amount.
			health -= amount;
		}

}
