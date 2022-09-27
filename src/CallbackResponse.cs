namespace Cheap.Ultralist.KnockOff
{
    public class CommandResult
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";

        public CommandResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        // Converts TaskResult to CommandResult
        public CommandResult(TaskResult res) : this(res.Success, res.Message)
        {
        }

        // Only message == OK
        public CommandResult(string message) : this(true, message)
        {
        }

        // No argument = OK
        public CommandResult() : this(true, "")
        {
        }

    }
}