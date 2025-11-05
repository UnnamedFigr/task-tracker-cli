using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_tracker_cli
{
    public interface ITaskRepository
    {
        TaskDataContainer Load();
        void Save(TaskDataContainer data);
        void Clear();
    }
}
