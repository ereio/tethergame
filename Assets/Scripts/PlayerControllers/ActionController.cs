/** Author: Taylor Ereio
 * File: CollisionDectection.cs
 * Date: 2/3/2015
 * Description: class that organizes the actions of the player, interacting with
 * the inventory via the inventory class. Helper functions are given here.
 * */

using UnityEngine;
using System.Collections;

public class ActionController : MonoBehaviour {

	public Texture2D crosshairImage;
	public AudioClip itemPickupAudio;
	private Inventory inventory;
			
	public Transform hand;
	private Camera cam;

	public Item equipItem;
	private bool equipActive;
	private bool equipTrigger;

	private GameObject current;
	private bool equip;

	private float grabLength = 10f;
	private int Layer = 1;

	// Use this for initialization
	void Start () {
		inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
		hand = GameObject.FindGameObjectWithTag ("Hand").GetComponent<Transform>();
		cam = Camera.main;
		equip = false;
		equipItem = new Item ();
	}
	
	// Update is called once per frame
	// either shows the cursor if the inventory is open, or hides it
	// if closed. Also locks it to preference the use of the crosshair
	void Update () {
		if(inventory.show){
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		} else {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		// Checks to see if player is trying to pick up an item
		if(Input.GetMouseButtonDown(0))
			CheckActionOnItem();

		// Actually sets the tool enabled throug the ToolController Scripts
		if(equipItem.itemType == Item.ItemType.Tool){
			if(Input.GetButtonDown("Fire2")){
				equipActive = !equipActive;
				if(equipItem.itemID == ItemDatabase.FLASHLIGHT)
					current.GetComponent<FlashLightController>().SetToolEnabled(equipActive);
				if(equipItem.itemID == ItemDatabase.TELESCOPE)
					current.GetComponent<TelescopeController>().SetToolEnabled(equipActive);
			}
		}

		// would allow for attacking
		if(equipItem.itemType == Item.ItemType.Weapon){
			if(Input.GetButtonDown("Fire2")){
				// trigger here
			}
		}
	}

	void CheckActionOnItem(){
		RaycastHit hitInfo;
		Transform cam = Camera.main.transform;
		Ray screenRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		
		// Debug
		//Debug.DrawRay(screenRay.origin, screenRay.direction * grabLength, Color.cyan, 0.2f);
		
		// Sends the raycast out to know when to grab items
		// doesn't actually grab items obviously
		if (Physics.Raycast 
		    (screenRay, out hitInfo, grabLength, (Layer << LayerMask.NameToLayer("Item")))) {
			inventory.AddItem(hitInfo.transform.gameObject.GetComponent<ItemDetails>().itemID);
			hitInfo.transform.gameObject.SetActive(false);
		}
	}

	// draws the cross hair to half the screen size
	void OnGUI(){
		float x = (Screen.width / 2) - (crosshairImage.width / 4);
		float y = (Screen.height / 2) - (crosshairImage.height / 4);
		GUI.DrawTexture (new Rect (x, y, crosshairImage.width, crosshairImage.height), crosshairImage);
	}

	// Handles equipping the item by instantiating the required item in the
	// "Hand" of the player. Any item instantiated here will appear as it's
	// prefabs location - it is kinematic, as it will appear as the player is
	// holding it
	public void SetEquip(Item item){
		if(!equip){
			equip = true;
			equipItem = item;
			current = Instantiate(item.itemObject.gameObject, hand.transform.position, hand.transform.rotation) as GameObject;
	
			current.transform.parent = hand.transform;
			current.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			handle_placement(equipItem);

		}

	}

	private void handle_placement(Item item){
		if(ItemDatabase.TELESCOPE == item.itemID){
			current.transform.localPosition += new Vector3(0f,0f,0.5f);
		} else if(item.itemType == Item.ItemType.Key){
			current.transform.localPosition += new Vector3(0f,0f,1f);
		} else {
			hand.transform.localPosition = new Vector3(0.8f, -0.5f, 0.55f);
		}

	}
	// Unequips the item by destroying whatever prefab is instantiated
	// allows for items to be swapped
	public void UnsetEquip(Item item){
		if(equip && current != null){	// if something is equiped
			if(item != null){			// and the item being passed is not null
				print("swap with " + item.itemName);
			} else {
				Destroy(current);		// otherwise, destroy the equiped and let
				equipItem = new Item();	// inv know hand is empty
				equipActive = false;
			}
			equip = false;
		}

	}
}
