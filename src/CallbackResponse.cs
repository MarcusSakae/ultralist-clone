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

        public CommandResult(string message) : this(true, message)
        {
        }

        public CommandResult() : this(true, "")
        {
        }

    }
}