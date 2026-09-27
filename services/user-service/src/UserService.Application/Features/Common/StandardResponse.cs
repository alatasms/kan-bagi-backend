namespace UserService.Application.Common
{
    public class StandardResponse<T>
    {
        public T? Response { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string ResultCode { get; set; } = "200";
        public string? ResultMessage { get; set; }
    }

}
