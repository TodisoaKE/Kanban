namespace ApprendreDotNet.model.Response
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; } = true;
        public T? Data { get; set; }
        public string ResponseCode { get; set; } = string.Empty;
        public string ResponseMessage { get; set; } = string.Empty;
        public string ResponseExecutionTime { get; set; } = string.Empty;
        public List<ErrorModel> Errors { get; set; } = [];
    }
}
public record ErrorModel
{
    public string ErrorCode { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}
