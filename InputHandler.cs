using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_tracker_cli
{
    public class InputHandler
    {
        private ITaskManager tm;
        public string Text { get; private set; }
        public string[] Parts { get; private set; }

        //private fields
        private string command;
        private int selectedTaskId;
        private string value;
        public InputHandler()
        {
            // Optional: Load tasks at initialization
            ITaskRepository repository = new JsonTaskRepository();
            tm = new TaskManager(repository);
        }

        private void SplitCommand(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Unspecified command. Please enter a command.");
            }

            Parts = input.Trim().Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

            if (Parts.Length < 1)
            {
                throw new ArgumentException("Incorrectly structured command.");
            }

            command = Parts[0].ToLowerInvariant();
            string argString = Parts.Length > 1 ? Parts[1].Trim() : string.Empty;

            selectedTaskId = 0;
            value = "";

            if (command == "delete" || command.StartsWith("mark-"))
            {
                if (argString.Trim() == "all")
                {
                    selectedTaskId = 0;
                    value = "all";
                }
                if (!int.TryParse(argString, out selectedTaskId) || selectedTaskId <= 0)
                {
                    selectedTaskId = 0;                    
                }

            }

            else if (command == "add" || command == "list")
            {
                value = argString;
            }
            else if (command == "update")
            {
                string[] updateParts = argString.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (updateParts.Length >= 2 && int.TryParse(updateParts[0], out selectedTaskId) && selectedTaskId > 0)
                {
                    value = updateParts[1].Trim();
                }
                else
                {
                    selectedTaskId = 0;
                }
            }
        }

        public bool HandleCommand(string input)
        {
            try
            {
                Console.WriteLine();
                SplitCommand(input);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return true; // Keep running
            }

            switch (command)
            {
                case "quit":
                    Console.WriteLine("Thank you. Good bye!");
                    return false;
                case "add":
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        Console.WriteLine("Error: 'add' command requires a description.");
                        break;
                    }
                    tm.AddTask(value);
                    break;
                case "update":
                    if (selectedTaskId <= 0 || string.IsNullOrWhiteSpace(value))
                    {
                        Console.WriteLine("Error: 'update' requires a valid Task ID and a new description. Usage: update <ID> <new description>");
                        break;
                    }
                    tm.UpdateTask(selectedTaskId, value);
                    break;
                case "delete":
                    if(value == "all")
                    {
                        tm.DeleteAllTasks();
                        break;
                    }
                    if (selectedTaskId <= 0 && value != "all")
                    {
                        Console.WriteLine("Error: 'delete' requires a valid Task ID or \"all\" argument");
                        break;
                    }  
                    tm.DeleteTask(selectedTaskId);
                    break;
                case "mark-in-progress":
                case "mark-done":
                case "mark-todo":
                    if (selectedTaskId <= 0)
                    {
                        Console.WriteLine($"Error: '{command}' requires a valid Task ID.");
                        break;
                    }
                    // Extract status from command name
                    StatusEnum status;
                    if (command == "mark-in-progress") status = StatusEnum.inProgress;
                    else if (command == "mark-done") status = StatusEnum.done;
                    else status = StatusEnum.todo;

                    tm.UpdateTask(selectedTaskId, status);
                    break;
                case "list":
                    if (string.IsNullOrEmpty(value))
                    {
                        tm.PrintAllTasks();
                    }
                    else if (Enum.TryParse<StatusEnum>(value, ignoreCase: true, out StatusEnum res))
                    {
                        tm.PrintAllTasks(res);
                    }
                    else
                    {
                        Console.WriteLine($"Error: Cannot filter by status '{value}'. Use 'todo', 'inProgress', or 'done'.");
                    }
                    break;
                default:
                    Console.WriteLine($"Could not recognise command: '{command}'");
                    break;

            }
            return true;
        }
    }
}
