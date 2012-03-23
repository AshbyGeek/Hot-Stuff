using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {	
	//how close does an object need to be before it can be grabbed?
	public float grabRange = 10.0f;
	
	//this class and the following field are used by the GUI
	//  to keep track of how many of what kinds of items are
	//  in inventory
	private class ItemsInfo{
		public string identifier;
		public int count;
		public Texture2D icon_selected;
		public Texture2D icon_unselected;
		
		public ItemsInfo(string identifier, int num, Texture2D icon_selected, Texture2D icon_unselected){
			this.identifier = identifier;
			this.count = num;
			this.icon_selected = icon_selected;
			this.icon_unselected = icon_unselected;
		}
	}
	private class ItemsList: List<ItemsInfo>{
		public int current;
		
		public bool contains(string identifier){
			foreach(ItemsInfo cur in this){
				if (cur.identifier == identifier){
					return true;
				}
			}
			return false;
		}
		
		public void addItem(Item item){
			foreach(ItemsInfo cur in this){
				if (cur.identifier == item.identifier){
					cur.count += 1;
					return;
				}
			}
			this.Add(new ItemsInfo(item.identifier,1,item.icon_selected,item.icon_unselected));
		}
		
		//returns true only if there are no items of that
		//  type left
		public bool removeItem(Item item){
			foreach(ItemsInfo cur in this){
				if (cur.identifier == item.identifier){
					cur.count -= 1;
					if (cur.count <= 0){
						this.Remove(cur);
						this.current = -1;
						return true;
					}else{
						return false;
					}
				}
			}
			return false;
		}
	}
	
	private ItemsList items = new ItemsList();
	
	//this is the item that the character is currently using, or null if none
	private Item current = null;
	
	public void Start(){
		//add any items currently held by this character to the Inventory
		Item[] list = GetComponentsInChildren<Item>(true);
		foreach (Item curItem in list){
			curItem.addToInventory(this);
			items.addItem(curItem);
		}
		current = null;
		items.current = -1;
	}
	
	//display the objects on hand
	public void OnGUI(){
		if (items.Count > 0){
			GUILayout.BeginArea(new Rect(10,80,60,Screen.height - 80 - 20));
			GUILayout.BeginVertical(GUI.skin.box);
			
			foreach(ItemsInfo curItemsInfo in items){
				//choose the icon to display
				GUIContent tmp;
				if (current != null && curItemsInfo.identifier == current.identifier){
					tmp = new GUIContent(curItemsInfo.count.ToString(),curItemsInfo.icon_selected);
				}else{
					tmp = new GUIContent(curItemsInfo.count.ToString(),curItemsInfo.icon_unselected);
				}
				
				//display it
				GUILayout.Label(tmp);
			}
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
	
	public void Update(){
		if (Input.GetButton("Fire2")){
			Collider[] inRange = Physics.OverlapSphere(transform.position,grabRange,1<<LayerMask.NameToLayer("Items"));
			//find the nearest item
			float dist = 9999.0f;
			Item nearObject = null;
			foreach(Collider collide in inRange){
				float tmp = Vector3.Distance(collide.transform.position,transform.position);
				Item curItem = collide.GetComponent<Item>();
				if (tmp < dist && curItem != null && !curItem.reusable){
					dist = tmp;
					nearObject = curItem;
				}
			}
			
			//add it to the inventory
			if (nearObject != null){
				nearObject.addToInventory(this);
				items.addItem(nearObject);
			}
		}
		
		if (Input.GetAxis("Mouse ScrollWheel") < 0){
			selectNextItem();
		}else if (Input.GetAxis("Mouse ScrollWheel") > 0){
			selectPrevItem();
		}
		
		int numHotKeys = items.Count;
		numHotKeys = Mathf.Clamp(numHotKeys,0,9);
		for (int i = 0; i < numHotKeys; i++){
			if (Input.GetKeyUp((i+1).ToString())){
				setAsCurrent(items[i].identifier);
			}
		}
	}
	
	public void removeCurItem(){
		if (items.removeItem(current))
			current = null;
		else
			setAsCurrent(current.identifier);
	}
	
	//returns true if an instance of the desired item type
	//  was set as the current item
	private bool setAsCurrent(string identifier){
		if (identifier == ""){		//do this if we are setting it to no item at all
			if (current != null)
				current.deactivate();
			current = null;
			return true;
		}else{
			Item[] list = GetComponentsInChildren<Item>(true);
			foreach( Item curItem in list){
				if (curItem.identifier == identifier){
					current = curItem;
					current.activate();
					return true;
				}
			}
		}
			return false;
	}
	
	private void selectNextItem(){
		items.current += 1;
		if (items.current >= items.Count){
			items.current = -1;
			setAsCurrent("");
			return;
		}
		setAsCurrent(items[items.current].identifier);
	}
	
	private void selectPrevItem(){
		items.current -= 1;
		if (items.current < -1)
			items.current = items.Count - 1;
		if (items.current == -1){
			setAsCurrent("");
			return;
		}
		setAsCurrent(items[items.current].identifier);
	}
}
