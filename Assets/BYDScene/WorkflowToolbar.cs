using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), "Workflow Toolbar")]
public class WorkflowToolbar : Overlay
{
    private Dictionary<EditorWindow, bool> EditorWindowState;
    private string styleSheetPath = "Assets/BYDScene/WorkflowToolbar.uss";

    public override VisualElement CreatePanelContent()
    {
        var root = new VisualElement();

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(styleSheetPath);
        root.styleSheets.Add(styleSheet);

        VisualElement defaultButtonsContainer = new();
        defaultButtonsContainer.style.flexDirection = FlexDirection.Row;

        defaultButtonsContainer.Add(CreateButton("Hierarchy", "Window/General/Hierarchy"));
        defaultButtonsContainer.Add(CreateButton("Inspector", "Window/General/Inspector"));
        defaultButtonsContainer.Add(CreateButton("Project", "Window/General/Project"));
        defaultButtonsContainer.Add(CreateButton("Game", "Window/General/Game"));
        defaultButtonsContainer.Add(CreateButton("Console", "Window/General/Console"));

        root.Add(defaultButtonsContainer);

        VisualElement layoutButtonsContainer = new();
        layoutButtonsContainer.style.flexDirection = FlexDirection.Row;

        List<string> layout_01 = new(){"Window/General/Hierarchy", "Window/General/Inspector"};
        layoutButtonsContainer.Add(CreateButtonLayout("HierarchyAndInspector", "Hi + Ins", "Hierarchy", "Inspector", "Window/General/Hierarchy", "Window/General/Inspector", "Project"));
        layoutButtonsContainer.Add(CreateButtonLayout("ProjectAndInspector", "Pro + Ins", "Project", "Inspector", "Window/General/Project", "Window/General/Inspector", "Hierarchy"));

        root.Add(layoutButtonsContainer);

        root.Add(CreateCloseButton("CloseButton"));

        return root;
    }

    Button CreateButton(string name, string menuPath)
    {
        Button button = new();
        button.name = name;
        button.AddToClassList("Workflow_Button");
        button.clicked += () => ToggleWindow(name, menuPath);

        return button;
    }

    Button CreateCloseButton(string name)
    {
        Button button = new();
        button.name = name;
        button.text = "Close All Windows";
        button.AddToClassList("Workflow_Button");
        button.clicked += () => 
        {
            CloseWindow("Hierarchy");
            CloseWindow("Inspector");
            CloseWindow("Project");
            CloseWindow("Game");
            CloseWindow("Console");
        };

        return button;
    }

    Button CreateButtonLayout(string nameButton, string textButton, string name_01, string name_02, string menuPath_01, string menuPath_02, string closeWindow_01)
    {
        Button button = new();
        button.name = nameButton;
        button.text = textButton;
        button.AddToClassList("Workflow_Button");
        button.clicked += () => 
        {
            CloseWindow(closeWindow_01);
            CloseWindow("Game");
            CloseWindow("Console");

            ToggleWindowLayout(name_01, menuPath_01, name_02, menuPath_02);
        };

        return button;
    }

    void ToggleWindow(string windowName, string menuPath)
    {
        EditorWindow window = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault(x => x.titleContent.text == windowName);

        if (window != null) window.Close();
        else EditorApplication.ExecuteMenuItem(menuPath);
    }

    void ToggleWindowLayout(string name_01, string menuPath_01, string name_02, string menuPath_02)
    {
        bool window01Open = IsWindowOpen(name_01);
        bool window02Open = IsWindowOpen(name_02);

        if (window01Open && window02Open)
        {
            CloseWindow(name_01);
            CloseWindow(name_02);
        }
        else
        {
            OpenWindow(name_01, menuPath_01);
            OpenWindow(name_02, menuPath_02);
        }
    }

    bool IsWindowOpen(string windowName)
    {
        return Resources.FindObjectsOfTypeAll<EditorWindow>().Any(x => x.titleContent.text == windowName);
    }

    void OpenWindow(string windowName, string menuPath)
    {
        if (!IsWindowOpen(windowName)) EditorApplication.ExecuteMenuItem(menuPath);
    }

    void CloseWindow(string windowName)
    {
        EditorWindow window = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault(x => x.titleContent.text == windowName);
        if (window != null) window.Close();
    }
}