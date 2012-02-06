using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	protected Vector3 defPos;
	public Texture2D icon_selected;
	public Texture2D icon_unselected;
	public string identifier;
	public bool inInventory;
	
	public void addToInventory(Inventory inv){
		//since its going into an inventory, don't let it move
		//  and move it to its position on the character
		inInventory = true;
		this.gameObject.active = false;
		this.GetComponent<Collider>().enabled = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		transform.parent = inv.transform;
		transform.localPosition = defPos;
		transform.localRotation = Quaternion.identity;
	}
	
	public void activate(){
		this.gameObject.active = true;
	}
	
	public void deactivate(){
		this.gameObject.active = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		this.GetComponent<Collider>().enabled = false;
	}
	
	public virtual void useItem(){
		//Let this object wander about the map again
		this.GetComponent<Collider>().enabled = true;
		transform.parent = null;
		rigidbody.constraints = RigidbodyConstraints.None;
		inInventory = false;
	}
}
