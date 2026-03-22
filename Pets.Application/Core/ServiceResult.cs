namespace Pets.Application.Core
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }
    }
}
