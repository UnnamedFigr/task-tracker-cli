using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_tracker_cli
{
    public class InputHandler
    {
        private TaskManager tm = new TaskManager();
        public string Text { get; private set; }
        public string[] Parts { get; private set; }

        //private fields
        private string command;
        private int selectedTaskId;
        private string value;
        public InputHandler()
        {
            
        }
        
        private void SplitCommand(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                Console.Write("Unspecified command.\n");
            }

            Parts = input.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);

            if (Parts.Length < 1)
            {
                throw new ArgumentNullException("Incorrectly structured commands");
            }

            command = Parts[0];
            selectedTaskId = 0;
            value = "";
            if (Parts.Length == 3)
            {
                if (int.TryParse(Parts[1], out selectedTaskId))
                {
                    if (!string.IsNullOrEmpty(Parts[2]))
                    {
                        value = Parts[2];
                    }                
                }
            }
            else if(Parts.Length == 2)
            {
                if (int.TryParse(Parts[1], out selectedTaskId))
                {
                    return;
                }
                value = Parts[1];
                return;
            }
            else if(command == "list")
            {
                return; 
            }
                      
        }
        public void HandleCommand(string input)
        {
            Console.WriteLine();

            SplitCommand(input);
            
            switch (command)
            {
                case "save":
                    tm.SaveTasks();
                    break;
                case "load":
                    tm.LoadTasks();
                    break;
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
                case "mark-todo":
                    tm.UpdateTask(selectedTaskId, StatusEnum.todo);
                    break;
                case "list":
                    if (Enum.TryParse<StatusEnum>(value, ignoreCase: true, out StatusEnum res) && !string.IsNullOrEmpty(value))
                    {                       
                        tm.PrintAllTasks(res);
                        break;
                    }
                    else if (string.IsNullOrEmpty(value) && Parts.Length == 1)
                    {
                        tm.PrintAllTasks();
                        break;
                    }
                    break;
                default:
                    Console.WriteLine("Could not recognise command");
                    break;

            }
        }
    }
}
