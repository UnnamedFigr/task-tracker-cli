# task-tracker-cli
**🚀 C# Persistent CLI Task Tracker**\
_A console-based task management application built in C#, featuring object-oriented design principles, JSON persistence, and robust command handling._

 **✨ Features**  
- Persistent Storage: Tasks are automatically saved to and loaded from a local JSON file (tasks.json).
- Unique ID Generation: Uses an auto-incrementing integer ID mechanism to guarantee unique task identifiers across sessions.
- CRUD Operations: Supports creating, listing, updating (description/status), and deleting tasks.
- Status Management: Tasks can be explicitly marked as todo, inProgress, or done.
- Object-Oriented Design: Clear separation of concerns between Task entity, TaskManager (data logic/persistence), and InputHandler (CLI logic).

**🛠️ Prerequisites**
- .NET SDK (6.0 or higher recommended)  
- A C# IDE (Visual Studio, VS Code with C# extension, etc.)

**How to use:**
``` 
  git clone https://github.com/UnnamedFigr/task-tracker-cli.git
  cd task-tracker-cli
```
- To add a task
```
  add Buy groceries
  add 1 apple to the bag
```
- To remove a task 
``` delete [task ID] 
	delete all  \\ to delete all tasks
```
- See all tasks
```
	list
```
- See tasks with certain status
``` list [status]
	list todo
	list inprogress
	list done
```
- To exit 
```
	exit
```
