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

    public static ICommand ChangeExtendedMenuToResolution = new ActionCommand(() => CurrentExtendedMenu = ExtendedMenu.Res);
    public static ICommand ChangeExtendedMenuToQuality = new ActionCommand(() => CurrentExtendedMenu = ExtendedMenu.Quality);
    public static ICommand CloseExtendedMenu = new ActionCommand(() => CurrentExtendedMenu = ExtendedMenu.None);
    public static ICommand FullscreenOn = new ActionCommand(() => Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen);
    public static ICommand FullscreenOff = new ActionCommand(() => Screen.fullScreenMode = FullScreenMode.Windowed);

    public static List<(string name, ICommand cmd)> ChangeQualityModeCommands = QualitySettings.names.Select((x, i) => (x, new ActionCommand(() => QualitySettings.SetQualityLevel(i)) as ICommand)).ToList();
    public static List<(string name, ICommand cmd)> ChangeResolutionCommands = Screen.resolutions.Select(x => ($"{x.width}x{x.height}@{x.refreshRate}", new ActionCommand(() => Screen.SetResolution(x.width, x.height, Screen.fullScreen, x.refreshRate)) as ICommand)).ToList();

    private static ExtendedMenu CurrentExtendedMenu = ExtendedMenu.None;
    private static int curResIndx = Array.IndexOf(Screen.resolutions, Screen.currentResolution);
    private static Vector2 scrollPosition;

    public static bool settingsMenu()
    {
        int width = 100;
        int height = 170;
        int left;
        if (CurrentExtendedMenu != ExtendedMenu.None)
            left = (Screen.width - width * 2 - 10) / 2;
        else
            left = (Screen.width - width) / 2;

        int top = (Screen.height - height) / 2;


        GUILayout.BeginArea(new Rect(left, top, width, height), GUI.skin.GetStyle("box"));
        GUILayout.Box("Settings");
        GUILayout.Space(10);

        if (GUILayout.Button("Quality"))
        {
            if (CurrentExtendedMenu == Settings_GUI.ExtendedMenu.Quality)
                CloseExtendedMenu.Execute();
            else
                ChangeExtendedMenuToQuality.Execute();
        }

        if (GUILayout.Button("Resolution"))
        {
            if (CurrentExtendedMenu == ExtendedMenu.Res)
                CloseExtendedMenu.Execute();
            else
                ChangeExtendedMenuToResolution.Execute();
        }

        if (GUILayout.Toggle(Screen.fullScreen, "Fullscreen"))
        {
            FullscreenOn.Execute();
        }
        else
        {
            FullscreenOff.Execute();
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Back"))
        {
            return true;
        }

        GUILayout.EndArea();

        if (CurrentExtendedMenu == ExtendedMenu.Res)
        {
            GUILayout.BeginArea(new Rect(left + width + 10, top + 20, width, height - 20));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            int newResIndx = GUILayout.SelectionGrid(curResIndx, ChangeResolutionCommands.Select(x => x.name).ToArray(), 1);
            if (newResIndx != curResIndx)
            {
                curResIndx = newResIndx;
                ChangeResolutionCommands[newResIndx].cmd.Execute();
                CloseExtendedMenu.Execute();
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
                ChangeQualityModeCommands[newModeIdx].cmd.Execute();

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        return false;
    }
}
