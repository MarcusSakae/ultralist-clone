using System.Diagnostics;
using System.Text.Json;

namespace Cheap.Ultralist.Knockoff
{



    class TaskManager
    {
        List<Task> _tasks = new();
        FileManager _fileManager;

        // We want to keep a reference around so that the task manager can trigger a save or load.
        public TaskManager(FileManager fileManager)
        {
            _fileManager = fileManager;
        }

        // Loads tasks from a json-string
        public (bool, string) LoadTasks(string[] args)
        {
            // Todo: We want to decouple the filemanager if we can,
            // so we can use something else, (db, http, etc)
            string jsonString = _fileManager.GetJsonString();
            List<Task>? loadedTasks = null;
            try
            {
                loadedTasks = JsonSerializer.Deserialize<List<Task>>(jsonString);
            }
            catch (JsonException e)
            {
                Debug.WriteLine(e);
                return (false, "tasks.json is not valid JSON");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return (false, "Unknown error loading tasks.json");
            }
            if (loadedTasks == null)
            {
                return (false, "Could not load any tasks. Is tasks.json is tasks empty?");
            }
            _tasks = loadedTasks;
            Debug.WriteLine("Tasks loaded!");
            return (true, "");
        }

        //
        //
        //
        private TaskResponse GetTask(string presumablyAnId)
        {
            TaskResponse response = new();
            int taskId;
            if (!int.TryParse(presumablyAnId, out taskId))
            {
                response.Message = "Could not parse todo ID:" + presumablyAnId;
            }
            else
            {
                response.Task = _tasks.Find(t => t.Id == taskId);
                if (response.Task == null)
                {
                    response.Message = $"No todo with that id.";
                }
                else
                {
                    response.Success = true;
                }
            }
            return response;
        }

        //
        // Lists all tasks
        //
        public (bool, string) List(string[] args)
        {
            string response = "all\n";
            foreach (Task task in _tasks)
            {
                response += task.ToString() + "\n";
            }
            return (true, response);
        }


        //
        //
        //
        public (bool, string) AddNote(string[] args)
        {
            TaskResponse res = GetTask(args[0]);
            if (!res.Success)
            {
                return (res.Success, res.Message);
            }
            res.Task!.Notes.Add(string.Join(" ", args[1..]));
            return (true, $"Added note to task {res.Task.Id}");
        }



        //
        // Archives a task
        //
        public (bool, string) Archive(string[] args)
        {
            TaskResponse res = GetTask(args[0]);
            if (!res.Success)
            {
                return (res.Success, res.Message);
            }
            res.Task!.IsArchived = true;
            return (true, $"Archived task {res.Task.Id}");
        }

        //
        // Unarchive task
        //
        public (bool, string) Unarchive(string[] args)
        {
            TaskResponse res = GetTask(args[0]);
            if (!res.Success)
            {
                return (res.Success, res.Message);
            }
            res.Task!.IsArchived = false;
            return (true, $"Unarchived task {res.Task.Id}");
        }

        //
        // Adds a new task
        //
        public (bool, string) AddTask(string[] args)
        {
            Task task = new Task();
            task.Id = _tasks.Count + 1;
            _tasks.Add(task);
            _fileManager.Save(JsonSerializer.Serialize(_tasks));
            return (true, $"Task {_tasks.Count} added");
        }

        //
        // Marks a task as completed
        //
        public (bool, string) Complete(string[] args)
        {
            TaskResponse res = GetTask(args[0]);
            if (!res.Success)
            {
                return (res.Success, res.Message);
            }
            res.Task!.IsCompleted = true;
            return (true, $"Completed task {res.Task.Id}");
        }

        //
        // Marks a task as not completed
        //
        public (bool, string) Uncomplete(string[] args)
        {
            TaskResponse res = GetTask(args[0]);
            if (!res.Success)
            {
                return (res.Success, res.Message);
            }
            res.Task!.IsCompleted = false;
            return (true, $"Uncompleted task {res.Task.Id}");
        }


        //
        // Deletes a task
        //
        public (bool, string) DeleteNote(string[] args)
        {
            TaskResponse res = GetTask(args[0]);
            if (!res.Success)
            {
                return (res.Success, res.Message);
            }
            res.Task!.Notes.RemoveAt(int.Parse(args[1]));
            return (true, $"Deleted note {args[1]} from task {res.Task.Id}");
        }

        //
        // Edits a note
        //
        public (bool, string) EditNote(string[] args)
        {
            TaskResponse res = GetTask(args[0]);
            if (!res.Success)
            {
                return (res.Success, res.Message);
            }
            res.Task!.Notes[int.Parse(args[1])] = string.Join(" ", args[2..]);
            return (true, $"Note edited.");
        }

        //
        // Edits a task
        //
        public (bool, string) Edit(string[] args)
        {
            TaskResponse res = GetTask(args[0]);
            if (!res.Success)
            {
                return (res.Success, res.Message);
            }
            res.Task!.Text = string.Join(" ", args[1..]);
            return (true, $"Task updated.");
        }

        //
        // Delete task
        //
        public (bool, string) Delete(string[] args)
        {
            TaskResponse res = GetTask(args[0]);
            if (!res.Success)
            {
                return (res.Success, res.Message);
            }
            _tasks.Remove(res.Task!);
            return (true, $"Task deleted.");
        }

        //
        // Prioritize task
        //
        public (bool, string) Prioritize(string[] args)
        {
            TaskResponse res = GetTask(args[0]);
            if (!res.Success)
            {
                return (res.Success, res.Message);
            }
            res.Task!.IsPriority = true;
            return (true, $"Task prioritized.");
        }

        //
        // Unprioritize task
        //
        public (bool, string) Unprioritize(string[] args)
        {
            TaskResponse res = GetTask(args[0]);
            if (!res.Success)
            {
                return (res.Success, res.Message);
            }
            res.Task!.IsPriority = false;
            return (true, $"Task unprioritized.");
        }


        //
        // Sets status for a task
        //
        public (bool, string) Status(string[] args)
        {
            TaskResponse res = GetTask(args[0]);
            if (!res.Success)
            {
                return (res.Success, res.Message);
            }
            res.Task!.Status = args[1];
            return (true, $"Status set.");
        }





    }


}