using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


namespace UIToolkit.TaskList
{
    public class TaskItem : VisualElement
    {
        private Toggle _taskToggle;
        private Label _taskLabel;

        #region constructor
        public TaskItem(string taskText)
        {
            VisualTreeAsset original =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TaskListEditor.path + "TaskItem.uxml");

            this.Add(original.Instantiate());

            _taskToggle = this.Q<Toggle>();
            _taskLabel = this.Q<Label>();

            _taskLabel.text = taskText;
        }
        #endregion

        public Toggle GetTaskToggle()
        {
            return _taskToggle;
        }

        public Label GetTaskLabel()
        {
            return _taskLabel;
        }
    }
}