using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {
	
	private ArrayList inRange = new ArrayList();
	
	private ArrayList inventory = new ArrayList();
	
	public void OnTriggerEnter(Collider other){
		inRange.Add(other);
	}
	
	public void OnTriggerExit(Collider other){
		inRange.Remove(other);	
	}
	
	public void Update(){
		if (Input.GetButton("Fire2")){
			float dist = 9999.0f;
			GameObject closeObj = null;
			foreach(Collider collide in inRange){
				float tmp = Vector3.Distance(collide.transform.position,gameObject.transform.position);
				
				if (tmp < dist){
					dist = tmp;
					closeObj = collide.transform.parent.gameObject;
				}
			}
			if (closeObj != null){
				addObject(closeObj);
			}
		}
	}
	
	//this needs some serious work
	public void addObject(GameObject obj){
		obj.transform.parent = gameObject.transform;
		obj.transform.position = new Vector3(1.0f,0.6f,0.07f);
		obj.active = false;
		ItemPickup tmp = obj.transform.FindChild("ItemPickup").GetComponent<ItemPickup>();
		tmp.control = this;
		
		inventory.Add(obj);
	}
	
	public void useObject(GameObject obj){
		inventory.Remove(obj);
		
		GameObject tmp = (GameObject) inventory[0];
		tmp.active = true;
	}
		
	
}
