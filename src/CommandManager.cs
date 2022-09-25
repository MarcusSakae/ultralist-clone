using System.Diagnostics;

namespace Cheap.Ultralist.KnockOff
{
    class CommandManager
    {
        public bool Exhausted { get; private set; } = false;

        // Keeps track if any of the executed commands modifies the tasks. (=="do we need to save?")
        // Todo: Maybe move to task manager. (Why should command manager be aware of tasks?)
        public bool ModifiedTasks { get; private set; } = false;

        // List of all available commands
        private List<Command> _commands = new();

        // List of queued up commands
        private List<Command> _queue = new();

        // List of results from executed commands
        public readonly List<string> Results = new List<string>();

        // Inserts a command to the START of the queue
        public void InsertCommand(Command command)
        {
            _queue.Insert(0, command);
        }

        // Adds a command to the end of the queue
        public void QueueCommand(Command command)
        {
            _queue.Add(command);
        }

        // Adds a command to the end of the queue. Now with arguments!
        public void QueueCommand(Command command, string[] args)
        {
            command.Arguments = args;
            _queue.Add(command);
        }


        // Registers a command
        public void Register(
            string commandName,
            CommandCallback callback,
            string description,
            bool exhausts = false,
            bool modifiesTasks = true)
        {
            var command = new Command(commandName, callback);
            command.Description = description;
            command.Exhausts = exhausts;
            command.ModifiesTasks = modifiesTasks;
            _commands.Add(command);

            // We also try to register the first letter as an alias 
            // It will fail silently if it's already taken.
            RegisterAlias(commandName, commandName[0].ToString());
        }

        // Command callback to show the help text
        public CommandResult ShowHelp(string[] args)
        {
            Results.Add("Ul, cheap ultralist knockoff for the common man.\n");

            var maxlength = _commands.Max(x => x.Name.Length);

            foreach (var command in _commands)
            {
                Results.Add($"  {command.Name.PadRight(maxlength)} {command.Description}");
            }

            return new CommandResult();
        }

        // Register an alias for a command
        //
        // An alias allows the user to run a command with a shorter name
        // For example "ul i" instead of "ul init"
        public void RegisterAlias(string original, string alias)
        {
            // Make sure we are not using the alias already
            if (_commands.Any(c => c.Aliases.Contains(alias)))
            {
                // Do nothing, but continue as usual. 
                return;
            }

            // Find the command we are aliasing
            Command? cmd = _commands.Find(c => c.Name == original);
            if (cmd == null)
            {
                throw new Exception($"You tried to register an alias for a command that doesn't exist: {original}");
            }
            cmd.AddAlias(alias);
        }


        //
        public void ParseArgs(string[] args)
        {
            //  Filter out all "--" flags
            args = args.SkipWhile(a => a.StartsWith("--")).ToArray();

            // No command provided? lets be friendly and redirect to help
            if (args.Length == 0)
            {
                this.Exhausted = true;
                Command? help_command = _commands.Find(c => c.Name == "help");
                if (help_command != null)
                {
                    QueueCommand(help_command);
                }
                return;
            }


            // Find command...
            Command? command = _commands.Find(c => c.Aliases.Contains(args[0]));

            // Command not found?
            if (command == null)
            {
                this.Exhausted = true;
                Results.Add($"Unknown command: {args[0]}");
                return;
            }

            // Command exhausts the command queue? (exhaust = prevents further commands)
            if (command.Exhausts)
            {
                Exhausted = true;
            }

            // Are the tasks modified? (Do we need to save when done?)
            if (command.ModifiesTasks)
            {
                ModifiedTasks = true;
            }

            // Queue up!
            QueueCommand(command, args[1..]);
        }

        // Process the command queue
        // Note tat we might be exhausted but still have a command in the queue,
        // for example "help" adds a command, but exhausts the queue to prevent further commands.
        public void ProcessCommandQueue()
        {
            do
            {
                if (_queue.Count == 0)
                {
                    Exhausted = true;
                    return;
                }

                Command command = _queue[0];

                _queue.RemoveAt(0);
                CommandResult res = command.Run();
                if (res.Success)
                {
                    Results.Add(res.Message);
                }
                else
                {
                    Results.Add("[red]Error[reset]: " + res.Message);
                    this.Exhausted = true;
                }
            } while (!Exhausted);

        }
    }
}