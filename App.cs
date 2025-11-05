using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace task_tracker_cli
{
    public class App
    {
        public App() { }
        public void Run()
        {
            Console.WriteLine("Welcome to task tracker CLI\n");
            TaskManager tm = new TaskManager();
            InputHandler inputHandler = new InputHandler();
            while (true)
            {
                try
                {
                    if (!inputHandler.HandleCommand(Console.ReadLine()))
                    {
                        break;
                    }
                    
                }
                catch (ArgumentNullException ANE)
                {
                    Console.WriteLine(ANE.Message.ToString() + "\nTry again.");

                }
            }
        }
    }
}
