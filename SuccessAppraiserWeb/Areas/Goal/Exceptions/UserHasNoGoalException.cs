namespace SuccessAppraiserWeb.Areas.Goal.Exceptions
{
    public class UserHasNoGoalException : Exception
    {
        public UserHasNoGoalException(string? message) : base(message)
        {
            
        }
    }
}
