using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_tracker_cli
{
    public class Task
    {
        private int id;
        private string description;
        private StatusEnum status;
        private DateTime createdAt;
        private DateTime updatedAt;

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
            get { return description; }
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

        public void SetStatus(StatusEnum status) {
            if(status == this.Status)
            {
                Console.WriteLine("The task status is already marked");
            }
            this.status = status;
            this.updatedAt = DateTime.UtcNow;
        }
        public void EditTask(string description)
        {
            this.description = description;
            this.updatedAt = DateTime.UtcNow;
        }
        public override string ToString()
        {
            return $"Task ID: {Id}\nTask Status: {status.ToString()}\ntask description \"{description}\"\nCreated at: {createdAt}\nLast Updated: {updatedAt}\n";
        }
    }
}
