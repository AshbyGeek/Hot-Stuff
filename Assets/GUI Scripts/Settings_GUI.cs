using UnityEngine;
using System.Collections;

public class Settings_GUI : MonoBehaviour {
	private enum ExtendedMenu{
		None,
		Res
	};
	
	private ExtendedMenu curMenu = ExtendedMenu.None;
	private int toolbarSelected = 0;
	private Vector2 scrollPosition;		
	private bool fullscreen;
	
	void Start(){
		fullscreen = Screen.fullScreen;
	}
	
	void OnGUI () {
		int width = 100;
		int height = 120;
		int left;
		if (curMenu != ExtendedMenu.None)
			left = (Screen.width - width*2 - 10)/2;
		else
			left = (Screen.width - width)/2;
		
		int top = (Screen.height - height)/2;
		
		
		GUILayout.BeginArea(new Rect(left,top,width,height),GUI.skin.GetStyle("box"));		
			GUILayout.Box("Settings");
			GUILayout.Space(10);
		
			if (GUILayout.Button("Resolution")){
				if (curMenu == ExtendedMenu.Res)
					curMenu = ExtendedMenu.None;
				else
					curMenu = ExtendedMenu.Res;
			}
			fullscreen = GUILayout.Toggle(fullscreen,"Fullscreen");
			if (fullscreen != Screen.fullScreen)
				Screen.fullScreen = fullscreen;
		
			if (GUILayout.Button("Main Menu")) {
				Application.LoadLevel("Loader");
			}
		
		GUILayout.EndArea();
		
		if (curMenu == ExtendedMenu.Res){
			GUILayout.BeginArea(new	Rect(left + width + 10,top+20,width,height-20));
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
				
			Resolution[] resolutions = Screen.resolutions;
			GUIContent[] list = new GUIContent[resolutions.Length];
			for(int i = 0; i < resolutions.Length; i++){
				list[i] = new GUIContent(resolutions[i].width.ToString() + "x" +
			                         resolutions[i].height.ToString());
			}
			int prev = toolbarSelected;
			toolbarSelected = GUILayout.SelectionGrid(toolbarSelected,list,1);
			if (toolbarSelected != prev){
				curMenu = ExtendedMenu.None;
				Resolution res = resolutions[toolbarSelected];
				Screen.SetResolution(res.width,res.height,fullscreen);
			}
			
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
	}
}
