using UnityEngine;
using Commands;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameMenu
{
    Main,
    Settings
}

public class Loader_GUI : MonoBehaviour
{

    public static GameMenu CurrentGameMenu = GameMenu.Main;

    public ICommand QuitCommandCmd = new ActionCommand("Quit Game", Application.Quit);

    public ICommand LoadGameCmd = new ActionCommand("Start Game", () => SceneManager.LoadScene("Game"));

    public ICommand ChangeToSettingsMenuCmd = new ActionCommand("Open Settings Menu", () => CurrentGameMenu = GameMenu.Settings);

    public ICommand ChangeToMainMenuCmd = new ActionCommand("Return to main menu", () => CurrentGameMenu = GameMenu.Main);

    void OnGUI()
    {
        Settings_GUI.MenuClosedCmd = ChangeToMainMenuCmd;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (CurrentGameMenu == GameMenu.Main)
        {
            // Make a background box
            int width = 100;
            int height = 120;
            int left = (Screen.width - width) / 2;
            int top = (Screen.height - height) / 2;

            GUI.BeginGroup(new Rect(left, top, width, height));
            GUI.Box(new Rect(0, 0, 100, 120), "Loader Menu");

            // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
            if (GUI.Button(new Rect(10, 30, 80, 20), "Load Level"))
            {
                LoadGameCmd.Execute();
            }

            if (GUI.Button(new Rect(10, 60, 80, 20), "Settings"))
            {
                ChangeToSettingsMenuCmd.Execute();
            }

            // Make the second button.
            if (GUI.Button(new Rect(10, 90, 80, 20), "exit"))
            {
                QuitCommandCmd.Execute();
            }

            GUI.EndGroup();
        }
        else if (CurrentGameMenu == GameMenu.Settings)
        {
            Settings_GUI.MenuClosedCmd = ChangeToMainMenuCmd;
            Settings_GUI.settingsMenu();
        }
    }
}
