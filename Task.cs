using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace task_tracker_cli
{
    public class Task
    {
        private int id;
        private string? description;
        private StatusEnum status;
        private DateTime createdAt;
        private DateTime updatedAt;

        [JsonConstructor]
        public Task(int id, string description)
        {
            this.Id = id;
            this.Description = description;
            this.Status = StatusEnum.todo;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }

        public string Description 
        {
            get { return description ?? ""; }
            private set { description = value; }
        }

        public int Id
        {
            private set { this.id = value; }
            get { return id; }
        }
        public StatusEnum Status 
        {
            get { return status; }
            private set { status = value; }
        }
        public DateTime CreatedAt 
        {
            get { return createdAt; }
            private set { createdAt = value; }
        }
        public DateTime UpdatedAt
        {
            get { return updatedAt; }
            private set { updatedAt = value; }
        }

        public bool SetStatus(StatusEnum status) {
            if(status == this.Status)
            {
                Console.WriteLine("The task status is already marked");
                return false;
            }
            this.Status = status;
            this.UpdatedAt = DateTime.UtcNow;
            Console.WriteLine("Task status updated successfully");
            return true;
        }
        public bool EditTask(string description)
        {
            if(Description == description)
            {
                Console.WriteLine("Current description is the same.");
                return false;
            }
            this.Description = description;
            this.UpdatedAt = DateTime.UtcNow;
            Console.WriteLine("Task description updated successfully");
            return true;
        }
        public override string ToString()
        {
            return $"Task ID: {Id}\nTask Status: {status.ToString()}\ntask description \"{description}\"\nCreated at: {createdAt}\nLast Updated: {updatedAt}\n";
        }
    }
}
