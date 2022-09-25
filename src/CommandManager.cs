using System.Diagnostics;

namespace Cheap.Ultralist.Knockoff
{
    class CommandManager
    {
        public bool Exhausted { get; private set; } = false;

        // List of all available commands
        private List<Command> _commands = new();

        // List of queued up commands
        private List<Command> _queue = new();

        public readonly List<string> Results = new List<string>();

        public void InsertCommand(Command command)
        {
            _queue.Insert(0, command);
        }
        public void QueueCommand(Command command)
        {
            _queue.Add(command);
        }

        public void QueueCommand(Command command, string[] args)
        {
            command.Arguments = args;
            _queue.Add(command);
        }


        // Registers an alias for a command
        public void Register(string commandName, CommandCallback callback, string description, bool exhausts = false)
        {
            var command = new Command(commandName, callback);
            command.Description = description;
            command.Exhausts = exhausts;
            _commands.Add(command);

            // We also try to register the first letter as an alias (if it's not already taken)
            RegisterAlias(commandName, commandName[0].ToString()); // Adds an alias "i" for "init"
        }



        //
        //
        public (bool, string) ShowHelp(string[] args)
        {
            Results.Add("Ul, cheap ultralist knockoff for the common man.\n");

            var maxlength = _commands.Max(x => x.Name.Length);

            foreach (var command in _commands)
            {
                Results.Add($"  {command.Name.PadRight(maxlength)} {command.Description}");
            }

            return (true, "");
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
            // No command provided, lets be friendly and redirect to help
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

            // Handle if the command isnt found.
            if (command == null)
            {
                Results.Add($"Unknown command: {args[0]}");
                return;
            }

            // Handle commands that exhaust the queue
            if (command.Exhausts)
            {
                Exhausted = true;
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
                CallbackResponse res = command.Run();
                if (res.Success)
                {
                    Results.Add(res.Message);
                }
                else
                {
                    Results.Add("Error: " + res.Message);
                    this.Exhausted = true;
                }
            } while (!Exhausted);

            Debug.WriteLine("Command queue exhausted");

        }
    }
}