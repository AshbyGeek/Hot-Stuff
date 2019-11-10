using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	protected Vector3 defPos;
	public Texture2D icon_selected;
	public Texture2D icon_unselected;
	public string identifier;
	public bool inInventory;
	public bool oneTimeUse;
	public bool reusable;
	
	protected Inventory inv;
	
	public void addToInventory(Inventory inv){
		//since its going into an inventory, don't let it move
		//  and move it to its position on the character
		inInventory = true;
		deactivate();
		transform.parent = inv.transform;
		transform.localPosition = defPos;
		transform.localRotation = Quaternion.identity;
		this.inv = inv;
	}
	
	public void activate(){
		gameObject.SetActive(true);
	}
	
	public void deactivate(){
		gameObject.SetActive(false);
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		enableColliders(false);
	}
	
	public virtual void useItem(){
		//Let this object wander about the map again
		enableColliders(true);
		transform.parent = null;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		inInventory = false;
	}
	
	private void enableColliders(bool enabled){
		GetComponent<Collider>().enabled = enabled;
		foreach (Collider curCollider in this.GetComponentsInChildren<Collider>()){
			curCollider.enabled = enabled;
		}
	}
}
