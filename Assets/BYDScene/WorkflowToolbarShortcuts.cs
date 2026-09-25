using System.Linq;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public static class WorkflowToolbarShortcuts
{
    [Shortcut("Workflow/Hierarchy", KeyCode.Mouse3)]
    static void ToggleHierarchy()
    {
        EditorWindow window = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault(x => x.titleContent.text == "Hierarchy");

        if (window != null) window.Close();
        else EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
    }

    [Shortcut("Workflow/Inspector", KeyCode.Mouse3, ShortcutModifiers.Control)]
    static void ToggleInspector()
    {
        EditorWindow window = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault(x => x.titleContent.text == "Inspector");

        if (window != null) window.Close();
        else EditorApplication.ExecuteMenuItem("Window/General/Inspector");
    }

    [Shortcut("Workflow/Project", KeyCode.Mouse4)]
    static void ToggleProject()
    {
        EditorWindow window = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault(x => x.titleContent.text == "Project");

        if (window != null) window.Close();
        else EditorApplication.ExecuteMenuItem("Window/General/Project");
    }

    [Shortcut("Workflow/Game", KeyCode.Mouse4, ShortcutModifiers.Control)]
    static void ToggleGame()
    {
        EditorWindow window = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault(x => x.titleContent.text == "Game");

        if (window != null) window.Close();
        else EditorApplication.ExecuteMenuItem("Window/General/Game");
    }

    [Shortcut("Workflow/CloseAll", KeyCode.Space, ShortcutModifiers.Control | ShortcutModifiers.Shift)]
    static void CloseAll()
    {
        CloseWindow("Hierarchy");
        CloseWindow("Inspector");
        CloseWindow("Project");
        CloseWindow("Game");
        CloseWindow("Console");
    }

    static void CloseWindow(string windowName)
    {
        EditorWindow window = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault(x => x.titleContent.text == windowName);
        if (window != null) window.Close();
    }
}
