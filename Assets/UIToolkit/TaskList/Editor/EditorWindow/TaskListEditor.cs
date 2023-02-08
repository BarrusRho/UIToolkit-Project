using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace UIToolkit.TaskList
{
    public class TaskListEditor : EditorWindow
    {
        private VisualElement _container;
        private ObjectField _savedTasksObjectField;
        private Button _loadTasksButton;
        private TextField _taskText;
        private Button _addTaskButton;
        private ScrollView _taskListScrollView;
        private Button _saveProgressButton;
        private ProgressBar _taskProgressBar;
        private ToolbarSearchField _searchBox;
        private Label _notificationLabel;

        private TaskListSO _taskListSO;

        public const string path = "Assets/UIToolkit/TaskList/Editor/EditorWindow/";

        [MenuItem("UIToolkit/Task List")]
        public static void ShowWindow()
        {
            var window = GetWindow<TaskListEditor>();
            window.titleContent = new GUIContent("Task List");
        }

        public void CreateGUI()
        {
            _container = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path + "TaskListEditor.uxml");
            if (visualTree != null)
            {
                _container.Add(visualTree.Instantiate());
            }
            else
            {
                Debug.Log($".uxml file not found");
            }

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path + "TaskListEditor.uss");
            if (styleSheet != null)
            {
                _container.styleSheets.Add(styleSheet);
            }
            else
            {
                Debug.Log($".uss file not found");
            }

            _taskText = _container.Q<TextField>("taskText");
            _taskText.RegisterCallback<KeyDownEvent>(AddTask);

            _addTaskButton = _container.Q<Button>("addTaskButton");
            _addTaskButton.clicked += AddTask;

            _taskListScrollView = _container.Q<ScrollView>("taskList");

            _savedTasksObjectField = _container.Q<ObjectField>("savedTasksObjectField");
            _savedTasksObjectField.objectType = typeof(TaskListSO);

            _loadTasksButton = _container.Q<Button>("loadTasksButton");
            _loadTasksButton.clicked += LoadTasks;

            _saveProgressButton = _container.Q<Button>("saveProgressButton");
            _saveProgressButton.clicked += SaveProgress;

            _taskProgressBar = _container.Q<ProgressBar>("taskProgressBar");

            _searchBox = _container.Q<ToolbarSearchField>("searchBox");
            _searchBox.RegisterValueChangedCallback(OnSearchTextChanged);

            _notificationLabel = _container.Q<Label>("notificationLabel");
            UpdateNotification($"Please load a ScriptableObject task list to continue.");
        }

        private void AddTask()
        {
            if (!string.IsNullOrEmpty(_taskText.value))
            {
                _taskListScrollView.Add(CreateTask(_taskText.value));
                SaveTask(_taskText.value);
                _taskText.value = String.Empty;
                _taskText.Focus();
                UpdateProgress();
                UpdateNotification($"Task added successfully.");
            }
        }

        private TaskItem CreateTask(string taskText)
        {
            var taskItem = new TaskItem(taskText);
            taskItem.GetTaskLabel().text = taskText;
            taskItem.GetTaskToggle().RegisterValueChangedCallback(UpdateProgress);
            return taskItem;
        }

        private void AddTask(KeyDownEvent e)
        {
            if (Event.current.Equals(Event.KeyboardEvent("Return")))
            {
                AddTask();
            }
        }

        private void LoadTasks()
        {
            _taskListSO = _savedTasksObjectField.value as TaskListSO;

            if (_taskListSO != null)
            {
                _taskListScrollView.Clear();

                var tasks = _taskListSO.GetTasks();

                foreach (var task in tasks)
                {
                    _taskListScrollView.Add(CreateTask(task));
                }
                
                UpdateProgress();
                UpdateNotification($"{_taskListSO.name} successfully loaded.");
            }
            else
            {
                UpdateNotification($"Failed to load task list.");
            }
        }

        private void SaveTask(string task)
        {
            _taskListSO.AddTask(task);
            EditorUtility.SetDirty(_taskListSO);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UpdateNotification($"Task added successfully.");
        }

        private void SaveProgress()
        {
            if (_taskListSO != null)
            {
                List<string> tasks = new List<string>();

                foreach (TaskItem task in _taskListScrollView.Children())
                {
                    if (!task.GetTaskToggle().value)
                    {
                        tasks.Add(task.GetTaskLabel().text);
                    }
                }
                
                _taskListSO.AddTasks(tasks);
                EditorUtility.SetDirty(_taskListSO);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                LoadTasks();
                UpdateNotification($"Tasks saved successfully.");
            }
        }

        private void UpdateProgress()
        {
            var count = 0;
            var completed = 0;

            foreach (TaskItem task in _taskListScrollView.Children())
            {
                if (task.GetTaskToggle().value)
                {
                    completed++;
                }

                count++;
            }

            if (count > 0)
            {
                var progress = completed / (float) count;
                _taskProgressBar.value = progress;
                var percentage = Mathf.Round(progress * 1000) / 10f;
                _taskProgressBar.title = $"{percentage}";
                UpdateNotification($"Progress updated. Don't forget to save.");
            }
            else
            {
                _taskProgressBar.value = 1;
                _taskProgressBar.title = $"100%";
            }
        }

        private void UpdateProgress(ChangeEvent<bool> e)
        {
            UpdateProgress();
        }

        private void OnSearchTextChanged(ChangeEvent<string> changeEvent)
        {
            var searchText = changeEvent.newValue.ToUpper();
            foreach (TaskItem task in _taskListScrollView.Children())
            {
                var taskText = task.GetTaskLabel().text.ToUpper();

                if (!string.IsNullOrEmpty(searchText) && taskText.Contains(searchText))
                {
                    task.GetTaskLabel().AddToClassList("highlight");
                }
                else
                {
                    task.GetTaskLabel().RemoveFromClassList("highlight");
                }
            }
        }

        private void UpdateNotification(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                _notificationLabel.text = text;
            }
        }
    }
}