namespace Cheap.Ultralist.Knockoff
{
    internal class TaskResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
        public Task? Task { get; set; }
    }
}