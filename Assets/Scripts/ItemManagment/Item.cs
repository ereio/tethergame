/** Author: Taylor Ereio
 * File: CollisionDectection.cs
 * Date: 2/3/2015
 * Description: class that contains the item's stored within the database and the
 * inventory of the player
 * Created using a tutorial at 
 * https://www.youtube.com/watch?v=D5WEwG4L5HQ
 * */
using UnityEngine;
using System.Collections;

// 
[System.Serializable]
public class Item {
	public string itemName;				// name displayed in tooltip
	public int itemID;					// as is other information
	public string itemDesc;
	public Texture2D itemIcon;
	public GameObject itemObject;		// holds the game object for rendering
	public int itemPower;				// power for flashlight and weapons
	public int itemSpeed;
	public ItemType itemType;

	public enum ItemType{
		Weapon,
		Tool,
		Consumable,
		Note,
		Key,
		Book
	}

	public Item(){

	}
	public Item(string name, int id, string desc, Texture2D icon, ItemType type){
		itemName = name;
		itemID = id;
		itemDesc = desc;
		itemIcon = icon;
		itemType = type;
	}

}
