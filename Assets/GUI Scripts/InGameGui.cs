using UnityEngine;
using System.Collections;

public class InGameGui : MonoBehaviour {
	private bool CameraMode;
	
	public GameObject charCam;
	public GameObject mapCam;
	
	void Start(){
		CameraMode = true; //camera is first person
	}
	
	void Update(){
		if (Input.GetButtonUp("Camera")){
			if (CameraMode){
				charCam.active = false;
				mapCam.active = true;
				CameraMode = false;
			}else{
				charCam.active = true;
				mapCam.active = false;
				CameraMode = true;
			}
		}
	}
}
