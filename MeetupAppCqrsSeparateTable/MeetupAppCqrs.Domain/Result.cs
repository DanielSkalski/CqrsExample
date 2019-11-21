namespace MeetupAppCqrs.Domain
{
    public class Result
    {
        public bool Success { get; private set; }
        public bool Failed => !Success;
        public string ErrorMessage { get; set; }

        private Result() { }

        public static Result Successful => new Result { Success = true };
        public static Result Failure(string message) => new Result 
        { 
            Success = false, 
            ErrorMessage = message 
        };
    }
}
