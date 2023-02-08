using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIToolkit.TaskList
{
    [CreateAssetMenu(menuName = "Task List", fileName = "New Task List")]
    public class TaskListSO : ScriptableObject
    {
        [SerializeField] private List<string> _tasks = new List<string>();

        public List<string> GetTasks()
        {
            return _tasks;
        }

        public void AddTask(string savedTask)
        {
            _tasks.Add(savedTask);
        }

        public void AddTasks(List<string> savedTasks)
        {
            _tasks.Clear();
            _tasks = savedTasks;
        }
    }
}