using System.Diagnostics;
using System.Text.Json;

namespace Cheap.Ultralist.KnockOff
{

    class TaskManager
    {
        List<Task> _tasks = new();

        FileManager _fileManager;

        bool ShowNotes = false;

        // We want to keep a reference around so that the task manager can trigger a save or load.
        public TaskManager(FileManager fileManager, bool showNotes = false)
        {
            _fileManager = fileManager;
            ShowNotes = showNotes;
        }

        //
        // Gets a single task by id (string)
        // Also performs some basic checks to parse id and make sure the task exists.
        //
        private TaskResult GetTask(string presumablyAnId)
        {
            TaskResult result = new();
            int taskId;
            if (!int.TryParse(presumablyAnId, out taskId))
            {
                result.Message = "Could not parse todo ID:" + presumablyAnId;
            }
            else
            {
                result.Task = _tasks.Find(t => t.Id == taskId);
                if (result.Task == null)
                {
                    result.Message = $"No todo with that id.";
                }
                else
                {
                    result.Success = true;
                }
            }
            return result;
        }

        //
        // Loads tasks from a json-string
        //
        public CommandResult LoadTasks(string[] args)
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
                return new CommandResult(false, "tasks.json is not valid JSON");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new CommandResult(false, "Unknown error loading tasks.json");
            }
            if (loadedTasks == null)
            {
                return new CommandResult(false, "Could not load any tasks. Is tasks.json is tasks empty?");
            }
            _tasks = loadedTasks;
            return new CommandResult();
        }

        public CommandResult SaveTasks(string[] args)
        {
            string jsonString = JsonSerializer.Serialize(_tasks);
            _fileManager.Save(jsonString);
            return new CommandResult();
        }


        //
        // Lists all tasks
        //
        public CommandResult List(string[] args)
        {
            string response = "all\n";
            foreach (Task task in _tasks)
            {
                response += task.ToString() + "\n";
                if (ShowNotes)
                {
                    response += task.NotesToString();
                }
            }
            return new CommandResult(response);
        }


        //
        //
        //
        public CommandResult AddNote(string[] args)
        {
            if (args.Length < 2)
            {
                return new CommandResult(false, "You must specify a task id and a note");
            }
            TaskResult res = GetTask(args[0]);
            if (!res.Success)
            {
                return new CommandResult(res.Success, res.Message);
            }
            res.Task!.Notes.Add(string.Join(" ", args[1..]));
            return new CommandResult($"Added note to task [yellow]{res.Task.Id}[reset]");
        }



        //
        // Archives a task
        //
        public CommandResult Archive(string[] args)
        {
            TaskResult res = GetTask(args[0]);
            if (!res.Success)
            {
                return new CommandResult(res.Success, res.Message);
            }
            res.Task!.IsArchived = true;
            return new CommandResult($"Archived task [yellow]{res.Task.Id}[reset]");
        }

        //
        // Unarchive task
        //
        public CommandResult Unarchive(string[] args)
        {
            TaskResult res = GetTask(args[0]);
            if (!res.Success)
            {
                return new CommandResult(res.Success, res.Message);
            }
            res.Task!.IsArchived = false;
            return new CommandResult(true, $"Unarchived task [yellow]{res.Task.Id}[reset]");
        }

        //
        // Adds a new task
        //
        public CommandResult AddTask(string[] args)
        {
            Task task = new Task(string.Join(" ", args));
            task.Id = _tasks.Count + 1;
            _tasks.Add(task);
            return new CommandResult($"Task [yellow]{task.Id}[reset] added.");
        }

        //
        // Marks a task as completed
        //
        public CommandResult Complete(string[] args)
        {
            TaskResult res = GetTask(args[0]);
            if (!res.Success)
            {
                return new CommandResult(res.Success, res.Message);
            }
            res.Task!.IsCompleted = true;
            return new CommandResult($"Marked task [yellow]'{res.Task.Id}'[reset] as completed.");
        }

        //
        // Marks a task as not completed
        //
        public CommandResult Uncomplete(string[] args)
        {
            TaskResult res = GetTask(args[0]);
            if (!res.Success)
            {
                return new CommandResult(res.Success, res.Message);
            }
            res.Task!.IsCompleted = false;
            return new CommandResult($"Marked task [yellow]'{res.Task.Id}'[reset] as not completed.");
        }


        //
        // Deletes a task
        //
        public CommandResult DeleteNote(string[] args)
        {
            TaskResult res = GetTask(args[0]);
            if (!res.Success)
            {
                return new CommandResult(res.Success, res.Message);
            }
            res.Task!.Notes.RemoveAt(int.Parse(args[1]));
            return new CommandResult($"Deleted note [blue]{args[1]}[reset] from task [yellow]{res.Task.Id}[reset]");
        }

        //
        // Edits a note
        //
        public CommandResult EditNote(string[] args)
        {
            TaskResult res = GetTask(args[0]);
            if (!res.Success)
            {
                return new CommandResult(res.Success, res.Message);
            }
            res.Task!.Notes[int.Parse(args[1])] = string.Join(" ", args[2..]);
            return new CommandResult($"Note edited.");
        }

        //
        // Edits a task
        //
        public CommandResult Edit(string[] args)
        {
            TaskResult res = GetTask(args[0]);
            if (!res.Success)
            {
                return new CommandResult(res.Success, res.Message);
            }
            res.Task!.Text = string.Join(" ", args[1..]);
            return new CommandResult($"Task [yellow]{res.Task.Id}[reset] updated.");
        }

        //
        // Delete task
        //
        public CommandResult Delete(string[] args)
        {
            TaskResult res = GetTask(args[0]);
            if (!res.Success)
            {
                return new CommandResult(res.Success, res.Message);
            }
            int id = res.Task!.Id;
            _tasks.Remove(res.Task!);
            return new CommandResult($"Task [yellow]{id}[reset] deleted.");
        }

        //
        // Prioritize task
        //
        public CommandResult Prioritize(string[] args)
        {
            TaskResult res = GetTask(args[0]);
            if (!res.Success)
            {
                return new CommandResult(res.Success, res.Message);
            }
            res.Task!.IsPriority = true;
            return new CommandResult($"Task [yellow]{res.Task.Id}[reset] is now a priority.");
        }

        //
        // Unprioritize task
        //
        public CommandResult Unprioritize(string[] args)
        {
            TaskResult res = GetTask(args[0]);
            if (!res.Success)
            {
                return new CommandResult(res.Success, res.Message);
            }
            res.Task!.IsPriority = false;
            return new CommandResult($"Task [yellow]{res.Task.Id}[reset] is no longer a priority.");
        }


        //
        // Sets status for a task
        //
        public CommandResult Status(string[] args)
        {
            TaskResult res = GetTask(args[0]);
            if (!res.Success)
            {
                return new CommandResult(res.Success, res.Message);
            }
            res.Task!.Status = args[1];
            return new CommandResult($"Status set to [blue]{args[1]}[reset] for task [yellow]{res.Task.Id}[reset]");
        }
    }

}