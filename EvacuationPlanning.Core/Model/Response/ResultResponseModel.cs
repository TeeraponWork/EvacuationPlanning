namespace EvacuationPlanning.Core.Model.Response
{
    public class ResultResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public T ErrorMessage { get; set; }

        public static ResultResponseModel<T> SuccessResponse(T value)
        {
            var result = new ResultResponseModel<T>
            {
                Data = value,
                ErrorMessage = default,
                IsSuccess = true
            };
            return result;
        }
        public static ResultResponseModel<T> ErrorResponse(T message)
        {
            var result = new ResultResponseModel<T>
            {
                ErrorMessage = message,
                IsSuccess = false
            };
            return result;
        }
        public static ResultResponseModel<T> ExceptionResponse(T error)
        {
            var result = new ResultResponseModel<T>
            {
                ErrorMessage = error,
                IsSuccess = false
            };
            return result;
        }
    }
}
