namespace Cheap.Ultralist.KnockOff
{
    // Response for when getting a tasks from the task manager
    public class TaskResult
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
        public Task? Task { get; set; } = null;
    }
}