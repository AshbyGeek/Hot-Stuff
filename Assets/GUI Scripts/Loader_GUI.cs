using UnityEngine;
using System.Collections;

public class Loader_GUI : MonoBehaviour {
	private enum Menu{
		Main,
		Settings
	}
	
	private Menu curMenu = Menu.Main;
	
	void OnGUI () {
		Cursor.visible = true;
		if (curMenu == Loader_GUI.Menu.Main){
			// Make a background box
			int width = 100;
			int height = 120;
			int left = (Screen.width - width)/2;
			int top = (Screen.height - height)/2;
			
			GUI.BeginGroup(new Rect(left,top,width,height));
			GUI.Box (new Rect (0,0,100,120), "Loader Menu");
		
			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			if (GUI.Button (new Rect (10,30,80,20), "Load Level")) {
				Application.LoadLevel (1);
			}
			
			if (GUI.Button(new Rect(10,60,80,20), "Settings")){
				curMenu = Loader_GUI.Menu.Settings;
			}
		
			// Make the second button.
			if (GUI.Button(new Rect (10,90,80,20), "exit")) {
				Application.Quit();
			}
			
			GUI.EndGroup();
		}else if (curMenu == Menu.Settings){
			bool done = Settings_GUI.settingsMenu();
			if (done)
				curMenu = Loader_GUI.Menu.Main;
		}
	}
}
