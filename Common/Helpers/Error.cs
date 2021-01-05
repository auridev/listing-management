namespace Common.Helpers
{
    public abstract class Error
    {
        public string Message { get; }
        private Error(string message)
            => Message = message;

        public sealed class NotFound : Error
        {
            public NotFound(string message)
                : base(message)
            {

            }
        }

        public sealed class Invalid : Error
        {
            public Invalid(string message)
                : base(message)
            {

            }
        }

        public sealed class Unauthorized : Error
        {
            public Unauthorized(string message)
                : base(message)
            {

            }
        }
    }
}
