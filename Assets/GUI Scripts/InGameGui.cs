using UnityEngine;
using System.Collections;
using Commands;

public class InGameGui : MonoBehaviour
{
    public ICommand PauseCmd = NullCommand.Instance;
    public ICommand ResumeCmd = NullCommand.Instance;
    public ICommand ReturnToGameCmd = NullCommand.Instance;

    public InGameGui()
    {
        PauseCmd = new ActionCommand("Pause Game", Pause);
        ResumeCmd = new ActionCommand("Resume Game", Resume);
        ReturnToGameCmd = new ActionCommand("Exit menu and return to game", Resume);
    }

    private static void Pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
    }

    private static void Resume()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
    }

    void Start()
    {
        Cursor.visible = false;
    }

    private bool Paused
    {
        get => Time.timeScale == 0;
    }

    void Update()
    {
        if (Input.GetButtonUp("Menu"))
        {
            if (Paused) ResumeCmd.Execute();
            else PauseCmd.Execute();
        }
    }

    void OnGUI()
    {
        if (Paused)
        {
            Settings_GUI.MenuClosedCmd = ReturnToGameCmd;
            Settings_GUI.settingsMenu();
        }
    }
}
