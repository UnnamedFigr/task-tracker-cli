using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_tracker_cli
{
    public interface ITaskManager
    {
        void AddTask(string description);
        bool DeleteTask(int id);
        void DeleteAllTasks();
        bool UpdateTask(int id, string description);
        bool UpdateTask(int id, StatusEnum status);
        IReadOnlyCollection<Task> GetAllTasks();
        IReadOnlyCollection<Task> GetAllTasks(StatusEnum status);
        public void PrintAllTasks();
        public void PrintAllTasks(StatusEnum status);
    }
}
