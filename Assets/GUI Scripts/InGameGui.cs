using UnityEngine;
using System.Collections;

public class InGameGui : MonoBehaviour {
	private bool cameraMode = true; //camera starts in first person mode
	private bool paused = false;
	
	public Camera charCam;
	public Camera mapCam;
	
	void Start(){
		Screen.showCursor = false;
	}

	void Update(){
		if (Input.GetButtonUp("Camera")){
			if (cameraMode){
				charCam.enabled = false;
				mapCam.enabled = true;
				cameraMode = false;
			}else{
				charCam.enabled = true;
				mapCam.enabled = false;
				cameraMode = true;
			}
		}
		if (Input.GetButtonUp("Menu")){
			paused = !paused;
			if (paused)
				//this stops game time
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}
	}
	
	void OnGUI(){
		if (paused){
			bool done = Settings_GUI.settingsMenu();
			if (done){
				paused = false;
				Time.timeScale = 1;
			}
		}
	}
}
