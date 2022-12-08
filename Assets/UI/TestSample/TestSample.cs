using System;
using UnityEngine;
using UnityEngine.UIElements;

public class TestSample : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _visualRoot;
    private Button _button1;
    private Button _button2;
    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        _visualRoot = _uiDocument.rootVisualElement;
        
        _button1 = _visualRoot.Q<Button>("myButton1");
        _button1.RegisterCallback<ClickEvent>(elem => _button1.style.backgroundColor = Color.yellow);

        _button2 = _visualRoot.Q<Button>("myButton2");
        _button2.RegisterCallback<ClickEvent>(elem => _button2.style.backgroundColor = Color.blue);
    }
}
