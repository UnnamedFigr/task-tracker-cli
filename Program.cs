using System.ComponentModel.Design;
using System.Data;
using System.Runtime.InteropServices;

namespace task_tracker_cli
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to task tracker CLI\n");
            TaskManager tm = new TaskManager();
            while (true)
            {
                Console.WriteLine();
                Console.Write("> ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.Write("Unspecified command.\n");
                }
                
                var parts = input.ToLower().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if(parts.Length > 3 || parts.Length < 1)
                {
                    throw new IndexOutOfRangeException("Incorrectly structured commands");
                }
                
                string command = parts[0];
                string value= "";
                int selectedTaskId = 0;
                if (parts.Length == 3 && int.TryParse(parts[1], out selectedTaskId))
                {
                    value = parts[2];
                }
                else if(parts.Length == 2)
                {
                    value = parts[1];
                }
                switch (command)
                {
                    case "add":
                        tm.AddTask(value);
                        break;
                    case "update":
                        tm.UpdateTask(selectedTaskId, value); 
                        break;
                    case "delete":
                        tm.DeleteTask(selectedTaskId);
                        break;
                    case "mark-in-progress":
                        tm.UpdateTask(selectedTaskId, StatusEnum.inProgress);
                        break;
                    case "mark-done":
                        tm.UpdateTask(selectedTaskId, StatusEnum.done);
                        break;
                    case "list":
                        if (!string.IsNullOrEmpty(value))
                        {
                            tm.PrintAllTasks(Enum.Parse<StatusEnum>(value, ignoreCase: true));
                            break;
                        }
                        else
                        {
                            tm.PrintAllTasks();
                            break;
                        }
                    default:
                        break;

                }
            }

            //TaskManager tm = new TaskManager();
            tm.AddTask("Martines");
            tm.AddTask("Martines");
            tm.AddTask("Martines");
            tm.AddTask("Martines");
            tm.AddTask("Martines");
            var tasks = tm.GetAllTasks();
            tm.PrintAllTasks();
            tm.DeleteTask(2);
            tm.PrintAllTasks();
            tm.UpdateTask(3, StatusEnum.inProgress);
            tm.PrintAllTasks(StatusEnum.inProgress);
            tm.PrintAllTasks(StatusEnum.todo);
        }
    }
}
