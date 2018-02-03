/** Author: Taylor Ereio
 * File: CollisionDectection.cs
 * Date: 2/3/2015
 * Description: Manages the inventory of the player. Allows for items to be picked up,
 * added, removed, and generally composed within the inventory. 
 * Created using a tutorial at 
//https://www.youtube.com/watch?v=D5WEwG4L5HQ
 * */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Inventory : MonoBehaviour {

	public int slotX = 1;	// sets the inventory 
	public int slotY = 9;	// slot trajectory
	public GUISkin invSkin;
	public List<Item> inventory = new List<Item>();	// inventory positions
	public List<Item> slots = new List<Item>();		// ui and drag position helpers
	static private ItemDatabase database;			// a hold on item database
	static private ActionController controller;

	public bool show;								// show or hide inventory 
	private bool showToolTip;						// 
	private string tooltip;							// tool Tip content
	private int toolTipCount;
	private bool draggingItem;						// if item is being dragged
	private Item draggedItem;						// item being dragged
	private int prevIndex;						    // previous index of dragged item



	// Use this for initialization
	void Start () {
		// Gets a hold on the Item database
		database = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
		controller = GetComponent<ActionController>();

		// initalizes the slots to zero
		for(int i = 0; i < (slotX * slotY); i++){
			slots.Add(new Item());
			inventory.Add(new Item());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Inventory")){
			show = !show;
		}

	}
	


	void OnGUI(){
		GUI.skin = invSkin;
		tooltip = "";
		// Prints the GUI Skin for the inventory
		if (show) {
			// draws the actual inventory and items
			DrawInventory();
			if(showToolTip){
				Rect ttBox = new Rect(Event.current.mousePosition.x + 15f, 
				                      Event.current.mousePosition.y, 200 * toolTipCount, 200);
				// Set's up the tool tip box if needed
				GUI.Box(ttBox, tooltip, invSkin.GetStyle("ToolTip"));
			}
		}
		// if an item is being dragged it will draw it following the mouse
		if(draggingItem){
			Rect diBox = new Rect(Event.current.mousePosition.x - 25f, 
			                      Event.current.mousePosition.y - 25f, 50, 50);
			GUI.DrawTexture(diBox, draggedItem.itemIcon);
		}

	}

	void DrawInventory(){

		int i = 0;
		Event e = Event.current;

		for(int x = 0; x < slotX; x++){
			for(int y = 0; y < slotY; y++){
				Rect slotRect = new Rect(x * 50, y * 85, 75 ,75);
				GUI.Box (slotRect, "", invSkin.GetStyle("SlotBackground"));
				slots[i] = inventory[i];
			
				// if the slot isn't null it will be checking to see if there's
				// any interaction with the item
				if(slots[i].itemName != null){
					// if the slot type is of Key, it needs to draw the picture a 
					// bit differently to render well
					if(slots[i].itemType == Item.ItemType.Key)
						GUI.DrawTexture(new Rect(x * 50, y * 85+20, 75 ,35), slots[i].itemIcon);
					else
						GUI.DrawTexture(slotRect, slots[i].itemIcon);

					// if the slot contains the mouse, it will draw a tooltip for the item
					if(slotRect.Contains(e.mousePosition)){
						CreateToolTip(slots[i]);
						showToolTip = true;
						if(e.button == 0 && e.type == EventType.MouseDrag && !draggingItem){
							showToolTip = false;
							draggingItem = true;
							prevIndex = i;
							draggedItem = slots[i];
							inventory[i] = new Item();
						}
						// if it's dragging, it will delete that item so it's moving
						if(e.type == EventType.MouseUp && draggingItem){
							inventory[prevIndex] = inventory[i];
							inventory[i] = draggedItem;
							draggingItem = false;
							draggedItem = new Item();
						}
						// This is what equips and inspects items
						// used to equip the flashlight
						if(e.type == EventType.MouseUp && e.button == 0 && !draggingItem){
 							if(slots[i].itemType == Item.ItemType.Tool){
								EquipItem(slots[i]);

							}

							if(slots[i].itemType == Item.ItemType.Key){
								EquipItem(slots[i]);
							}

							if(slots[i].itemType == Item.ItemType.Note){
								print ("Read Note");
							}
						}
					}
				} else {
					if(slotRect.Contains(e.mousePosition)){
						if(e.type == EventType.MouseUp && draggingItem){
							inventory[i] = draggedItem;
							draggingItem = false;
							draggedItem = new Item();
						}
					}
				}
				if(tooltip == ""){
					showToolTip = false;
				}
				i++;
			}
		}
	}

	// creates a tool tip based on item type
	// and properties being passed in
	string CreateToolTip(Item item){
		tooltip = item.itemName + "\n" 
				+ item.itemDesc + "\n";
		if(item.itemPower != 0){
			tooltip += "Power: " + item.itemPower;
		} else if(item.itemID == 0){
			tooltip += "Batteries are dead";
		}

		toolTipCount = 1;
		return tooltip;
	}

	// equips an item based on if it's equipable
	private bool EquipItem(Item item){ 

		// if it's currently equipped, unequip it
		if(controller.equipItem.itemName == item.itemName){
			controller.UnsetEquip(null);

			// otherwise, equip the item if it's id matchs a tool
		} else {
			switch(item.itemID){
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
				controller.SetEquip(item);
				return true;
				break;
			}
		}
		return false;
	}

	// adds an item to the inventory
	// navigates rows to find an empty position
	public void AddItem(int id){
		for(int i = 0; i < inventory.Count; i++){
			if(inventory[i].itemName == null){
				for(int j = 0; j < database.items.Count; j++){
					if(database.items[j].itemID == id){
						inventory[i] = database.items[j];
					}
				}
				break;
			}
		}
	}

	// removes an item from the inventory
	public void RemoveItem(int id){
		for(int i = 0; i < inventory.Count; i++){
			if(inventory[i].itemID == id){
				inventory[i] = new Item();
				break;
			}
		}

	}
	// inventory check to see if the item within the inventory
	// exists, to allow checking of keys for door opening, etc.
	public bool InventoryCheck(int id){
		for(int i = 0; i < inventory.Count; i++){
			if(inventory[i].itemID == id)
				return true;
		}

		return false;
	}

}
