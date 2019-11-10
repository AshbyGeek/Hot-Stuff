using UnityEngine;
using Commands;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameMenu
{
    Main,
    Settings
}

public class Loader_GUI : MonoBehaviour {
	
	public static GameMenu CurrentGameMenu = GameMenu.Main;

    public ICommand QuitCommand = new ActionCommand(() => Application.Quit());

    public ICommand LoadGame = new ActionCommand(() => SceneManager.LoadScene("Game"));

    public ICommand ChangeToSettingsMenu = new ActionCommand(() => CurrentGameMenu = GameMenu.Settings);

    public ICommand ChangeToMainMenu = new ActionCommand(() => CurrentGameMenu = GameMenu.Main);
	
	void OnGUI () {
		Cursor.visible = true;
		if (CurrentGameMenu == GameMenu.Main){
			// Make a background box
			int width = 100;
			int height = 120;
			int left = (Screen.width - width)/2;
			int top = (Screen.height - height)/2;
			
			GUI.BeginGroup(new Rect(left,top,width,height));
			GUI.Box (new Rect (0,0,100,120), "Loader Menu");
		
			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			if (GUI.Button (new Rect (10,30,80,20), "Load Level")) {
                LoadGame.Execute();
			}
			
			if (GUI.Button(new Rect(10,60,80,20), "Settings")){
                ChangeToSettingsMenu.Execute();
			}
		
			// Make the second button.
			if (GUI.Button(new Rect (10,90,80,20), "exit")) {
                QuitCommand.Execute();
			}
			
			GUI.EndGroup();
		}else if (CurrentGameMenu == GameMenu.Settings){
			bool done = Settings_GUI.settingsMenu();
            if (done)
                ChangeToMainMenu.Execute();
		}
	}
}
