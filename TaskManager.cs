using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace task_tracker_cli
{
    public class TaskDataContainer
    {
        public List<Task> Tasks { get; set; } = new List<Task>();
        public int NextTaskId { get; set; } = 1; // default equals 1
    }

    public class TaskManager
    {
        private const string filePath = ".\\jsonFiles\\tasks.json";
        private int uniqueIdCounter = 1;
        private List<Task> taskList;

        public TaskManager()
        {
            taskList = new List<Task>();
        }
        public void LoadTasks()
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"\nFile {filePath} not found. Starting with an empty task list.");
                return;
            }
            try
            {
                string jsonStr = File.ReadAllText(filePath);
                var dataContainer = JsonSerializer.Deserialize<TaskDataContainer>(jsonStr);
                if (dataContainer.Tasks != null)
                {
                    taskList = dataContainer.Tasks;
                    uniqueIdCounter = dataContainer.NextTaskId;
                    Console.WriteLine($"\nSuccessfully loaded {dataContainer.Tasks.Count} tasks from {filePath}.");
                }
                else
                {
                    Console.WriteLine($"\nSuccessfully loaded {dataContainer.Tasks.Count} tasks from {filePath}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading tasks " + ex.Message);
                taskList = new List<Task>();
                uniqueIdCounter = 1;
            }
        }
        public void SaveTasks()
        {
            try
            {
                var dataContainer = new TaskDataContainer
                {
                    Tasks = taskList,
                    NextTaskId = uniqueIdCounter
                };
                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonStr = JsonSerializer.Serialize(dataContainer, options);
                File.WriteAllText(filePath, jsonStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving tasks " + ex.Message);
            }
        }
        public void AddTask(string description)
        {
            
            try
            {
                if (description == "") throw new ArgumentNullException("No description provided");
                int id = uniqueIdCounter;
                uniqueIdCounter++;
                string taskDesc = description;

                taskList.Add(new Task(id, taskDesc));
            } catch (ArgumentNullException ANE)
            {
                Console.WriteLine(ANE.Message);
            }
        }
        private void CheckForId(int id)
        {
            Task? task = taskList.FirstOrDefault(x => x.Id == id);
            try {
                if(task != null)
                {
                    return;
                }
                else
                {
                    throw new IndexOutOfRangeException($"The task with index - {id} cannot found. Wrong index");
                    
                }
            } catch (IndexOutOfRangeException IOR)
            {
                Console.WriteLine(IOR.Message);
            }
        }
        
        public void UpdateTask(int id, string description, StatusEnum status)
        {
            if (description == null) return;
            CheckForId(id);
            Task selectedTask = taskList.FirstOrDefault(x => x.Id == id);
            selectedTask.EditTask(description);
            selectedTask.SetStatus(status);
        }
        public void UpdateTask(int id, StatusEnum status)
        {
            CheckForId(id);
            Task selectedTask = taskList.FirstOrDefault(x=> x.Id == id);
            selectedTask.SetStatus(status);
        }
        public void UpdateTask(int id, string description)
        {
            if (description == null) return;
            CheckForId(id);
            taskList.FirstOrDefault(x => x.Id == id).EditTask(description);
        }
        public void DeleteTask(int id)
        {
            CheckForId(id);
            Task tasktoRemove = taskList.FirstOrDefault(x => x.Id == id);
            taskList.Remove(tasktoRemove);
        }
                
        public IReadOnlyCollection<Task> GetAllTasks()
        {
            if (taskList != null) return taskList.AsReadOnly();
            return null;
        }
        public IReadOnlyCollection<Task> GetAllTasks(StatusEnum status)
        {
            switch (status)
            {
                case StatusEnum.todo:
                    return taskList.Where(x => x.Status == StatusEnum.todo).ToList().AsReadOnly();
                case StatusEnum.done:
                    return taskList.Where(x => x.Status == StatusEnum.done).ToList().AsReadOnly();
                case StatusEnum.inProgress:
                    return taskList.Where(x => x.Status == StatusEnum.inProgress).ToList().AsReadOnly();
                default:
                    return null;
            }
        }
        public void SerializeAllTasks()
        {
            
        }
        public void PrintAllTasks(StatusEnum status)
        {
            Console.WriteLine("--- TASK LIST ---");
            Console.WriteLine($"--- WITH STATUS {status.ToString()} ---\n");
            foreach(var task in GetAllTasks(status))
            {
                Console.WriteLine(task.ToString());
            }
        }
        public void PrintAllTasks()
        {
            if (taskList == null) return;
            Console.WriteLine("--- TASK LIST ---\n");
            foreach (var task in taskList)
            {
                Console.WriteLine(task.ToString());
            }
        }
    }
}
