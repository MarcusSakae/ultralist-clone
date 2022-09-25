namespace Cheap.Ultralist.Knockoff
{
    class QueuedCommand
    {
        private Command command { get; set; }
        private string[] arguments { get; set; }

        public QueuedCommand(Command command, string[] arguments)
        {
            this.command = command;
            this.arguments = arguments;
        }

    }
}