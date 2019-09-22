using UnityEngine;
using System.Collections;

public class InGameGui : MonoBehaviour {
	private bool cameraMode = true; //camera starts in first person mode
	private bool paused = false;
	
	void Start(){
		Cursor.visible = false;
	}

	void Update(){
		if (Input.GetButtonUp("Menu") && Time.timeScale > 0){
			paused = !paused;
			if (paused){
				//this stops game time
				Time.timeScale = 0;
				Cursor.visible = true;
			}else{
				Time.timeScale = 1;
				Cursor.visible = false;
			}
		}
	}
	
	void OnGUI(){
		if (paused){
			bool done = Settings_GUI.settingsMenu();
			if (done){
				paused = false;
				Time.timeScale = 1;
				Cursor.visible = false;
			}
		}
	}
}
