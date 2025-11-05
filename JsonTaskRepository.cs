using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace task_tracker_cli
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly string _filePath;

        public JsonTaskRepository()
        {
            string baseDir = AppContext.BaseDirectory;
            string projectRoot = Path.Combine(baseDir, "..", "..", "..");
            _filePath = Path.Combine(projectRoot, "tasks.json");
        }

        public TaskDataContainer Load()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine($"\nFile 'tasks.json' not found. Starting with an empty task list.");
                return new TaskDataContainer();
            }

            try
            {
                string jsonStr = File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(jsonStr))
                {
                    return new TaskDataContainer();
                }
                return JsonSerializer.Deserialize<TaskDataContainer>(jsonStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tasks: {ex.Message}. Starting with an empty list.");
                return new TaskDataContainer(); // Return empty on error
            }
        }

        public void Save(TaskDataContainer data)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonStr = JsonSerializer.Serialize(data, options);
                File.WriteAllText(_filePath, jsonStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CRITICAL ERROR: Could not save tasks! {ex.Message}");
            }
        }
        public void Clear()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    File.Delete(_filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing data file: {ex.Message}");
            }
        }
    }
}
