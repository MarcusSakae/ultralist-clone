using System.Diagnostics;

namespace Cheap.Ultralist.Knockoff
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            EnableConsoleDebug();

            FileManager fileManager = new();
            TaskManager taskManager = new(fileManager);
            CommandManager commandManager = new();

            RegisterCommands(fileManager, taskManager, commandManager);

            RegisterAliases(commandManager);

            // Parse the arguments into commands
            commandManager.ParseArgs(args);

            // We only want to load tasks if the command manager is not flagged as exhausted
            // i.e.: No need to load anything if the user just wants to see the help message
            if (!commandManager.Exhausted)
            {
                Command command = new Command("LoadTasks", taskManager.LoadTasks);
                commandManager.InsertCommand(command);
            }

            // Execute all commands in the queue
            commandManager.ProcessCommandQueue();

            // Output results
            foreach (var result in commandManager.Results)
            {
                Console.WriteLine(result);
            }

        }


        // Register all commands that the user can call upon
        // Short variable names are used to reduce some of the verbosity.
        private static void RegisterCommands(FileManager fm, TaskManager tm, CommandManager cm)
        {
            cm.Register("add", tm.AddTask, "Add a task");
            cm.Register("addnote", tm.AddNote, "Adds notes to task");
            cm.Register("archive", tm.Archive, "Archives a task");
            cm.Register("auth", ServerManager.Auth, "Authenticates you with server");
            cm.Register("complete", tm.Complete, "Completes a task");
            cm.Register("delete", tm.Delete, "Deletes a task");
            cm.Register("deletenote", tm.DeleteNote, "Deletes a note from a task");
            cm.Register("edit", tm.Edit, "Edits task");
            cm.Register("editnote", tm.EditNote, "Edits a note from a task");
            cm.Register("help", cm.ShowHelp, "Show this help message", true);
            cm.Register("init", fm.Init, "Initialize a new tasks.json in the current directory", true);
            cm.Register("list", tm.List, "List all tasks");
            cm.Register("prioritize", tm.Prioritize, "Prioritizes a task");
            cm.Register("status", tm.Status, "Shows status of tasks");
            cm.Register("sync", ServerManager.Sync, "Syncs tasks with server");
            cm.Register("unarchive", tm.Unarchive, "Un-archives a task");
            cm.Register("uncomplete", tm.Uncomplete, "Un-completes a task");
            cm.Register("unprioritize", tm.Unprioritize, "Un-prioritizes a task");
            cm.Register("version", Version, "Shows version of UL");
            cm.Register("web", ServerManager.Web, "Open your list on ultralist.io");
        }


        // Manually adds aliases for commands where aliases are likely to be missing. 
        //
        // Aliases are otherwise automaticly created from the first letter of the command.
        // If howerver the first letter collides with another command, the alias is not created.
        //
        // An alias allows the user to run a command with a shorter name
        // For example "ul i" instead of "ul init"
        private static void RegisterAliases(CommandManager cm)
        {
            cm.RegisterAlias("addnote", "an");
            cm.RegisterAlias("archive", "ar");
            cm.RegisterAlias("deletenote", "dn");
            cm.RegisterAlias("editnote", "en");
            cm.RegisterAlias("unarchive", "ua");
            cm.RegisterAlias("uncomplete", "uc");
            cm.RegisterAlias("unprioritize", "up");
        }

        // Displays the version of the program
        // Todo: Can we make an automatically incrementing version number for each build?
        private static (bool, string) Version(string[] args)
        {
            return (true, "UL version 0.0.1");
        }

        // Setup logging to console
        private static void EnableConsoleDebug()
        {
            ConsoleTraceListener consoleTracer = new();
            consoleTracer.TraceOutputOptions = TraceOptions.DateTime;
            Trace.Listeners.Add(consoleTracer);
            Trace.Indent();            
        }
    }
}