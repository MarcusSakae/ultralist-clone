namespace Cheap.Ultralist.Knockoff
{
    public class CallbackResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";

        public CallbackResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}