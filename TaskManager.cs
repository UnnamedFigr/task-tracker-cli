using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace task_tracker_cli
{
    public class TaskDataContainer
    {
        public List<Task> Tasks { get; set; } = new List<Task>();
        public int NextTaskId { get; set; } = 1; // default equals 1
    }

    public class TaskManager : ITaskManager
    {
        private ITaskRepository repository;
        string baseDir = AppContext.BaseDirectory;
        private string filePath; 
        private int uniqueIdCounter = 1;
        private List<Task> taskList;

        public TaskManager(ITaskRepository repository)
        {
            this.repository = repository;
            filePath = Path.Combine(baseDir, "..", "..", "..", "jsonFiles", "tasks.json");
            taskList = new List<Task>();
        }
        public void LoadTasks()
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"\nFile {filePath} not found. Starting with an empty task list.");
                File.Create(filePath, 4096);
                return;
            }
            try
            {
                string jsonStr = File.ReadAllText(filePath);
                var dataContainer = JsonSerializer.Deserialize<TaskDataContainer>(jsonStr);
                if (dataContainer?.Tasks != null)
                {
                    taskList = dataContainer.Tasks;
                    uniqueIdCounter = dataContainer.NextTaskId;
                    Console.WriteLine($"\nSuccessfully loaded {dataContainer.Tasks.Count} tasks from {filePath}.");
                }
                else
                {
                    Console.WriteLine($"\nNo tasks found from {filePath}.");
                }
            }
            catch (JsonException JE)
            {
                Console.WriteLine("JSON file is corrupted. " + JE.Message);
                taskList = new List<Task>();
                uniqueIdCounter = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading tasks " + ex.Message + ex.InnerException);
                taskList = new List<Task>();
                uniqueIdCounter = 1;
            }
        }
        public void SaveData()
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
        private Task CheckForId(int id)
        {
            Task? task = taskList.FirstOrDefault(x => x.Id == id);
            try {
                if(task != null)
                {
                    return task;
                }
                else
                {
                    throw new IndexOutOfRangeException($"The task with index - {id} cannot found. Wrong index");
                }
            } catch (IndexOutOfRangeException IOR)
            {
                Console.WriteLine(IOR.Message);
            }
            return null;
        }
        
        public bool UpdateTask(int id, string description, StatusEnum status)
        {
            if (string.IsNullOrEmpty(description)) return false;
            Task selectedTask = CheckForId(id);
            if (selectedTask == null) return false;
            selectedTask.EditTask(description);
            selectedTask.SetStatus(status);

            return true;
        }
        public bool UpdateTask(int id, StatusEnum status)
        {
            Task selectedTask = CheckForId(id);
            if (selectedTask == null) return false;
            selectedTask.SetStatus(status);

            return true;
        }
        public bool UpdateTask(int id, string description)
        {
            if (string.IsNullOrEmpty(description)) return false;
            Task selectedTask = CheckForId(id);
            if (selectedTask == null) return false; // Null check added
            selectedTask.EditTask(description);

            return true;
            //Task selectedTask = taskList.FirstOrDefault(x => x.Id == id);
            //selectedTask.EditTask(description);
        }
        public bool DeleteTask(int id)
        {
            CheckForId(id);
            Task? tasktoRemove = taskList.FirstOrDefault(x => x.Id == id);
            if (tasktoRemove == null) return false; // Null check added
            taskList.Remove(tasktoRemove);
            return true;
        }
        public void DeleteAllTasks()
        {
            taskList.Clear();
            uniqueIdCounter = 1; 

            SaveData();

            repository.Clear();
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
            if (taskList.Count == 0)
            {
                Console.WriteLine("Task list is empty");
                return;
            }
            Console.WriteLine("--- TASK LIST ---");
            Console.WriteLine($"--- WITH STATUS {status.ToString()} ---\n");
            foreach(var task in GetAllTasks(status))
            {
                Console.WriteLine(task.ToString());
            }
        }
        public void PrintAllTasks()
        {
            if (taskList.Count == 0)
            {
                Console.WriteLine("Task list is empty");
                return;
            }  
            Console.WriteLine("--- TASK LIST ---\n");
            foreach (var task in taskList)
            {
                Console.WriteLine(task.ToString());
            }
        }
    }
}
