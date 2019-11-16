using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public abstract class Item : MonoBehaviour {
	protected Vector3 defPos;
	public Texture2D icon_selected;
	public Texture2D icon_unselected;
	public string identifier;
	public bool inInventory;
	public bool oneTimeUse;
	public bool reusable;
	
	public void Activate(){
		enableColliders(true);
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		gameObject.SetActive(true);
	}
	
	public void Deactivate(){
		gameObject.SetActive(false);
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		enableColliders(false);
        transform.localPosition = defPos;
        transform.localRotation = Quaternion.identity;
    }

    public virtual void UseItem(){
		//Let this object wander about the map again
		Activate();
	}
	
	private void enableColliders(bool enabled){
		GetComponent<Collider>().enabled = enabled;
		foreach (Collider curCollider in this.GetComponentsInChildren<Collider>()){
			curCollider.enabled = enabled;
		}
	}
}
