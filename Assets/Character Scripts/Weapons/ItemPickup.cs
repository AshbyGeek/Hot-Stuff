using UnityEngine;
using System.Collections;

//this needs some serious work
public class ItemPickup : MonoBehaviour {
	public GUIContent GUI_Image;
	public ItemController control;
	public GameObject parent;
	
	public void Start(){
		parent = transform.parent.gameObject;
	}
}
