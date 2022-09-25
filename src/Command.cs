namespace Cheap.Ultralist.Knockoff
{

    public enum ArgumentType
    {
        // For example "help" or "version", which don't require any arguments
        None,
        // For example "add <name> <text>" where text is optional
        // "word" is ended by a single space. 
        // Text is either to the end of arguments or enclosed within quotes.
        WordPlusOptionalText,
        TextArgument
    }

    // Callback should return true/false for success/failure and a string for the result
    // Todo: We should upgrade the response to some "CommandResult" or similar
    public delegate CallbackResponse CommandCallback();

    internal class Command
    {
        public int? Id { get; set; }

        public bool RequiresId { get; set; } = false;

        public string Name { get; set; } = "";

        public string Description = "";

        public bool Exhausts { get; set; } = false;

        public List<string> Aliases = new();

        private CommandCallback _callback;

        public String[] Arguments;

        // Todo: Maybe remove
        public ArgumentType ArgumentType { get; set; } = ArgumentType.None;

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
        internal void AddAlias(string alias)
        {
            Aliases.Add(alias.ToLower());
        }

        // Executes the command
        internal CallbackResponse Run()
        {

            return _callback();
        }
    }
}