using UnityEngine;
using System.Collections;

public class MakeMiniMapBig : MonoBehaviour {
	
	Rect smallSize;
	Rect largeSize;
	// Use this for initialization
	void Start () {
		smallSize = new Rect(0,0,0.3f,0.3f);
		largeSize = new Rect(0.05f,0.05f,0.9f,0.9f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.F1)){
			if (GetComponent<Camera>().rect == smallSize){
				GetComponent<Camera>().rect = largeSize;
			}else{
				GetComponent<Camera>().rect = smallSize;
			}
		}
	}
}
