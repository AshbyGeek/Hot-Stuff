using UnityEngine;
using System.Collections;

public class Settings_GUI : MonoBehaviour {
	private enum ExtendedMenu{
		None,
		Res,
		Quality
	};
	
	private static ExtendedMenu curMenu = ExtendedMenu.None;
	private static int curResIndx = 0;
	private static Vector2 scrollPosition;		
	private static bool fullscreen;
	
	public static bool settingsMenu() {
			int width = 100;
			int height = 170;
			int left;
			if (curMenu != ExtendedMenu.None)
				left = (Screen.width - width*2 - 10)/2;
			else
				left = (Screen.width - width)/2;
			
			int top = (Screen.height - height)/2;
			
			
			GUILayout.BeginArea(new Rect(left,top,width,height),GUI.skin.GetStyle("box"));		
				GUILayout.Box("Settings");
				GUILayout.Space(10);
			
				if (GUILayout.Button("Quality")){
					if (curMenu == Settings_GUI.ExtendedMenu.Quality)
						curMenu = Settings_GUI.ExtendedMenu.None;
					else
						curMenu = Settings_GUI.ExtendedMenu.Quality;
				}
			
				if (GUILayout.Button("Resolution")){
					if (curMenu == ExtendedMenu.Res)
						curMenu = ExtendedMenu.None;
					else
						curMenu = ExtendedMenu.Res;
				}
				fullscreen = Screen.fullScreen;
				fullscreen = GUILayout.Toggle(fullscreen,"Fullscreen");
				if (fullscreen != Screen.fullScreen)
					Screen.fullScreen = fullscreen;
			
			
				GUILayout.Space(10);
				if (GUILayout.Button("Back")) {
					return true;
				}
			
			GUILayout.EndArea();
		
		if (curMenu == ExtendedMenu.Res){
			GUILayout.BeginArea(new	Rect(left + width + 10,top+20,width,height-20));
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
				
				Resolution[] resolutions = Screen.resolutions;
				GUIContent[] list = new GUIContent[resolutions.Length];
				for(int i = 0; i < resolutions.Length; i++){
					if (resolutions[i].Equals(Screen.currentResolution))
						curResIndx = i;
					list[i] = new GUIContent(resolutions[i].width.ToString() + "x" +
				                         resolutions[i].height.ToString());
				}
				int prev = curResIndx;
				curResIndx = GUILayout.SelectionGrid(curResIndx,list,1);
				if (curResIndx != prev){
					curMenu = ExtendedMenu.None;
					Resolution res = resolutions[curResIndx];
					Screen.SetResolution(res.width,res.height,fullscreen);
				}
			
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
		
		if (curMenu == Settings_GUI.ExtendedMenu.Quality){
			GUILayout.BeginArea(new	Rect(left + width + 10,top+20,width,height-20));
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			
				GUIContent[] list = {
					new GUIContent("Fastest"),
					new GUIContent("Fast"),
					new GUIContent("Simple"),
					new GUIContent("Good"),
					new GUIContent("Beautiful"),
					new GUIContent("Fantastic")};
				int curQuality = (int) QualitySettings.currentLevel;
				int tmp = GUILayout.SelectionGrid(curQuality,list,1);
				
				if (tmp != curQuality)
					QualitySettings.currentLevel = (QualityLevel) tmp;
			
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
		return false;
	}
}
