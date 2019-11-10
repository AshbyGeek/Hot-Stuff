using UnityEngine;
using Commands;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

public class Settings_GUI : MonoBehaviour
{
    private enum ExtendedMenu
    {
        None,
        Res,
        Quality
    };

    public static ICommand ChangeExtendedMenuToResolutionCmd = new ActionCommand("Open Resolution Sub Menu", () => CurrentExtendedMenu = ExtendedMenu.Res);
    public static ICommand ChangeExtendedMenuToQualityCmd = new ActionCommand("Open Quality Sub Menu", () => CurrentExtendedMenu = ExtendedMenu.Quality);
    public static ICommand CloseExtendedMenuCmd = new ActionCommand("Close Sub Menu", () => CurrentExtendedMenu = ExtendedMenu.None);
    public static ICommand FullscreenOnCmd = new ActionCommand("Enter Fullscreen", () => Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen);
    public static ICommand FullscreenOffCmd = new ActionCommand("Exit Fullscreen", () => Screen.fullScreenMode = FullScreenMode.Windowed);
    public static ICommand MenuClosedCmd = NullCommand.Instance;

    public static List<(string name, ICommand cmd)> ChangeQualityModeCommands = QualitySettings.names.Select((x, i) => (x, new ActionCommand("Change to quality: " + x, () => QualitySettings.SetQualityLevel(i)) as ICommand)).ToList();
    public static List<(string name, ICommand cmd)> ChangeResolutionCommands = Screen.resolutions.Select(x => (GetName(x), new ActionCommand("Change Resolution to: " + GetName(x), () => Screen.SetResolution(x.width, x.height, Screen.fullScreen, x.refreshRate)) as ICommand)).ToList();

    private static string GetName(Resolution res) => $"{res.width}x{res.height}@{res.refreshRate}";

    private static ExtendedMenu CurrentExtendedMenu = ExtendedMenu.None;
    private static int curResIndx = Array.IndexOf(Screen.resolutions, Screen.currentResolution);
    private static Vector2 scrollPosition;

    public static bool settingsMenu()
    {
        int width = 100;
        int height = 170;
        int left = (Screen.width - width) / 2;
        int top = (Screen.height - height) / 2;


        GUILayout.BeginArea(new Rect(left, top, width, height), GUI.skin.GetStyle("box"));
        GUILayout.Box("Settings");
        GUILayout.Space(10);

        if (GUILayout.Button("Quality"))
        {
            if (CurrentExtendedMenu == Settings_GUI.ExtendedMenu.Quality)
                CloseExtendedMenuCmd.Execute();
            else
                ChangeExtendedMenuToQualityCmd.Execute();
        }

        if (GUILayout.Button("Resolution"))
        {
            if (CurrentExtendedMenu == ExtendedMenu.Res)
                CloseExtendedMenuCmd.Execute();
            else
                ChangeExtendedMenuToResolutionCmd.Execute();
        }

        if (GUILayout.Toggle(Screen.fullScreen, "Fullscreen"))
        {
            FullscreenOnCmd.Execute();
        }
        else
        {
            FullscreenOffCmd.Execute();
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Back"))
        {
            MenuClosedCmd.Execute();
        }

        GUILayout.EndArea();

        if (CurrentExtendedMenu == ExtendedMenu.Res)
        {
            GUILayout.BeginArea(new Rect(left + width + 10, top + 20, width + 15, height - 20));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            int newResIndx = GUILayout.SelectionGrid(curResIndx, ChangeResolutionCommands.Select(x => x.name).ToArray(), 1);
            if (newResIndx != curResIndx)
            {
                curResIndx = newResIndx;
                ChangeResolutionCommands[newResIndx].cmd.Execute();
                CloseExtendedMenuCmd.Execute();
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        if (CurrentExtendedMenu == Settings_GUI.ExtendedMenu.Quality)
        {
            GUILayout.BeginArea(new Rect(left + width + 10, top + 20, width, height - 20));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            int curModeIdx = QualitySettings.GetQualityLevel();
            int newModeIdx = GUILayout.SelectionGrid(curModeIdx, ChangeQualityModeCommands.Select(x => x.name).ToArray(), 1);
            if (newModeIdx != curModeIdx)
            {
                ChangeQualityModeCommands[newModeIdx].cmd.Execute();
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        return false;
    }
}
