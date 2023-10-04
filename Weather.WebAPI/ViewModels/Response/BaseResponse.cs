namespace Weather.WebAPI.ViewModels.Response
{
    public class BaseResponse
    {
        public bool Success { get; set; } = true;

        public string Message { get; set; } = string.Empty;

        public object? Data { get; set; }
    }
}
