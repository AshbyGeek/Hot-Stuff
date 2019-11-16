using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
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
			Camera camera = GetComponent<Camera>();
			if (camera.rect == smallSize){
				camera.rect = largeSize;
			}else{
				camera.rect = smallSize;
			}
		}
	}
}
