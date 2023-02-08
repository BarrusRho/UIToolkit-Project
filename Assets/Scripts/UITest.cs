using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class UITest : EditorWindow
{
    private VisualElement _container;
    
    [MenuItem("Testing/Test Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<UITest>();
        window.titleContent = new GUIContent($"Test Window");
        window.minSize = new Vector2(500, 500);
    }
    
    public void CreateGUI()
    {
        _container = rootVisualElement;
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UXML/Test.uxml");
        _container.Add(visualTree.Instantiate());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/USS/test.uss");
        _container.styleSheets.Add(styleSheet);
        
    }
}
