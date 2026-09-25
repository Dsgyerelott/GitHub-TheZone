using UnityEngine;
using UnityEditor;
using Unity.Hierarchy.Editor;
using System.Reflection;
using Unity.Hierarchy;
using UnityEngine.UIElements;
using System;

[InitializeOnLoad]
static class BYDHierarchy
{
    static BYDHierarchy()
    {
        HierarchyWindow.BindView += OnBindView;
        HierarchyWindow.BindViewItem += OnBindViewItem;
    }

    private static void OnBindViewItem(HierarchyWindow window, HierarchyView view, HierarchyViewItem item)
    {
        
    }

    static void OnBindView(HierarchyWindow window, HierarchyView hierarchyView)
    {
        var styleSheet = UnityEditor.AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BYDHierarchy/BYDHierarchy_USS.uss");
        window.rootVisualElement.styleSheets.Add(styleSheet);

        var collection = hierarchyView.Q<VisualElement>("unity-tree-view__list-view");
        var property = collection.GetType().GetProperty("fixedItemHeight", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        property?.SetValue(collection, 20f);
    }
}