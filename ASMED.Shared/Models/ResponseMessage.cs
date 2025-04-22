namespace ASMED.Shared.Models
{
    public class ResponseMessage<T>
    {
        public bool Success { get; private set; }
        public string? Message { get; private set; }
        public T? Data { get; private set; }

        private ResponseMessage() { }

        public static ResponseMessage<T> CreateValidResult(T data, string? message = null)
        {
            return new ResponseMessage<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        public static ResponseMessage<T> CreateValidResult(string? message = null)
        {
            return new ResponseMessage<T>
            {
                Success = true,
                Message = message
            };
        }

        public static ResponseMessage<T> CreateInvalidResult(string message)
        {
            return new ResponseMessage<T>
            {
                Success = false,
                Message = message
            };
        }
    }
}
