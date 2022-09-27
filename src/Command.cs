namespace Cheap.Ultralist.KnockOff
{


    // To be able to use callbacks later, we need to match the function signatures.
    // Thus, we provide a delegate for each type of command.
    //
    // The reasoning is that we can parse and check an id before calling the callback, preventing
    // lots of duplicated code in each callback.
    public delegate CommandResult CommandCallback(string[] args);
    public delegate CommandResult CommandCallbackId(int id, string[] args);

    class Command
    {
        public int? Id { get; set; } = null;

        public string Name { get; set; } = "";

        public string Description = "";

        public bool Exhausts { get; set; } = false;

        public bool ModifiesTasks { get; set; } = false;

        public List<string> Aliases = new();

        public String[] Arguments = new String[0];

        // The callback that is called when the command is executed
        private CommandCallback _callback;

        //
        public Command(string commandName, CommandCallback callback)
        {
            Name = commandName;
            Aliases.Add(commandName.ToLower());
            _callback = callback;
        }

        //
        public Command(CommandCallback callback) : this("", callback)
        {
        }

        // Adds an alias to the command
        public void AddAlias(string alias)
        {
            Aliases.Add(alias.ToLower());
        }

        // Executes the command
        public CommandResult Run()
        {
            return _callback(Arguments);
        }
    }
}