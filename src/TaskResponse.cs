namespace Cheap.Ultralist.KnockOff
{
    public class TaskResult
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
        public Task? Task { get; set; } = null;
    }
}